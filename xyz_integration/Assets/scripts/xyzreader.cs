using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SFB; // Namespace for Standalone File Browser
using UnityEngine;

public class xyzreader : MonoBehaviour
{
    public GameObject atomprefab;
    private Dictionary<string, Color> atomc = new Dictionary<string, Color>
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

    public void OpenFilePicker()
    {
        // Open file picker for .xyz files
        var extensions = new[] { new ExtensionFilter("XYZ Files", "xyz") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open XYZ File", "", extensions, false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log("Selected file: " + paths[0]);
            xyzread(paths[0]);
        }
    }

    void xyzread(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return;
        }

        ClearPreviousAtoms(); // Clear existing atoms before loading new ones

        string[] lines = File.ReadAllLines(path);
        int count = int.Parse(lines[0]);
        for (int i = 2; i < 2 + count; i++)
        {
            string[] parts = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            string element = parts[0];
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            float z = float.Parse(parts[3]);
            plota(element, new Vector3(x, y, z));
        }
    }

    void plota(string element, Vector3 position)
    {
        GameObject atom = Instantiate(atomprefab, position, Quaternion.identity);
        Renderer renderer = atom.GetComponent<Renderer>();
        renderer.material.color = atomc.ContainsKey(element) ? atomc[element] : Color.black;
        instantiatedAtoms.Add(atom);
    }

    void ClearPreviousAtoms()
    {
        foreach (GameObject atom in instantiatedAtoms)
        {
            Destroy(atom);
        }
        instantiatedAtoms.Clear();
    }
}
