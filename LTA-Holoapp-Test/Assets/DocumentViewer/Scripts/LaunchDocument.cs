using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if WINDOWS_UWP
using System;
using Windows.Storage;
using Windows.System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
#endif

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class LaunchDocument : MonoBehaviour
    {
        List<string> allPDF = new List<string>();

        private void Start()
        {
            //string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string[] files = System.IO.Directory.GetFiles(filePath);
            //foreach (string file in files)
            //{
            //    if (file.Substring(file.Length - 3) == "pdf")
            //    {
            //        allPDF.Add(System.IO.Path.GetFileName(file));
            //        Debug.Log(System.IO.Path.GetFileName(file));
            //    }

            //}

        }
        public async void PressedButton()
        {
            var uri = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Operation-Checklist.pdf");
            Debug.Log(uri);
#if !UNITY_EDITOR && UNITY_WSA_10_0

                    UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                    {
                        

                        await Launcher.LaunchUriAsync(uri);



                        //var filepicker = new FileOpenPicker();
                        //filepicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        //filepicker.FileTypeFilter.Add(".pdf");

                        //var file = await filepicker.PickSingleFileAsync();
                        //await Launcher.LaunchFileAsync(file);

                        Debug.Log("works");
                    }, false);



#else
            UnityEngine.WSA.Launcher.LaunchUri(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Operation-Checklist.pdf", false);

#endif
        }
    }
}
