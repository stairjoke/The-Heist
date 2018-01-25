using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace theHeist
{
    public class PlayerBehaviour : MonoBehaviour
    {

        private NavMeshAgent navigation;
        //public Transform UserInputAbstractionEmpty;

        void Start()
        {
            navigation = GetComponent<NavMeshAgent>();
            //abstractInputs = UserInputAbstractionEmpty.GetComponent<UserInputAbstraction>();
        }

        private RaycastHit[] fingerToRaycastHit(Touch finger, LayerMask mask){
            var pos = finger.position;
            if(pos.Equals(new Vector2(0,0))){
                return new RaycastHit[0];
            }
            return Physics.RaycastAll(
                Camera.current.ScreenPointToRay(pos),
                50,
                mask
            ); //Camera distance less than 50
        }
        private bool fingerTouchingGameObject(Touch finger, GameObject target){
            bool hitMe = false;

            //Get all hits of finger-to-world ray and test
            foreach(RaycastHit hit in fingerToRaycastHit(finger, LayerMask.GetMask("UserInteractible"))){
                LinkToToplevel top = hit.transform.GetComponent<LinkToToplevel>();
                if(top.getToplevel().gameObject.Equals(target)){
                    hitMe = true;
                }
            }
            return hitMe;
        }


        private float minDistance = 3f;
        public Transform waypointPrefab;
        public Transform waypointPathObject;
        public Transform waypointPathErrorGameObject;
        private List<Transform> playerMotionPath = new List<Transform>();

        private void addToPlayerMotionPath(Touch finger){
            RaycastHit[] hits = fingerToRaycastHit(
                finger,
                LayerMask.GetMask("screenToWorldRaycastTarget")
            );
            //touch point = transform position if user did not hit gameObject
            Vector3 point = (hits.Length > 0) ? hits[0].point : this.transform.position;
            //lastPoint = transform position is there is no last point
            Vector3 lastPoint = (playerMotionPath.Count <= 0) ? this.transform.position : playerMotionPath[playerMotionPath.Count - 1].transform.position;
            float distance = Vector3.Distance(point, lastPoint);
            bool touchEnded = false;

            //if the finger was removed from the display during the last frame
            if(finger.phase == TouchPhase.Ended || finger.phase == TouchPhase.Canceled){
                touchEnded = true;

                //hide line from last point to finger
                waypointPathObject.GetComponent<LineRenderer>().enabled = false;
            }

            //if the finger began touching the display during the last frame
            if(finger.phase == TouchPhase.Began && fingerTouchingGameObject(finger, this.gameObject)){
                //show line from last point to finger
                waypointPathObject.GetComponent<LineRenderer>().enabled = true;

                //remove all the dots
                foreach(Transform waypoint in playerMotionPath){
                    Destroy(waypoint.gameObject);
                }
                playerMotionPath.Clear(); //remove all the position from the path
            }

            //position the line from point to finger
            Vector3[] waypointLinePositions = new Vector3[2];
            waypointLinePositions[0] = lastPoint;
            waypointLinePositions[1] = point;
            waypointPathObject.GetComponent<LineRenderer>().SetPositions(waypointLinePositions);
            waypointPathErrorGameObject.GetComponent<LineRenderer>().SetPositions(waypointLinePositions);
            //don't show errors yet
            var waypointErrors = false;


            //if distance is sufficient for nw point in path
            if(distance > minDistance || touchEnded || playerMotionPath.Count < 1){
                //MISING: needs to check for collision in between last and next point
                var waypointPathHits = Physics.Linecast(
                    new Vector3(
                        lastPoint.x,
                        1,
                        lastPoint.z
                    ),
                    new Vector3(
                        point.x,
                        1,
                        point.z
                    ));
                
                if (waypointPathHits) //No collision other than self
                {
                    //now show the errors
                    waypointErrors = true;
                }else{
                    Transform waypoint = Instantiate(waypointPrefab, point, Quaternion.LookRotation(Vector3.forward));
                    playerMotionPath.Add(waypoint);
                }
            }
            waypointPathErrorGameObject.GetComponent<LineRenderer>().enabled = waypointErrors;
        }

        private int motionFingerId = -1;
        private bool planPlayerMovement(){
            /* if user touches player with finger
             * > allow for new path
             * if user does not start drag event while touching player
             * > follow existing path if available
            */
                if(Input.touchCount > 0){ //User is interacting

                /* This is a potential source for a bug: touch count may
                 * not be at zero while user shoots but does not input
                 * new motion path!
                */

                //find the right touch/finger for player movement
                Touch motionFinger = new Touch();
                if (motionFingerId != -1)
                {
                    //we already know which touch/finger is to be used
                    foreach(Touch finger in Input.touches){
                        if(finger.fingerId == motionFingerId){
                            motionFinger = finger;
                            break;
                        }
                    }
                }else{
                    //we still need to figure out which touch/finger is the
                    //right one to take input from
                    foreach(Touch finger in Input.touches){
                        if(fingerTouchingGameObject(finger, this.gameObject)){
                            motionFinger = finger;
                            motionFingerId = motionFinger.fingerId;
                            break;
                        }
                    }
                }

                //At this point we should have a touch ready for use
                addToPlayerMotionPath(motionFinger);
            }else{ //User isn't interacting
                motionFingerId = -1; //currently not tracking a finger
                //tapping is not registered in motion methods, interrupted touches are ignored

                waypointPathErrorGameObject.GetComponent<LineRenderer>().enabled = false;
            }
            return false;
        }
        private void excecutePlayerMovement(){
            /* Method makes player follow motion path unless new motion path is
             * entered or end of path has been reached
            */
            planPlayerMovement();
            if(playerMotionPath.Count > 0 && navigation.remainingDistance < minDistance/4){
                navigation.SetDestination(playerMotionPath[0].position);
                Destroy(playerMotionPath[0].gameObject);
                playerMotionPath.RemoveAt(0);
            }
        }

        //Fighting
        private void shoot()
        {
            if (UserInputAbstraction.isShooting())
            {
                //GetComponent -> Weapon -> isFiring=true
            }
            else
            {
                //GetComponent -> Weapon -> isFiring=false
            }
        }

        // Update is called once per frame
        void Update()
        {
            shoot(); //First shoot
            excecutePlayerMovement(); //movement
        }
    }
}
