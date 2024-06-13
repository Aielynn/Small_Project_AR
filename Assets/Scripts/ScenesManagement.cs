using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
    GameObject Avatar;

    public void LoadInstructions()
    {
        Avatar = GameObject.FindWithTag("Avatar");
        UnityEngine.Debug.Log(Avatar.name);
        Scene scene = Avatar.scene;
        Debug.Log(Avatar.name + " is from the Scene: " + scene.name);
        foreach(var root in Avatar.scene.GetRootGameObjects())
        {
            Destroy(root);
        }

        UnityEngine.Debug.Log("Load Instructions");
        SceneManager.LoadScene(0);
    }

    public void LoadCalibration()
    {
        UnityEngine.Debug.Log("Load Calibration");
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        GameManager.onVictory += LoadVictory;
        GameManager.onDefeat += LoadDefeat;

        Avatar = GameObject.FindWithTag("Avatar");
        GameObject AvatarTracking = GameObject.FindWithTag("AvatarTracking");
        Camera ARcam = Camera.main;
        GameObject Server = GameObject.FindWithTag("Server");

        SetMeshVisibility(Avatar, false);
        //SetMeshVisibility(AvatarTracking, false);

        DontDestroyOnLoad(Avatar);
        DontDestroyOnLoad(AvatarTracking);
        DontDestroyOnLoad(ARcam);
        DontDestroyOnLoad(Server);

        UnityEngine.Debug.Log("Load Game");
        SceneManager.LoadScene(2);
    }
    
    public void LoadVictory()
    {
        UnityEngine.Debug.Log("Load Victory Screen");
        SceneManager.LoadScene(3);
    }

    public void LoadDefeat()
    {
        UnityEngine.Debug.Log("Load Defeat Screen");
        SceneManager.LoadScene(4);
    }


    private void SetMeshVisibility(GameObject avatar, bool setVisible)
    {
        SkinnedMeshRenderer renderer = avatar.GetComponentInChildren<SkinnedMeshRenderer>(false);
        renderer.enabled = setVisible;
    }
}
