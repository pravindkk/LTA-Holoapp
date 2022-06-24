using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.UI;

namespace LTA.Holoapp
{
    public class PlaceLabel : MonoBehaviour
    {
        PointerHandler pointerHandler;
        public static PlaceLabel Instance;
        public Transform parent;
        //public GameObject label;
        //GameObject quad;
        //Renderer quadRenderer;
        private static float probabilityThreshold = 0.7f;
        public GameObject startImageCaptureButton;
        private GameObject box;
        private Renderer quadRenderer;
        private Bounds quadBounds;
        //internal Transform lastLabelPlaced;
        //internal TextMeshPro lastLabelPlacedText;
        private void Awake()
        {
            // Use this class instance as singleton
            Instance = this;
            startImageCaptureButton.GetComponent<Interactable>().OnClick.AddListener(delegate
            {
                ImageCapture.CameraExecute();
            });

            box = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadRenderer = box.GetComponent<Renderer>() as Renderer;
            quadBounds = quadRenderer.bounds;

            Material m = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));

            quadRenderer.material = m;

            float transparency = 0f;
            quadRenderer.material.color = new Color(1, 1, 1, transparency);

            //box.transform.parent = transform;
            //box.transform.rotation = transform.rotation;

            // The quad is positioned slightly forward in font of the user
            box.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

            // The quad scale as been set with the following value following experimentation,  
            // to allow the image on the quad to be as precisely imposed to the real world as possible
            box.transform.localScale = new Vector3(3f, 1.65f, 1f);
            box.transform.parent = parent;
        }
            // Start is called before the first frame update
        private void OnEnable()
        {
            
            pointerHandler = gameObject.AddComponent<PointerHandler>();
            pointerHandler.OnPointerDown.AddListener((evt) =>
            {
                //ImageCapture.Instance.CameraExecute();
            });
            // Make this a global input handler, otherwise this object will only receive events when it has input focus
            CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }

        private void OnDestroy()
        {
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }

        public void CreateLabel(Rootobject jsonObject)
        {
            //parent = GameObject.FindGameObjectWithTag("ObjDetectParent").transform;
            if (jsonObject.predictions == null)
            {
                Debug.Log("There are no predictions!!");
                return;
            }

            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = jsonObject.predictions.OrderBy(p => p.probability).ToList().FindAll(e => e.probability > probabilityThreshold);

            var box = GameObject.CreatePrimitive(PrimitiveType.Quad);
            var quadRenderer = box.GetComponent<Renderer>() as Renderer;
            Bounds quadBounds = quadRenderer.bounds;

            Material m = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));

            quadRenderer.material = m;

            float transparency = 0f;
            quadRenderer.material.color = new Color(1, 1, 1, transparency);

            //box.transform.parent = transform;
            //box.transform.rotation = transform.rotation;

            // The quad is positioned slightly forward in font of the user
            box.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

            // The quad scale as been set with the following value following experimentation,  
            // to allow the image on the quad to be as precisely imposed to the real world as possible
            box.transform.localScale = new Vector3(3f, 1.65f, 1f);
            box.transform.parent = parent;

            foreach (Prediction predict in sortedPredictions)
            {


                //Prediction bestPrediction = new Prediction();
                //bestPrediction = sortedPredictions[sortedPredictions.Count - 1];

                //if (bestPrediction.probability > probabilityThreshold)
                //{



                


                GameObject label = new GameObject();
                label.transform.parent = box.transform;
                var labelText = label.gameObject.AddComponent<TextMeshPro>();
                RectTransform rt = label.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0.1f, 0.1f);
                labelText.fontSize = 0.2f;
                //labelText.color = new Color(0,0,0,255);
                labelText.text = predict.tagName;
                label.transform.localPosition = new Vector3(-0.3812f, 0.4631f, 0);
                label.transform.localPosition = CalculateBoundingBoxPosition(quadBounds, predict.boundingBox);
                Debug.Log(label.transform.localPosition);
            }    
                




            //lastLabelPlaced.transform.parent = quad.transform;
                

            //lastLabelPlacedText.text = bestPrediction.tagName;



            //}
        }

        public Vector3 CalculateBoundingBoxPosition(Bounds b, Boundingbox boundingBox)
        {
            Debug.Log($"BB: left {boundingBox.left}, top {boundingBox.top}, width {boundingBox.width}, height {boundingBox.height}");

            double centerFromLeft = boundingBox.left + (boundingBox.width / 2);
            double centerFromTop = boundingBox.top + (boundingBox.height / 2);
            Debug.Log($"BB CenterFromLeft {centerFromLeft}, CenterFromTop {centerFromTop}");

            double quadWidth = b.size.normalized.x;
            double quadHeight = b.size.normalized.y;
            Debug.Log($"Quad Width {b.size.normalized.x}, Quad Height {b.size.normalized.y}");

            double normalisedPos_X = (quadWidth * centerFromLeft) - (quadWidth / 2);
            double normalisedPos_Y = (quadHeight * centerFromTop) - (quadHeight / 2);

            return new Vector3((float)normalisedPos_X, (float)normalisedPos_Y, 2.0f);
        }

    }
}
