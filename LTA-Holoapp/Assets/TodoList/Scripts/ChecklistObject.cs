using System.Collections;
using System.Collections.Generic;
using TMPro;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class ChecklistObject : MonoBehaviour
    {
        public string objName;
        public bool toggle;
        public int index;


        private TextMeshPro itemText;

        private void Start()
        {
            itemText = GetComponentInChildren<TextMeshPro>();
            //itemText.text = objName;
        }

        /// <summary>
        /// Sets the ChecklistObject info from the ChecklistManager class
        /// </summary>
        public void SetObjectInfo(string name, bool toggle, int index)
        {
            this.objName = name;
            this.toggle = toggle;
            this.index = index;
        }
    }
}
