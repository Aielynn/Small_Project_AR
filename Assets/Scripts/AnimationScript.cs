using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public delegate void BombHit();
    public static event BombHit onBombHit;

    public void onEndAnimation()
    {
        onBombHit.Invoke();
    }
}
