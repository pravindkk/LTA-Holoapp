using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class ChecklistManager : MonoBehaviour
    {
        public Transform content;
        public GameObject addPanel;
        public Button createButton;
        public GameObject checklistItemPrefab;
        public string filename;

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
            filePath = Application.persistentDataPath + "/checklists/" + filename;
            LoadJSONData();
            //addInputFields = addPanel.GetComponentsInChildren<InputField>();

            createButton.onClick.AddListener(delegate
            {
                SaveAsNewJson();
            });
        }

        //void SwitchMode(int mode)
        //{
        //    switch (mode)
        //    {
        //        case 0:
        //            addPanel.SetActive(false);
        //            break;
        //        case 1:
        //            addPanel.SetActive(true);
        //            break;
        //    }
        //}

        /// <summary>
        /// The filename can be set by the Checklist class
        /// </summary>
        public void SetFileName(string filename)
        {
            this.filename = filename;
        }   

        void SaveAsNewJson()
        {
            string showFile = filename.Remove(filename.Length - 4);

            System.DateTime theTime = System.DateTime.Now;
            string date = (theTime.Minute + theTime.Second + theTime.Day + theTime.Month).ToString();

            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/saved_checklists";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string newFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/saved_checklists/" + showFile + date + ".txt";
            //string newFilePath = Application.persistentDataPath + "/saved_checklists/" + showFile + date + ".txt";

            string contents = "";

            for (int i = 0; i < checklistObjects.Count; i++)
            {
                ChecklistItem temp = new ChecklistItem(checklistObjects[i].objName, checklistObjects[i].toggle, checklistObjects[i].index);
                contents += JsonUtility.ToJson(temp) + '\n';
            }   

            if (File.Exists(newFilePath))
            {

                newFilePath = Application.persistentDataPath + "/saved_checklists/" + showFile + date + Random.Range(0, 10.0f).ToString() +  ".txt";
                
            }
            System.IO.File.WriteAllText(newFilePath, contents);
            Debug.Log("saved as new json");
            //ChecklistObject temp = new ChecklistObject();
            // System.IO.File.WriteAllText(newFilePath, contents);

        }

        /// <summary>
        /// Creates new checklist line items with the ChecklistObject class
        /// </summary>
        public void CreateCheckListItems(string name, bool toggle, int loadIndex = 0, bool loading = false)
        {
            GameObject item = Instantiate(checklistItemPrefab);

            item.transform.SetParent(content);
            ChecklistObject itemObject = item.GetComponent<ChecklistObject>();
            int index = loadIndex;
            //Debug.Log(itemObject.index);
            if (!loading)
                index = checklistObjects.Count;

            
            itemObject.SetObjectInfo(name, toggle, index);
            itemObject.GetComponentInChildren<Text>().text = name;
            itemObject.transform.localScale = new Vector3(1, 1, 1);
            itemObject.transform.localPosition = new Vector3(itemObject.transform.localPosition.x, itemObject.transform.localPosition.y, 0);
            checklistObjects.Add(itemObject);
            ChecklistObject temp = itemObject;
            itemObject.GetComponent<Toggle>().isOn = toggle;
            itemObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                CheckItem(temp);
            });
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
            Debug.Log(filePath);
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
                        CreateCheckListItems(temp.objName, temp.toggle, temp.index, true);
                    }

                }
            }
            else
            {
                Debug.Log("No File!!");
            }

        }
    }
}
