using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Wheel_monitor : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;

    public Text speedInfo;
    public float speed;
    public WheelCollider front_left;
    public WheelCollider front_right;
    public WheelCollider rear_left;
    public WheelCollider rear_right;
    public float maxSpeed = 40f;
    public float Torque = 1000f;
    public float brakeTorque = 2000f;
    public float WheelAngleMax = 30f;
    private bool isBraking;


    public Vector3 com = new Vector3(0f, -0.1f, 0f);
    public Rigidbody rb;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;

    }

    private void Update()
    {
        //on affiche la vitesse
        speed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        speedInfo.text = "Speed : " + speed;
        GetInput();
    }

    // Update is called once per frame

    private void FixedUpdate()

    {


        HandleEngine();
        HandleSteering();

    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
        //Input.GetKeyDown(KeyCode.Space);
 /*       Debug.Log(horizontalInput);
        Debug.Log(verticalInput);*/

    }

    private void HandleSteering()
    {
        front_left.steerAngle = horizontalInput * WheelAngleMax;
        front_right.steerAngle = horizontalInput * WheelAngleMax;

    }
    private void HandleEngine()
    {
        rear_left.brakeTorque = 0;
        rear_right.brakeTorque = 0;

        front_left.brakeTorque = 0;
        front_right.brakeTorque = 0;



        Debug.Log("Accélération");
        front_right.motorTorque = verticalInput * Torque * Time.deltaTime;
        front_left.motorTorque = front_right.motorTorque;
/*        Debug.Log(front_left.motorTorque);
        Debug.Log(front_right.motorTorque);*/








        if (speed >= maxSpeed)
        {

            front_left.motorTorque = 0;
            front_right.motorTorque = 0;
        }
        if (isBraking)
        {
            rear_left.brakeTorque = brakeTorque;
            rear_right.brakeTorque = brakeTorque;
            front_left.brakeTorque = brakeTorque;
            front_right.brakeTorque = brakeTorque;
        }
        else
        {
            rear_left.brakeTorque = 0;
            rear_right.brakeTorque = 0;

            front_left.brakeTorque = 0;
            front_right.brakeTorque = 0;
        }



    }




}

