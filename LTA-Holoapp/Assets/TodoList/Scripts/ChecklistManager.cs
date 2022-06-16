using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Net.Http;
using System.Text;

#if WINDOWS_UWP
using System;
using Windows.Storage;
using Windows.System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using System.Diagnostics;
#endif

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class ChecklistManager : MonoBehaviour
    {
        public Transform content;
        public GameObject checklistItemPrefab;
        public string filename;

        public Interactable saveButton;
        public TextMeshPro title;

        private float yCoord = 0;


        string filePath;

        private List<ChecklistObject> checklistObjects = new List<ChecklistObject>();

        private InputField[] addInputFields;

        public class ChecklistItem
        {
            public string objName;
            public bool toggle;
            public int index;

            public ChecklistItem(string name, bool toggle, int index)
            {
                this.objName = name;
                this.toggle = toggle;
                this.index = index;
            }

        }

        //private void Start()
        //{

        //}


        /// <summary>
        /// Opens the specific checklist
        /// </summary>
        public void Creation()
        {
            filePath = Application.persistentDataPath + "/checklists/checklists/" + filename;
            LoadJSONData();
            //addInputFields = addPanel.GetComponentsInChildren<InputField>();
            saveButton.OnClick.AddListener(delegate
            {
                SaveAsNewJson();
            });
            title.text = filename.Remove(filename.Length - 4);
        }


        /// <summary>
        /// The filename can be set by the Checklist class
        /// </summary>
        public void SetFileName(string filename)
        {
            this.filename = filename;
        }
        
        private string GetContents()
        {
            string contents = "";

            for (int i = 0; i < checklistObjects.Count - 1; i++)
            {
                ChecklistItem temp = new ChecklistItem(checklistObjects[i].objName, checklistObjects[i].toggle, checklistObjects[i].index);
                contents += JsonUtility.ToJson(temp) + '\n';
            }
            return contents;
        }

        private string GetNewFileName()
        {
            string showFile = filename.Remove(filename.Length - 4);
            var dateTime = DateTime.Now;
            var newFileName = showFile + "_" + dateTime.ToString("ddMMyy") + "_" + dateTime.ToString("HHmm");
            return newFileName;
        }

        async void SaveAsNewJson()
        {
            
            UnityEngine.Debug.Log("start save");
            string contents = GetContents();
            string newFileName = GetNewFileName();

            

            

            

#if !UNITY_EDITOR && UNITY_WSA_10_0
            StorageFolder myDocuments = KnownFolders.DocumentsLibrary;
            StorageFolder savedChecklists = await myDocuments.CreateFolderAsync("saved_checklists", CreationCollisionOption.OpenIfExists);
            StorageFile newFile = await savedChecklists.CreateFileAsync(newFileName,CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteTextAsync(newFile, contents);
            SendNewJson(newFile.Name);

#endif
            UnityEngine.Debug.Log("saved as new json");

        }

        async void SendNewJson(string newFileName)
        {
            var accessToken = Graph.ACCESS_TOKEN;
            UnityEngine.Debug.Log("start send");

            //string newFileName = GetNewFileName();
            string contents = GetContents();

            var httpContent = new StringContent(contents, Encoding.UTF8, "text/plain");

            var url = $"https://graph.microsoft.com/v1.0/users/ltaholo@ltaholotestoutlook.onmicrosoft.com/drive/root:/saved_checklists/{newFileName}.txt:/content";
            //UnityEngine.Debug.Log(url);
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PutAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            //UnityEngine.Debug.Log(responseContent);

        }

        /// <summary>
        /// Creates new checklist line items with the ChecklistObject class
        /// </summary>
        public void CreateCheckListItems(string name, bool toggle, int loadIndex = 0, bool loading = false, bool last = false)
        {
            GameObject item = Instantiate(checklistItemPrefab);

            item.transform.SetParent(content);
            ChecklistObject itemObject = item.GetComponent<ChecklistObject>();
            int index = loadIndex;
            //Debug.Log(itemObject.index);
            if (!loading)
                index = checklistObjects.Count;

            
            itemObject.SetObjectInfo(name, toggle, index);
            itemObject.GetComponentInChildren<TextMeshPro>().text = name;
            itemObject.transform.localScale = new Vector3(0.3762138f, 0.398381f, 1);
            //float localY = (float)(itemObject.transform.localPosition.y - yCoord * 0.03);
            //Debug.Log(localY);
            itemObject.transform.localPosition = new Vector3(0.01f, -0.009999983f - yCoord*0.06f, 0);
            yCoord++;
            checklistObjects.Add(itemObject);
            ChecklistObject temp = itemObject;
            itemObject.GetComponent<Interactable>().IsToggled = toggle;
            itemObject.GetComponent<Interactable>().OnClick.AddListener(delegate
            {
                CheckItem(temp);
            });
            if (last == true)
            {
                item.SetActive(false);
            }
            if (!loading)
            {
                SaveJSONData();
                //SwitchMode(0);
            }
        }

        /// <summary>
        /// Deletes the ChecklistObject in the checklist
        /// </summary>
        void CheckItem(ChecklistObject item)
        {
            //checklistObjects.Remove(item);
            item.toggle = !item.toggle;
            //SaveJSONData();
            //Destroy(item.gameObject);
            
        }

        void SaveJSONData()
        {
            string contents = "";
            //Debug.Log(checklistObjects[0].objName);
            for (int i = 0; i < checklistObjects.Count; i++)
            {
                ChecklistItem temp = new ChecklistItem(checklistObjects[i].objName, checklistObjects[i].toggle, checklistObjects[i].index);
                contents += JsonUtility.ToJson(temp) + '\n';
                //Debug.Log(temp);
                //Debug.Log(contents);
            }
            UnityEngine.Debug.Log(filePath);
            //ChecklistObject temp = new ChecklistObject();
            System.IO.File.WriteAllText(filePath, contents);
        }

        /// <summary>
        /// Loads the data from the saved Json file
        /// </summary>
        void LoadJSONData()
        {
            if (File.Exists(filePath))
            {
                string contents = File.ReadAllText(filePath);
                string[] splitContents = contents.Split('\n');

                foreach (string content in splitContents)
                {
                    if (content.Trim() != "")
                    {
                        //Debug.Log(content);
                        ChecklistItem temp = JsonUtility.FromJson<ChecklistItem>(content);
                        CreateCheckListItems(temp.objName, temp.toggle, temp.index, true, false);
                    }

                }
                CreateCheckListItems("", false, 0, true, true);
                
            }
            else
            {
                UnityEngine.Debug.Log("No File!!");
            }

        }
    }
}
