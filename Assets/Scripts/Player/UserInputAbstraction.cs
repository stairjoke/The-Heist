using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class UserInputAbstraction : MonoBehaviour
{
    private Vector3 accelorometerInput = new Vector3(0,0,0);

<<<<<<< HEAD
        public void readAcceleration()
        {
            accelorometerInput = Input.acceleration;
            StartCoroutine(accelerationAsyncRead());
        }
=======
    public void readAcceleration(){
        accelorometerInput = Input.acceleration;
        Debug.Log(accelorometerInput);
        StartCoroutine(accelerationAsyncRead());
    }
>>>>>>> parent of 5c0b977... namespace, acceleration remapping to change sensitivity

    public IEnumerator accelerationAsyncRead(){
        yield return new WaitForEndOfFrame();
        readAcceleration();
    }

    void Start(){
        readAcceleration();
    }

    public Vector3 motionVector()
    {
        /*
         * Translates different input options into one unified vector
        */
        Vector3 motion = new Vector3(0,0,0);

#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxisRaw("Horizontal");
        motion.z = Input.GetAxisRaw("Vertical");
#endif
#if UNITY_IOS
        motion.x = accelorometerInput.x;
        motion.z = accelorometerInput.y;
#endif
        if(motion.magnitude < 0.005f){
            motion = Vector3.zero;
        }
        return motion;
    }

    public static Vector3 targetCoordinateWorldSpace(LayerMask mask)
    {
        /*
         * Translate pointer or finger on screen to 3D coordinates
        */
        return new Vector3();
    }

    public static bool isShooting()
    {
        /*
         * Translate fire inputs from keyboards and touchsreen into bool value
        */
        bool fire = false;
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetAxis("Fire1").Equals(true) || Input.GetAxis("Fire1") > 0.5f){
            fire = true;
        }
#endif
#if UNITY_IOS
#endif
        return fire;
    }
}
