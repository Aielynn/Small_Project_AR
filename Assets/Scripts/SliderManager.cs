using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Camera ARCam;
    private Vector3 startCamPos;
    private GameObject AvatarObject;
    ManualTranslation translationScript;

    void Start()
    {
        Avatar.onPrepareCalibration += OnRestart;
        startCamPos = ARCam.transform.position;
    }

    void OnDestroy()
    {
        Avatar.onPrepareCalibration -= OnRestart;
    }

    private void OnRestart()
    {
        UnityEngine.Debug.Log("OnRestart");
        AvatarObject = GameObject.FindWithTag("Avatar");
        UnityEngine.Debug.Log(AvatarObject);
        translationScript = AvatarObject.GetComponent<ManualTranslation>();
    }

    public void OnSliderYChanged(Slider y)
    {
        Vector3 newPos = startCamPos + new Vector3(0f, -y.value, 0f);
        ARCam.transform.position = newPos;
    }

    public void OnSliderXChanged(Slider x)
    {
        if(AvatarObject == null || translationScript == null)
        {
            OnRestart();
        }

        translationScript.Position = new Vector3(x.value, translationScript.Position.y, translationScript.Position.z);
        AvatarObject.transform.position = translationScript.Position;
        UnityEngine.Debug.Log(translationScript.Position);
    }

    public void OnSliderZChanged(Slider z)
    {
        if(AvatarObject == null || translationScript == null)
        {
            OnRestart();
        }

        translationScript.Position = new Vector3(translationScript.Position.x, translationScript.Position.y, z.value);
        AvatarObject.transform.position = translationScript.Position;
        UnityEngine.Debug.Log(translationScript.Position);
    }
}
