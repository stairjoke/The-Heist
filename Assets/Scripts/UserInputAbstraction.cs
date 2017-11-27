using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputAbstraction : MonoBehaviour
{
    public static Vector3 motionVector()
    {
        /*
         * Translates different input options into one unified vector
        */
        Vector3 motion = new Vector3();
#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxisRaw("Horizontal");
        motion.y = 0f;
        motion.z = Input.GetAxisRaw("Vertical");
#endif
#if UNITY_IOS
#endif
        return motion.normalized; //scale vector if more than one axis is given by user
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
