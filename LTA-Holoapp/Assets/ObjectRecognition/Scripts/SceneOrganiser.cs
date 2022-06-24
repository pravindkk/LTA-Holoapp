using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LTA.Holoapp
{
    public class SceneOrganiser : MonoBehaviour
    {
        public Transform parent;
        public GameObject imageCaptureButton;

        public static SceneOrganiser Instance;

        private GameObject quad;


        private void Awake()
        {
            Instance = this;

            imageCaptureButton.GetComponent<Interactable>().OnClick.AddListener(delegate
            {
                ImageCapture.CameraExecute();
            });

            quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        }

        private void OnDestroy()
        {
            Destroy(quad);
        }


    }
}
