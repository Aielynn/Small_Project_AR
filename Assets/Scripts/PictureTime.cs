using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureTime : MonoBehaviour
{
    public delegate void OnTakePic();
    public static event OnTakePic onTakePic;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TakePicture()
    {
        UnityEngine.Debug.Log("pica");
        onTakePic.Invoke();
    }
}
