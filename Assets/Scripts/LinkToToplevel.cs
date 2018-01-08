using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace theHeist
{
    public class LinkToToplevel : MonoBehaviour
    {
        [Help("Reference top level transform on child Objects with collider")]
        public Transform top;
        public Transform getToplevel()
        {
            return top;
        }
    }
}