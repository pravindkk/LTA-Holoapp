using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityEngine.XR.WSA.Input;
//using UnityEngine.XR.WSA.WebCam;

namespace LTA.Holoapp
{
    public class ImageCapture : MonoBehaviour
    {
        //PointerHandler pointerHandler;

        public static ImageCapture Instance;
        static string folderPath;
        /// <summary>
        /// Photo Capture object
        /// </summary>
        static PhotoCapture photoCaptureObject = null;
        static Texture2D targetTexture = null;

        /// <summary>
        /// Keep counts of the taps for image renaming
        /// </summary>
        private int captureCount = 0;

        /// <summary>
        /// Flagging if the capture loop is running
        /// </summary>
        internal bool captureIsActive;
        private static string filePath;

        private void Awake()
        {
            Instance = this;
        }

        //private void OnEnable()
        //{
        //    pointerHandler = gameObject.AddComponent<PointerHandler>();
        //    pointerHandler.OnPointerDown.AddListener((evt) =>
        //    {
        //        CameraExecute();
        //    });
        //    // Make this a global input handler, otherwise this object will only receive events when it has input focus
        //    CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        //}

        //private void OnDestroy()
        //{
        //    CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        //}

        // Start is called before the first frame update
        void Start()
        {
            folderPath = Application.persistentDataPath + "/pred_photos";
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            DirectoryInfo info = new DirectoryInfo(folderPath);
            var fileInfo = info.GetFiles();
            foreach (var file in fileInfo)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                    Debug.LogFormat("Cannot delete file: ", file.Name);
                }
            }

        }

        public static void CameraExecute()
        {
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height, TextureFormat.ARGB32, false);

            // Create a PhotoCapture object
            PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
                photoCaptureObject = captureObject;
                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                // Activate the camera
                photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                    // Take a picture
                    var tempFileName = string.Format(@"CapturedImage{0}_n.jpg", Time.time);

                    filePath = System.IO.Path.Combine(folderPath, tempFileName);
                    var tempFilePathAndName = filePath;
                    Debug.Log("Saving photo to " + tempFileName);
                    try
                    {
                        photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToMemory);
                    } catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                    //photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                });
            });
        }

        async static void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result)
        {

            // Deactivate our camera
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            await CustomVision.MakePredictionRequest(filePath);
        }

        static void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            // Shutdown our photo capture resource
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
    }
}
