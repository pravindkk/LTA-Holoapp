using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class TodoClose : MonoBehaviour
    {
        public void CloseTodo(GameObject item)
        {
            Destroy(item);
        }
    }
}
