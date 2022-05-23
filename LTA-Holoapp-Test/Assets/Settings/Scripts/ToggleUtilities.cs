using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class ToggleUtilities : MonoBehaviour
    {
        public void Toggle(GameObject utilities)
        {
           // Transform t = utils as Transform;
           // GameObject utilities = t.gameObject;
            utilities.SetActive(!utilities.activeSelf);
        }
    }
}
