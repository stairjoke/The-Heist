using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class UserInputAbstraction : MonoBehaviour
{
    private Vector3 motion = new Vector3();
    private Vector3 lastCalledMotion = new Vector3(0,0,0);

    private static Vector3 dampen(Vector3 vector, float minimumMagnitude){
        if(vector.normalized.magnitude <= minimumMagnitude){
            return new Vector3(0, 0, 0);
        }else{
            return vector;
        }
    }

    public Vector3 motionVector()
    {
        /*
         * Translates different input options into one unified vector
        */
#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxisRaw("Horizontal");
        motion.y = 0f;
        motion.z = Input.GetAxisRaw("Vertical");
#endif
#if UNITY_IOS
        motion.x = Input.acceleration.x;
        motion.y = 0f;
        motion.z = Input.acceleration.y;
#endif
        motion = motion * Time.deltaTime;
        motion = motion + lastCalledMotion;
        lastCalledMotion = dampen(motion.normalized, 0.1f);
        return lastCalledMotion;
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
