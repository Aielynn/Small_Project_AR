using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public delegate void OnPropCollided(GameObject prop, GameObject outline);
    public static event OnPropCollided onPropCollided;

    private float distMargin;
    private Vector3 vectorMargin;
    private float angleMargin;
    private bool collisionSuccess;

    // Start is called before the first frame update
    void Start()
    {
        //Angle diff of 20 degrees allowed
        angleMargin = 30f;

        Vector3 size = GetComponent<Collider>().bounds.size;

        //Margin is half of dist from middle to bounds
        vectorMargin = 0.5f * size; 
        distMargin = Vector3.Distance(Vector3.zero, vectorMargin);
        collisionSuccess = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!collisionSuccess && other.gameObject.tag == "Outline" && other.gameObject.name == (this.gameObject.name + "Collider"))
        {
            UnityEngine.Debug.Log("Collision with Corresponding outline");
            GameObject outlineProp = other.transform.parent.gameObject;
            GameObject prop = this.transform.parent.gameObject;

            if(marginError(prop, outlineProp))
            {
                UnityEngine.Debug.Log("CAUGHT PROP");
                collisionSuccess = true;
                onPropCollided.Invoke(prop, outlineProp);
            }            
        }
    }

    private bool marginError(GameObject prop, GameObject outlineProp)
    {
        //should be relative to size of object
        //UnityEngine.Debug.Log("Angle");
        //UnityEngine.Debug.Log(Quaternion.Angle(outlineProp.transform.rotation, prop.transform.rotation));

        bool posDiff = Vector3.Distance(outlineProp.transform.position,prop.transform.position) <= distMargin;
        bool angleDiff = Quaternion.Angle(outlineProp.transform.rotation, prop.transform.rotation) <= angleMargin;
        
        return (posDiff && angleDiff); //true if both posmargin and anglemargin are met
    }
}
