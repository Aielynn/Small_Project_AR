using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void OnStart(List<string> fileNames);
    public static event OnStart onStart;

    public delegate void OnCollide(int index);
    public static event OnCollide onCollide;

    public delegate void OnVictory();
    public static event OnVictory onVictory;

    public delegate void OnDefeat();
    public static event OnDefeat onDefeat;

    private GameObject bombPrefab;
    private GameObject instance;
    private int instanceInd;
    private List<string> fileNames;
    private string[] filePaths;
    public List<GameObject> collidedInstances;

    //public GameObject outlinesPrefab;
    private GameObject outlines;

    [Range(0, 10)]
    //relative bomb frequency as 1 bomb per x props
    private float bombFreq = 4f;
    private float bombProb;
    private int propFireCount;

    private GameObject textBox;
    private Text text;
    private Camera mainCam;

    int numLives;
    int itemsLeft;

    //public List<int> setProp = new List<int>();

    public void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
        numLives = 3;
        itemsLeft = 2;

        textBox = GameObject.FindWithTag("GameText");
        text = textBox.GetComponent<Text>();

        PrepareProps();
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()
    {
        UnityEngine.Debug.Log("START COUNTDOWN");
        text.text = "READY?";
        yield return new WaitForSeconds (1.0f);
        text.text = "SET...";
        yield return new WaitForSeconds (1.0f);
        text.text = "GO!";
        yield return new WaitForSeconds (1.0f);
        textBox.SetActive(false);
        yield return new WaitForSeconds (1.0f);
        InstantiateProp();
    }

    private void OnEnable()
    {
        //PhaseManager.onStartGame += StartGame;
        CollisionManager.onPropCollided += PropCollision;
        //MoveProp.onPropStopped += PropCollision;
        MoveBomb.bombOnFloor += DestroyProp;
        //CollisionManager.onPropCollided += DestroyProp;
        MoveProp.onPropOutOfRange += UpdateHealth;
        //MoveProp.onPropOutOfRange += DestroyProp;
        AnimationScript.onBombHit += UpdateHealth;
    }

    void OnDestroy()
    {
        //PhaseManager.onStartGame += StartGame;
        CollisionManager.onPropCollided -= PropCollision;
        //MoveProp.onPropStopped += PropCollision;
        MoveBomb.bombOnFloor -= DestroyProp;
        //CollisionManager.onPropCollided += DestroyProp;
        MoveProp.onPropOutOfRange -= UpdateHealth;
        //MoveProp.onPropOutOfRange += DestroyProp;
        AnimationScript.onBombHit -= UpdateHealth;
    }

    private void PrepareProps()
    {
        outlines = GameObject.FindWithTag("AllOutlines");
        foreach(Transform child in outlines.transform)
        {
            child.gameObject.SetActive(true);
        }

        propFireCount = 0;
        filePaths = Directory.GetFiles("./Assets/Resources", "*.Prefab");
        fileNames = filePaths.Select(Path.GetFileNameWithoutExtension).ToList();

        foreach(var file in filePaths)
        {
            UnityEngine.Debug.Log(file);
        }

        itemsLeft = fileNames.Count;

        //Prepare bomb prefab
        //bombPrefab = (GameObject)Resources.Load("Bomb/bomb");

        GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");

        foreach(var prop in props)
        {
            prop.GetComponent<SpriteRenderer>().enabled = true;
        }

        onStart.Invoke(fileNames);
    }

    private void InstantiateProp()
    {
        GameObject loadedPrefabResource;
        Vector3 camPos = mainCam.transform.position;
        Vector3 startPos;

        if(fireBomb())
        {
            //Bomb
            UnityEngine.Debug.Log("BOMB");
            propFireCount = 0;
            startPos = new Vector3(camPos.x, 0.1f, camPos.z);
            bombPrefab = (GameObject)Resources.Load("Bomb/bomb");
            UnityEngine.Debug.Log(bombPrefab);
            instance = Instantiate(bombPrefab, startPos, bombPrefab.transform.rotation);
        }
        else
        {
            //Prop
            startPos = camPos + new Vector3(Random.Range(-1,1),Random.Range(-1,1),0f);
            propFireCount += 1;

            //Quaternion startRot = new Quaternion(-0.56f,-0.52f,0.27f,0.57f);//(-0.67f,-0.57f,0.14f,0.425f);
            Quaternion startRot = Quaternion.identity;
            loadedPrefabResource = getRandomProp();
            instance = Instantiate(loadedPrefabResource, startPos, startRot);
        }
    }

    private bool fireBomb()
    {
        float bombChance;
        if(propFireCount == 0)
        {
            //no chance for bomb throw
            return false;
        }
        else if(propFireCount >= bombFreq)
        {
            //guaranteed bomb throw
            return true;
        }
        else
        {
            bombChance = (float)(propFireCount - 1) / bombFreq;
        }
        
        float randomNum = Random.Range(0f, 1f);
        if (randomNum < bombChance) {
            UnityEngine.Debug.Log("BOMB THROW");
            return true;
        } else {
            UnityEngine.Debug.Log("PROP THROW");
            return false;
        }
    }


    //get prop from list of props
    private GameObject getRandomProp()
    { 
        GameObject loadedPrefab;
           
        int count = fileNames.Count;
        instanceInd = Random.Range(0, count);

        loadedPrefab = (GameObject)LoadPrefabFromFile(fileNames[instanceInd]);

        return loadedPrefab;
    }

    private void UpdateHealth()
    {
        numLives--;
        DestroyProp(instance);
    }

    private void PropCollision(GameObject prop, GameObject outline)
    {
        //Copy position of outline object
        prop.transform.position = outline.transform.position;
        prop.transform.rotation = outline.transform.rotation; 

        //Copy parent constraint source
        //prop.transform.SetParent(outline.transform.parent, true);

        outline.GetComponent<ConstraintCopier>().SetParentConstraint(prop);
        //SetParentConstraint(prop, outline);
        //CopyComponent(outline.GetComponent<ParentConstraint>(), prop);

        //Remove filename from filenames list so it cannot be called
        fileNames.RemoveAt(instanceInd);

        //Update number of items left
        itemsLeft--;

        //Invoke function to remove item in UI
        onCollide.Invoke(instanceInd);

        outline.SetActive(false);

        GetNextState();
        
        //Destroy outline object
        //DestroyProp(outline);
    }
    
    //Called when prop out of range (prop should be destroyed and new prop should be instantiated)
    //Called when prop collided with outline (prop outline should be destroyed and new prop should be instantiated)
    private void DestroyProp(GameObject objToDestroy)
    {
        Destroy(objToDestroy);
        GetNextState();
    }

    //Determine whether we should instantiate new prop 
    //or Game Over (no lives)
    //or Victory (no items left)
    private void GetNextState()
    {
        if(numLives==0)
        {
            GameOver();
        }
        else if(itemsLeft == 0)
        {
            Victory();
        }
        else
        {
            InstantiateProp();
        }
    }

    private void Victory()
    {
        UnityEngine.Debug.Log("Victory");
        
        //Destroy(outlines);
        //if(instance) Destroy(instance);
        onVictory.Invoke();
    }

    private void GameOver()
    {
        //Destroy(outlines);
        if(instance) Destroy(instance);

        UnityEngine.Debug.Log("GAME OVER");
        onDefeat.Invoke();
    }

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        UnityEngine.Debug.Log(filename);

        UnityEngine.Debug.Log("Trying to load Prefab from file ("+filename+ ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    private void checkList()
    {
        UnityEngine.Debug.Log(instanceInd);
        UnityEngine.Debug.Log(fileNames.Count);
        foreach(var i in fileNames)
        {
            UnityEngine.Debug.Log(i);
        }
    }
}
