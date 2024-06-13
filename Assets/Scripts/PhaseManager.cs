using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    //public delegate void StartGame();
    //public static event StartGame onStartGame;
    
    public delegate void StartCalibration();
    public static event StartCalibration onStartCalibration;

    //public GameManager gameManagerPrefab;
    //public UIManager uiManagerPrefab;
    //public Canvas gameCanvasPrefab;
    public Camera ARCam;
    public GameObject Calibrator;

    public GameObject AvatarPrefab;
    public GameObject AvatarTrackingPrefab;
    private GameObject Avatar;
    private GameObject AvatarOnlyTracking;
    //private GameManager gameManager;
    //private UIManager uiManager;
    //private Canvas gameCanvas;

    private Vector3 startCamPos;
    //public bool onlyGame;

    // Start is called before the first frame update

    void OnEnable()
    {
        //PrepareAvatars(new Vector3(0,0,0));
        //Calibrator.SetActive(true);
        startCamPos = ARCam.transform.position;
    }

    private void PrepareAvatars(Vector3 AvatarPos)
    {
        UnityEngine.Debug.Log("Prepare Avatars Function");
        Destroy(Avatar);
        Destroy(AvatarOnlyTracking);
        Avatar = Instantiate(AvatarPrefab, AvatarPos, new Quaternion(0f,1f,0f,0f));

        AvatarOnlyTracking = Instantiate(AvatarTrackingPrefab, new Vector3(0, 0, 0), new Quaternion(0f,1f,0f,0f));
        TransformCopier CopierScript = Avatar.GetComponent<TransformCopier>();
        CopierScript.source = AvatarOnlyTracking.transform;
        CopierScript.destination = Avatar.transform;
        CopierScript.Initialize();

        ManualTranslation translationScript = Avatar.GetComponent<ManualTranslation>();
        translationScript.Position = AvatarPos;

        SetMeshVisibility(Avatar, true);
        SetMeshVisibility(AvatarOnlyTracking, false);
    }

    private void SetMeshVisibility(GameObject avatar, bool setVisible)
    {
        SkinnedMeshRenderer renderer = avatar.GetComponentInChildren<SkinnedMeshRenderer>(false);
        renderer.enabled = setVisible;
    }

    void Start()
    {
        PreparationPhase();
    }

    public void PreparationPhase()
    {
        Calibrator.SetActive(true);

        UnityEngine.Debug.Log("Prepare Avatars");
        if(Avatar != null)
        {
            ManualTranslation translationScript = Avatar.GetComponent<ManualTranslation>();
            Vector3 AvatarPos = translationScript.Position;
            PrepareAvatars(AvatarPos);
        }
        else
        {
            PrepareAvatars(new Vector3(0,0,0));
        }
    }

    public void CalibrationPhase()
    {
        onStartCalibration.Invoke();
    }

    public void RecalibrationPhase()
    {        
        PreparationPhase();
    }

    /*private void DestroyProps()
    {
        foreach(var propclone in GameObject.FindGameObjectsWithTag("PropClone"))
        {
            Destroy(propclone);
        }
    }*/

    /*public void GameEnded()
    {
        UnityEngine.Debug.Log("Game Ended Phasemanager");
        SetMeshVisibility(Avatar, true);

        /*foreach(GameObject prop in gameManager.collidedInstances)
        {
            Destroy(prop);
        }

        Destroy(gameManager.gameObject);
        Destroy(uiManager.gameObject);
    }*/
}
