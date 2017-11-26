using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputAbstraction : MonoBehaviour
{
    public static Vector2 userMotionVector()
    {
        Vector2 motion = new Vector2();
#if UNITY_EDITOR || UNITY_STANDALONE
        motion.x = Input.GetAxis("Horizontal");
        motion.y = Input.GetAxis("Vertical");
#endif

#if UNITY_IOS
#endif
        return motion;
    }
    public static Vector3 usertargetCoordinateWorldSpace(LayerMask mask){

        return new Vector3();
    }
}
