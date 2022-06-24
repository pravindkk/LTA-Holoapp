using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LTA.Holoapp
{

    public class TapListener : MonoBehaviour
	{
        PointerHandler pointerHandler;
        CustomVision vision;
        private void OnEnable()
        {
            pointerHandler = gameObject.AddComponent<PointerHandler>();
            pointerHandler.OnPointerDown.AddListener((evt) => {
                //ImageCapture.CameraExecute();
            });
            // Make this a global input handler, otherwise this object will only receive events when it has input focus
            CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }   

        private void OnDestroy()
        {
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }
    }
}
