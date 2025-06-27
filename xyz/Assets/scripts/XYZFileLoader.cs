using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB; // Namespace for Standalone File Browser

public class XYZFileLoaderWithSFB : MonoBehaviour
{
    public GameObject atomPrefab; // Prefab for visualizing atoms
    public Button uploadFileButton; // Button for uploading the file

    private Dictionary<string, Color> atomColors = new Dictionary<string, Color>
    {
        { "H", Color.white },
        { "O", Color.red },
        { "C", Color.grey },
        { "N", Color.blue },
        { "Cl", Color.green },
        { "S", Color.yellow },
        { "P", Color.magenta }
    };

    private List<GameObject> instantiatedAtoms = new List<GameObject>(); // To track instantiated atoms

    void Start()
    {
        // Add listener to the upload file button
        uploadFileButton.onClick.AddListener(OpenFilePicker);
    }

    void OpenFilePicker()
    {
        // Open the file picker with filter for .xyz files
        var extensions = new[] { new ExtensionFilter("XYZ Files", "xyz") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open XYZ File", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log("Selected file: " + paths[0]);
            ReadXYZFile(paths[0]);
        }
    }

    void ReadXYZFile(string path)
    {
        ClearPreviousAtoms(); // Clear any previously instantiated atoms

        // Read all lines from the file
        string[] lines = File.ReadAllLines(path);

        // First line contains the atom count
        int atomCount = int.Parse(lines[0]);

        // Parse each atom from the file
        for (int i = 2; i < 2 + atomCount; i++)
        {
            string[] parts = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string element = parts[0];
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            float z = float.Parse(parts[3]);

            // Instantiate the atom at the given position
            PlotAtom(element, new Vector3(x, y, z));
        }
    }

    void PlotAtom(string element, Vector3 position)
    {
        // Instantiate the atom prefab at the specified position
        GameObject atom = Instantiate(atomPrefab, position, Quaternion.identity);

        // Set the color of the atom based on its type
        Renderer renderer = atom.GetComponent<Renderer>();
        renderer.material.color = atomColors.ContainsKey(element) ? atomColors[element] : Color.black;

        // Add the instantiated atom to the list for tracking
        instantiatedAtoms.Add(atom);
    }

    void ClearPreviousAtoms()
    {
        // Destroy all previously instantiated atoms and clear the list
        foreach (GameObject atom in instantiatedAtoms)
        {
            Destroy(atom);
        }
        instantiatedAtoms.Clear();
    }
}
