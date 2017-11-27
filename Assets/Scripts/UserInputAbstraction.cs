using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputAbstraction : MonoBehaviour
{
    public static Vector3 userMotionVector()
    {
        Vector3 motion = new Vector3();
#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxis("Horizontal");
        motion.y = 0f;
        motion.z = Input.GetAxis("Vertical");
#endif

#if UNITY_IOS
#endif
        return motion.normalized;
    }
    public static Vector3 usertargetCoordinateWorldSpace(LayerMask mask){

        return new Vector3();
    }
}
