using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class OpenCheckLists : MonoBehaviour
    {
        public void OpenLists(GameObject checklistMenu)
        {
            checklistMenu.SetActive(true);
        }
        public void CloseLists(GameObject checklistMenu)
        {
            checklistMenu.SetActive(false);
        }
    }
}
