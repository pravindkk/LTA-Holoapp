using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LTA.Holoapp
{
    public class CustomVision : MonoBehaviour
    {

        public static async Task MakePredictionRequest(string imagePath)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", "d6f3b065201b42279bd91c28e06a38a3");

            string url = "https://ltaobjectdetection-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/5651bec6-f763-4d91-b61d-3834dea609e6/detect/iterations/ObjectDetect1/image";

            HttpResponseMessage response;

            byte[] byteData = GetBytesFromImage(imagePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var resContent = await response.Content.ReadAsStringAsync();
                Debug.Log(resContent);
                Rootobject res = JsonUtility.FromJson<Rootobject>(resContent);
                PlaceLabel.Instance.CreateLabel(res);
                //SceneOrganiser.Instance.PlaceAnalysisLabel();
                //SceneOrganiser.Instance.FinaliseLabel(res);
                Debug.Log(res.predictions[0].tagName + ", " + res.predictions[0].probability);
                Debug.Log(res.predictions[1].tagName + ", " + res.predictions[1].probability);
            }
        }

        private static byte[] GetBytesFromImage(string imagePath)
        {
            FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            BinaryReader binReader = new BinaryReader(fs);
            return binReader.ReadBytes((int)fs.Length);
            
        }
    }
}
