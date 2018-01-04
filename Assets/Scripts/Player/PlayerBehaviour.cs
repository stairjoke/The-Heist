using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace theHeist
{
    public class PlayerBehaviour : MonoBehaviour
    {

        private NavMeshAgent navigation;
        public Transform UserInputAbstractionEmpty;
        //private UserInputAbstraction abstractInputs;
        private int motionFingerId = -1;

        void Start()
        {
            navigation = GetComponent<NavMeshAgent>();
            //abstractInputs = UserInputAbstractionEmpty.GetComponent<UserInputAbstraction>();
        }

        //Moevement
        private bool fingerTouchingGameObject(Touch finger, GameObject target, bool useUserInteractibleLayerMask = true){
            //trace from camera to game
            var ray = Camera.current.ScreenPointToRay(finger.position);
            RaycastHit[] hits;
            bool hitMe = false;

            //Raycast from finger to scene
#if DEBUG
            if(Camera.current.transform.position.y < 35f || Camera.current.transform.position.y > 37f){
                Debug.LogError("RAYCAST for touch to motion only tests 3 units, make sure your near and far plane are set as narrow as possible!");
            }
#endif
            //collect all collisions of raycast
            if(useUserInteractibleLayerMask){
                hits = Physics.RaycastAll(ray, 3, LayerMask.GetMask("UserInteractible"));
            }else{
                hits = Physics.RaycastAll(ray, 3);
            }
            //test if once of the collisions hit the target
            foreach(RaycastHit hit in hits){
                LinkToToplevel top = hit.transform.GetComponent<LinkToToplevel>();
                if(top.getToplevel().gameObject.Equals(target)){
                    hitMe = true;
                }
            }

            return hitMe;
        }
        private bool addPointToPlayerMotionPath(Vector2 screenCoordinates, bool touchEnded = false){
            /* convert to world coordinates (see "fingerTouchingGameObject")
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
            return false;
        }
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
                if(motionFinger.fingerId != -1){
                    /*
                     * 1: get touchPhase
                    */
                    switch (motionFinger.phase)
                    {
                        case TouchPhase.Ended:
                            //Set last known coordinate as point
                            motionFingerId = -1;
                            break;
                        case TouchPhase.Canceled:
                            //Set last known coordinate as point
                            motionFingerId = -1;
                            break;
                        case TouchPhase.Moved:
                            //Push coordinate to path
                            break;
                    }
                }else{
                    //did not get valid touch (no finger on player)
                    //if there is somewhere to go, go there
                }
            }else{ //User isn't interacting
                motionFingerId = -1; //currently not tracking a finger
            }
            return false;
        }
        private void excecutePlayerMovement(){
            /* Method makes player follow motion path unless new motion path is
             * entered or end of path has been reached
            */

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
