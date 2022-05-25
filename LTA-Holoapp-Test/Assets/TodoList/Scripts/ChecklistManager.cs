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
            public int index;

            public ChecklistItem(string name, int index)
            {
                this.objName = name;
                this.index = index;
            }

        }

        private void Start()
        {
            
        }

        public void Creation()
        {
            filePath = Application.persistentDataPath + "/checklists/" + filename;
            LoadJSONData();
            //addInputFields = addPanel.GetComponentsInChildren<InputField>();

            createButton.onClick.AddListener(delegate
            {
                CreateCheckListItems(addInputFields[0].text);
            });
        }

        void SwitchMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    addPanel.SetActive(false);
                    break;
                case 1:
                    addPanel.SetActive(true);
                    break;
            }
        }

        public void SetFileName(string filename)
        {
            this.filename = filename;
        }

        public void CreateCheckListItems(string name, int loadIndex = 0, bool loading = false)
        {
            GameObject item = Instantiate(checklistItemPrefab);

            item.transform.SetParent(content);
            ChecklistObject itemObject = item.GetComponent<ChecklistObject>();
            int index = loadIndex;
            //Debug.Log(itemObject.index);
            if (!loading)
                index = checklistObjects.Count;

            
            itemObject.SetObjectInfo(name, index);
            itemObject.GetComponentInChildren<Text>().text = name;
            itemObject.transform.localScale = new Vector3(1, 1, 1);
            itemObject.transform.localPosition = new Vector3(itemObject.transform.localPosition.x, itemObject.transform.localPosition.y, 0);
            checklistObjects.Add(itemObject);
            ChecklistObject temp = itemObject;
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

        void CheckItem(ChecklistObject item)
        {
            checklistObjects.Remove(item);
            SaveJSONData();
            Destroy(item.gameObject);
        }

        void SaveJSONData()
        {
            string contents = "";

            for (int i = 0; i < checklistObjects.Count; i++)
            {
                ChecklistItem temp = new ChecklistItem(checklistObjects[i].objName, checklistObjects[i].index);
                contents += JsonUtility.ToJson(temp + "\n");
            }
            Debug.Log(filePath);
            //ChecklistObject temp = new ChecklistObject();
            File.WriteAllText(filePath, contents);
        }

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
                        CreateCheckListItems(temp.objName, temp.index, true);
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
