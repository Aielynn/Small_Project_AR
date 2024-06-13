using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualTranslation : MonoBehaviour
{
    public Vector3 Position;
    public float Scale;
    public bool FixTransform = false;

    void Start()
    {
        Position = this.transform.position;
        Scale = this.transform.localScale.x;
    }
}
