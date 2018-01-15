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

        //Moevement
        private RaycastHit[] fingerToRaycastHit(Touch finger, LayerMask mask){
            var ray = Camera.current.ScreenPointToRay(finger.position);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 50, mask);
            return hits;
        }
        private bool fingerTouchingGameObject(Touch finger, GameObject target){
            //trace from camera to game
            bool hitMe = false;
            RaycastHit[] hits;
            //collect all collisions of raycast
            hits = fingerToRaycastHit(finger, LayerMask.GetMask("UserInteractible"));

            //test if once of the collisions hit the target
            foreach(RaycastHit hit in hits){
                LinkToToplevel top = hit.transform.GetComponent<LinkToToplevel>();
                if(top.getToplevel().gameObject.Equals(target)){
                    hitMe = true;
                }
            }

            return hitMe;
        }

        public float minDistance = 2;
        private List<Vector3> playerMotionPath = new List<Vector3>();
        private void addToPlayerMotionPath(Touch finger){
            Debug.Log("TRY: Add waypoint");
            /* convert to world coordinates
             * see if there are obstacles between this point and last point
             * > only accept points with no obstacles, to avoid cheating by
             * > dragging across a labyrinth wall
             * 
             * test if the distance to last point in List is big enough OR if
             * the touch ended
             * > add to point list
             * 
             * if point added to list return true, else return false
            */
            RaycastHit[] hits = fingerToRaycastHit(finger, LayerMask.GetMask("screenToWorldRaycastTarget"));
            Vector3 point = (hits.Length > 0) ? hits[0].point : this.transform.position; //works
            Vector3 lastPoint = (playerMotionPath.Count <= 0) ? this.transform.position : playerMotionPath[playerMotionPath.Count - 1];
            float distance = Vector3.Distance(point, lastPoint);
            bool touchEnded = false;
            if(finger.phase == TouchPhase.Ended || finger.phase == TouchPhase.Canceled){
                touchEnded = true;
                Debug.Log("Touch Ended");
            }
            //If this is a new gesture, clear the old path
            if(finger.phase == TouchPhase.Began){
                Debug.Log("New touch, clearing waypoint path");
                playerMotionPath.Clear();
            }

            if((distance > minDistance || touchEnded)){ //removed, because waypoints currently are INSIDE the floor plane and therefore will always have a collider in beteween but never touch a wall -> && Physics.Raycast(lastPoint, point, distance, LayerMask.GetMask("StaticObjects"))
                /* Translation:
                 * if distance sufficient or touch ended, test if new point leads thrugh a wall
                */
                playerMotionPath.Add(point);
                Debug.Log("PlayerMotionPath.Count: " + playerMotionPath.Count);
            }
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
                            Debug.Log("Finger touching game object"); //works
                            motionFinger = finger;
                            motionFingerId = motionFinger.fingerId;
                            break;
                        }
                    }
                }

                //At this point we should have a touch ready for use
                Debug.Log("motionFingerId: " + motionFingerId);
                addToPlayerMotionPath(motionFinger);
            }else{ //User isn't interacting
                motionFingerId = -1; //currently not tracking a finger
                //tapping is not registered in motion methods, interrupted touches are ignored
            }
            return false;
        }
        private void excecutePlayerMovement(){
            /* Method makes player follow motion path unless new motion path is
             * entered or end of path has been reached
            */
            planPlayerMovement();
            if(playerMotionPath.Count > 0 && navigation.remainingDistance < minDistance/4){
                navigation.SetDestination(playerMotionPath[0]);
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
