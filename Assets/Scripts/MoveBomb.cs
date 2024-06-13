using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBomb : MonoBehaviour
{
    public delegate void BombOnFloor(GameObject bomb);
    public static event BombOnFloor bombOnFloor;

    // launch variables
    private Transform targetTF;

    //private bool onGround;
    private bool isMoving;

    private Rigidbody rb;
    private Vector3 initialPos;
    private Quaternion initialRot;
    private float launchAngle;
    private Vector3 targetXZPos;
    Animator animator;
    
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        targetTF = GameObject.FindWithTag("Target").transform;
        
        animator = this.transform.GetChild(0).gameObject.GetComponent<Animator>();

        //onGround = false;
        isMoving = false;
        initialPos = this.transform.position;
        initialRot = this.transform.rotation;
        launchAngle = estimateLaunchAngle();
        Launch();
    }
    
    private float estimateLaunchAngle()
    {
        //angle between line to target, line to target x
        //cos(angle) = a/s
        //angle = cos^-1 (a/s)
        //we want to hit when coming down
        Vector3 initialXZPos;

        initialXZPos = new Vector3(initialPos.x, 0.0f, initialPos.z);

        targetXZPos = new Vector3(targetTF.position.x, 0.0f, targetTF.position.z) + new Vector3(Random.Range(-1, 1), Random.Range(0, 1), 0f);

        float distToTarget = Vector3.Distance(initialPos, targetTF.position); //schuin
        float distXZToTarget = Vector3.Distance(initialXZPos, targetXZPos); //aanliggenc

        float angle = Mathf.Acos(distXZToTarget/distToTarget);
        
        return (angle * 2.2f);
    }

	// Update is called once per frame
	void Update ()
    {
        if(isMoving && this.transform.position.y < 0)
        {
            bombOnFloor.Invoke(this.transform.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //onGround = true;
        UnityEngine.Debug.Log("Collision Bomb and Avatar");
            
        //Stop movement
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
            
        //Activate Animation
        animator.speed = 0.8f;
        animator.SetTrigger("attack01");  
    }

    // launches the object towards the TargetObject with a given LaunchAngle
    void Launch()
    {
        Vector3 bombXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        // Vector3 targetXZPos = new Vector3(targetTF.position.x, 0.0f, targetTF.position.z);
        
        // rotate object to face target
        transform.LookAt(targetXZPos);

        float R = Vector3.Distance(bombXZPos, targetXZPos);
        float G = Physics.gravity.y;
        //UnityEngine.Debug.Log(launchAngle);

        float tanAlpha = Mathf.Tan(launchAngle);
        float H = (targetTF.position.y) - transform.position.y;

        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        rb.velocity = globalVelocity;
        isMoving = true;
    }
}