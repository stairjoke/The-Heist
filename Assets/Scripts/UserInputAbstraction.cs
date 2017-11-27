using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputAbstraction : MonoBehaviour
{
    public static Vector3 motionVector()
    {
        Vector3 motion = new Vector3();
#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxisRaw("Horizontal");
        motion.y = 0f;
        motion.z = Input.GetAxisRaw("Vertical");
#endif
#if UNITY_IOS
#endif
        return motion.normalized;
    }

    public static Vector3 targetCoordinateWorldSpace(LayerMask mask)
    {

        return new Vector3();
    }

    public static bool isShooting()
    {
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
