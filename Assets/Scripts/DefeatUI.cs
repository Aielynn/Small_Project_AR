using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatUI : MonoBehaviour
{
    private Camera mainCam;
    public Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = mainCam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
