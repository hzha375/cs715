using static System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [Serializable]
    public struct PrefabMetadata
    {
        public int prefab;
        public float x, y, z;
        public float rw, rx, ry, rz;
    }

    // Main camera
    public Camera cam;
    // The object we're placing
    private GameObject placing;
    // GameObject that we can place on
    public GameObject baseplate;
    // Parent for placed objects
    public GameObject parent;
    // Prefabs the user can select
    private GameObject[] prefabs;
    int currentPrefab;
    // Placed prefabs
    private List<PrefabMetadata> placed = new List<PrefabMetadata>();

    private String filename;

    public Button loadButton;
    public Button saveButton;
    public InputField fnField;

    void Start()
    {
        currentPrefab = 0;
        prefabs = Resources.LoadAll("PrefabPicker", typeof(GameObject))
            .Cast<GameObject>()
            .ToArray();
        Debug.Log(prefabs.Length);

        fnField.onValueChanged.AddListener(ChangeFilename);
        loadButton.onClick.AddListener(Load);
        saveButton.onClick.AddListener(Save);

        // Initialise 'placing'
        ReplaceObject();
    }

    private void ChangeFilename(String filename)
    {
        this.filename = filename;
    }

    private void Save()
    {
        FileStream fs = new FileStream(filename, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, placed);
        fs.Close();
    }

    private void Load()
    {
        // Destroy old objects
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Load in new ones
        FileStream fs = new FileStream(filename, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        placed = (List<PrefabMetadata>) bf.Deserialize(fs);
        fs.Close();

        foreach (PrefabMetadata meta in placed)
        {
            GameObject o = UnityEngine.Object.Instantiate(prefabs[meta.prefab]);
            o.transform.position = new Vector3(meta.x, meta.y, meta.z);
            o.transform.rotation = new Quaternion(meta.rx, meta.ry, meta.rz, meta.rw);
            o.transform.parent = parent.transform;
        }
    }

    private void ReplaceObject(bool destroyOld = false)
    {
        if (destroyOld)
        {
            currentPrefab += 1;
            currentPrefab %= prefabs.Length;
            Destroy(placing);
        } else if (placing != null) {
            if (!placing.activeInHierarchy)
            {
                // Only replace objects that are in view and on the baseplate
                return;
            } else
            {
                placing.transform.parent = parent.transform;

                Vector3 pos = placing.transform.position;
                Quaternion rot = placing.transform.rotation;

                // Add metadata to list
                PrefabMetadata meta = new PrefabMetadata()
                {
                    prefab = currentPrefab,
                    x = pos.x, y = pos.y, z = pos.z,
                    rw = rot.w, rx = rot.x, ry = rot.y, rz = rot.z
                };

                placed.Add(meta);
            }
        }

        // Create a new 'placing' object
        placing = UnityEngine.Object.Instantiate(prefabs[currentPrefab]);

        placing.transform.parent = parent.transform;
        
        // Remove from view until it is on the baseplate
        placing.SetActive(false);
    }

    private Vector3 AlignToGrid(Vector3 i)
    {
        // Align the given vector to the unit grid
        return new Vector3(Mathf.Round(i.x), i.y, Mathf.Round(i.z));
    }

    void Update()
    {
        // Ray from the mouse cursor
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(r);
        foreach (RaycastHit hitInfo in hits) {
            // If the mouse is over the baseplate
            if (hitInfo.collider.gameObject.GetInstanceID() == baseplate.GetInstanceID())
            {
                // Align point to 1-unit grid
                Vector3 gridAligned = AlignToGrid(hitInfo.point);

                // Ensure the cube is in view
                placing.SetActive(true);
                // Move the object to that point
                placing.transform.position = gridAligned;
            }
        }

        // Replace the object when the user clicks
        if (Input.GetMouseButtonDown(0))
        {
            ReplaceObject();
        } else if (Input.GetMouseButtonDown(1))
        {
            placing.transform.Rotate(0, 90, 0);
        } else if (Input.GetMouseButtonDown(2))
        {
            ReplaceObject(true);
        }
    }
}
