using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    public class Checklist : MonoBehaviour
    {
        string filePath;
        List<string> allFiles = new List<string>();

        public GameObject todoListPrefab;
        public Transform menu;
        public GameObject openButtonPrefab;
        public Transform allTodoListManager;
        private int yCoord = 0;

        // Start is called before the first frame update
        void Start()
        {
            filePath = Application.persistentDataPath + "/checklists/checklists";
            string[] files = System.IO.Directory.GetFiles(filePath);
            foreach (string file in files)
            {
                allFiles.Add(System.IO.Path.GetFileName(file));
            }
            CreateMenu();
        }

        /// <summary>
        /// Creates a menu filled with the preconfigured checklists
        /// </summary>
        public void CreateMenu()
        {
            for (int i=0; i<allFiles.Count; i++)
            {
                string filename = allFiles[i];
                GameObject item = Instantiate(openButtonPrefab);
                Debug.Log(allFiles[i]);
                item.transform.SetParent(menu);
                item.transform.localScale = new Vector3(0.9560533f, 0.9295349f, 1);
                item.transform.localPosition = new Vector3(0.067f, -0.015f - yCoord*0.04f, 0);
                yCoord++;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);
                string showFile = filename.Remove(filename.Length - 4);
                item.GetComponentInChildren<TextMeshPro>().text = showFile;
                item.GetComponentInChildren<Interactable>().OnClick.AddListener(delegate
                {
                    OpenTodo(filename);
                });
                //item.GetComponent<Button>().onClick.AddListener(delegate
                //{
                //    OpenTodo(filename);
                //});
            }
        }

        /// <summary>
        /// Opens the speific checklist clicked by utilising the ChecklistManager
        /// </summary>
        void OpenTodo(string filename)
        {
            GameObject todoList = Instantiate(todoListPrefab);
            todoList.transform.SetParent(allTodoListManager);
            todoList.GetComponentInChildren<ClippingBox>().enabled = true;
            ChecklistManager todoListManager = todoList.GetComponentInChildren<ChecklistManager>();
            todoListManager.SetFileName(filename);
            todoListManager.Creation();
            ChecklistManager temp = todoListManager;
            //todoList.transform.Find("ButtonExit").GetComponentInChildren<Button>().onClick.AddListener(delegate
            //{
            //    CloseTodo(todoList);
            //});

            //todoList.GetComponentInChildren<Button>().onClick.AddListener(delegate
            //{
            //    CloseTodo(todoList);
            //});
        }

    }
}
