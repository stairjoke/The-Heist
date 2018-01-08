using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

namespace theHeist
{
    public class UserInputAbstraction : MonoBehaviour
    {
        private Vector3 accelorometerInput = new Vector3(0, 0, 0);

        public void readAcceleration()
        {
            accelorometerInput = Input.acceleration;
            StartCoroutine(accelerationAsyncRead());
        }

        public IEnumerator accelerationAsyncRead()
        {
            yield return new WaitForEndOfFrame();
            readAcceleration();
        }

        void Start()
        {
            readAcceleration();
        }

        public Vector3 motionVector()
        {
            /*
             * Translates different input options into one unified vector
            */
            Vector3 motion = new Vector3(0, 0, 0);

#if UNITY_EDITOR || UNITY_STANDALONE
            motion.x = Input.GetAxisRaw("Horizontal");
            motion.z = Input.GetAxisRaw("Vertical");
#endif
#if UNITY_IOS
            motion.x = accelorometerInput.x;
            motion.z = accelorometerInput.y;
            motion = new Vector3(
                motion.x.Remap(-0.3f, 0.3f, -1, 1),
                motion.y,
                motion.z.Remap(-0.3f, 0.3f, -1, 1)
            );
#endif
            if (motion.magnitude < 0.05f)
            {
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
            if (Input.GetAxis("Fire1").Equals(true) || Input.GetAxis("Fire1") > 0.5f)
            {
                fire = true;
            }
#endif
#if UNITY_IOS
#endif
            return fire;
        }
    }
}
