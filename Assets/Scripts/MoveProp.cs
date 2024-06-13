
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProp : MonoBehaviour
{
    public delegate void OnPropOutOfRange();
    public static event OnPropOutOfRange onPropOutOfRange;

    private GameObject prop;
    private Vector3 direction;
    private Vector3 initPos;
    private float acceleration = 1.5f;
    private float initSpeed = 1f;
    private float friction = 0.5f;
    //private float maxDistance = 20f;
    private bool isMoving;

    private Rigidbody rb;
    private GameObject avatar;
    private Camera mainCam;

    void Start()
    {
        avatar = GameObject.FindWithTag("Avatar");

        mainCam = Camera.main;

        //prop = this.transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        initPos = this.transform.position;

        isMoving = true;
        Fire();
    }

    void OnEnable()
    {
        CollisionManager.onPropCollided += StopMovement;
    }

    void OnDestroy(){
        CollisionManager.onPropCollided -= StopMovement;
    }

    private void FixedUpdate()
    {
        // Apply acceleration towards the player

        if(isMoving)
        {
            //direction = (new Vector3(0,5,0)-this.transform.position).normalized;
            rb.AddForce(direction * acceleration, ForceMode.Acceleration);

            // Apply friction to slow down the object
            rb.velocity *= (1 - friction * Time.fixedDeltaTime);

            if(propOutOfRange())
            {
                onPropOutOfRange.Invoke();  
            }
        }
    }

    // Check if the object has traveled too far from avatar and initpos
    private bool propOutOfRange()
    {
        //compare distance prop-cam to distance avatar-cam
        float distPropCam = Vector3.Distance(this.transform.position, mainCam.transform.position);
        float distAvatarCam = Vector3.Distance(avatar.transform.position, mainCam.transform.position);

        if(distPropCam > (distAvatarCam * 1.3f))
        {
            return true;
        }
        return false;
    }

    private void Fire()
    {
        if(isMoving)
        {
            // Calculate initial direction towards the player
            GameObject targetObj = findTarget();
            Vector3 targetPos = new Vector3(0f, targetObj.transform.position.y, targetObj.transform.position.z) + new Vector3(Random.Range(-1, 1), 0f, 0f);

            direction = (targetPos-initPos).normalized;

            calculateSpeed();

            // Apply initial velocity
            rb.velocity = direction * initSpeed;
        }
    }

    private GameObject findTarget()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Outline");  //Find all GameObjects with specific tag
        string name = this.gameObject.name.Split('(')[0] + "Collider";

        foreach (GameObject go in targetObjects)  //iterate through all returned objects, and find the one with the correct name
        {
            if (go.name == name)
            {
                return go.transform.parent.gameObject;
            }
        }

        return null;
    }

    //Calculate speed, acceleration, friction based on distance
    private void calculateSpeed()
    {
        float distance = Mathf.Abs(avatar.transform.position.z - mainCam.transform.position.z);
        //float distAvatarCam = Vector3.Distance(avatar.transform.position, mainCam.transform.position);
        
        initSpeed = distance / 50; 
        acceleration = initSpeed * 3.5f;
        friction = initSpeed * 0.5f;
    }

    public void StopMovement(GameObject prop, GameObject outline)
    {   
        prop.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isMoving = false;
    }
}
