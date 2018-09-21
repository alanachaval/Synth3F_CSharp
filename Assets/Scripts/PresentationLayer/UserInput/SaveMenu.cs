using Constants;
using DomainLayer.Common;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace PresentationLayer.UserInput
{
    public class SaveMenu : MonoBehaviour
    {
        [SerializeField] private GameObject saveItemsList;
        [SerializeField] private InputField fileName;
        [SerializeField] private Button buttonPrefab;
        private bool save;

        public void Accept()
        {
            Clear();
            gameObject.transform.localScale = Vector3.zero;
            if (save)
            {
                MainManager.GetInstance().Save(fileName.text);
            }
            else
            {
                MainManager.GetInstance().Load(fileName.text);
            }
        }

        public void Cancel()
        {
            Clear();
            gameObject.transform.localScale = Vector3.zero;
        }

        public void Init(bool save)
        {
            gameObject.transform.localScale = Vector3.one;
            this.save = save;
            fileName.enabled = save;
            Button button;
            Directory.CreateDirectory(Application.persistentDataPath + Others.SavesFolder);
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath + Others.SavesFolder);
            ((RectTransform)saveItemsList.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Others.SaveFilesButtonHeigth * (filePaths.Length + 1));
            ((RectTransform)saveItemsList.transform).localPosition = Vector3.zero;
            for (int i = 0; i < filePaths.Length; i++)
            {
                button = Instantiate(buttonPrefab, saveItemsList.transform);
                string name = Path.GetFileName(filePaths[i]);
                button.onClick.AddListener(delegate () { SelectItem(name); });
                button.GetComponentInChildren<Text>().text = name;
                ((RectTransform)button.transform).SetPositionAndRotation(new Vector3(0, -((i + 1) * Others.SaveFilesButtonHeigth), 0) + saveItemsList.transform.position, Quaternion.identity);
            }
        }

        public void SelectItem(string name)
        {
            fileName.text = name;
        }

        private void Clear()
        {
            Button[] buttons = saveItemsList.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                Destroy(button.gameObject);
            }
        }
    }
}