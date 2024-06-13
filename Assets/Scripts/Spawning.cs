using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawning : MonoBehaviour
{
    //public GameObject prefab;
    private GameObject instance;
    private string[] filePaths;
    private int fileCount;

    // Start is called before the first frame update
    void Start()
    {
        filePaths = Directory.GetFiles("./Assets/Resources", "*.Prefab");
        fileCount = 0;

        foreach(var file in filePaths)
        {
            UnityEngine.Debug.Log(file);
        }
    }

    // Update is called once per frame
    void Update()
    {    
        if(instance == null || instance.transform.parent != null)
        {
            UnityEngine.Debug.Log("New Instance");

            if(fileCount < filePaths.Length)
            {
                InstantiateProp();
                fileCount++;
            }            
        }
    }

    private void InstantiateProp()
    {
        Vector3 startPos = new Vector3(Random.Range(-9,9),Random.Range(-11,1),-2.5f);

        GameObject loadedPrefabResource = (GameObject)LoadPrefabFromFile(Path.GetFileNameWithoutExtension(filePaths[fileCount]));
        instance = Instantiate(loadedPrefabResource, startPos, Quaternion.identity);
        //instance = Instantiate(prefab, startPos, Quaternion.identity);
    }

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        //filename = "MarioCap";
        UnityEngine.Debug.Log(filename);

        UnityEngine.Debug.Log("Trying to load Prefab from file ("+filename+ ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }
}
