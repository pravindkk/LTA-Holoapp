using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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

        // Start is called before the first frame update
        void Start()
        {
            filePath = Application.persistentDataPath + "/checklists";
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
                //Debug.Log(allFiles[i]);
                item.transform.SetParent(menu);
                item.transform.localScale = new Vector3(1, 1, 1);
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);
                string showFile = filename.Remove(filename.Length - 4);
                item.GetComponentInChildren<Text>().text = showFile;
                item.GetComponent<Button>().onClick.AddListener(delegate
                {
                    OpenTodo(filename);
                });
            }
        }

        /// <summary>
        /// Opens the speific checklist clicked by utilising the ChecklistManager
        /// </summary>
        void OpenTodo(string filename)
        {
            GameObject todoList = Instantiate(todoListPrefab);
            todoList.transform.SetParent(allTodoListManager);
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
