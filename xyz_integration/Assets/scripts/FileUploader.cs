using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB; // StandaloneFileBrowser namespace

public class FileUploader : MonoBehaviour
{
    public Button uploadButton;

    void Start()
    {
        uploadButton.onClick.AddListener(OpenFilePicker);
    }

    public void OpenFilePicker()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an XYZ File", "", "xyz", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            string filePath = paths[0];
            Debug.Log("File Selected: " + filePath);
            string fileContent = File.ReadAllText(filePath);
            ProcessXYZFile(fileContent);
        }
    }

    void ProcessXYZFile(string fileContent)
    {
        Debug.Log("File Content:\n" + fileContent);
    }
}
