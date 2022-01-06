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

    private float AutoSteer = 0;
    private float AutoAcc = 0.3f;
    public WheelCollider front_left;
    public WheelCollider front_right;
    public WheelCollider rear_left;
    public WheelCollider rear_right;
    public float maxSpeed = 40f;
    public float Torque = 1000f;
    public float brakeTorque = 2000f;
    public float WheelAngleMax = 35f;
    private bool isBraking;
    private bool IsAutonomous = true;
    public Vector3 rayOffset = new Vector3(0f, 2.2f, 0f);

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (IsAutonomous == false)
            {
                IsAutonomous = true;
                maxSpeed = 60;
            }
            else
            {
                IsAutonomous = false;
                maxSpeed = 150;
            }

        }
    }

    // Update is called once per frame

    private void FixedUpdate()

    {
    /*Comme toujours la physique est calculer dans la fixed update 
     Cependant ici on a une implementation plus précise pour l'avancement du véhicule, en effet nous avons ici des roues qui ont proriété proche du réel (frottement, couple, rotation...)
    et nous permet de simuler la commande d'une voiture
     */
    if (!(IsAutonomous))
        {
            HandleEngine();
            HandleSteering();
        }

    else 
        {
            AutoSteer = 0;
            Vector3 left = transform.TransformDirection(2.8f*Vector3.left + Vector3.down);
            Vector3 right = transform.TransformDirection(2.8f * Vector3.right + Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + rayOffset, left, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (!(hit.collider.gameObject.tag == "Road_Left"))
                {

                    Debug.DrawRay(transform.position + rayOffset, left * hit.distance, Color.yellow);
                    AutoSteer =-1.5f;
                    Debug.Log("Je vois pas la gauche");
                }
            }
            if (Physics.Raycast(transform.position + rayOffset, right, out hit, Mathf.Infinity))
            { 
                if (!(hit.collider.gameObject.tag == "Road_Right"))
                {
                    Debug.DrawRay(transform.position + rayOffset, right * hit.distance, Color.yellow);
                    AutoSteer = 1.5f;
                    Debug.Log("Je vois pas la droite");

                   

                }
            }
            AutoSteer = Math.Min(Math.Max(AutoSteer, -1.5f), 1.5f);
            HandleEngineAuto();
            HandleSteeringAuto();
        }

   
        
        
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

    private void HandleSteeringAuto()
    {
        front_left.steerAngle = AutoSteer * WheelAngleMax;
        front_right.steerAngle = AutoSteer * WheelAngleMax;

    }

    private void HandleEngineAuto()
    {
        rear_left.brakeTorque = 0;
        rear_right.brakeTorque = 0;

        front_left.brakeTorque = 0;
        front_right.brakeTorque = 0;



        Debug.Log("Accélération");
      
        front_right.motorTorque = AutoAcc * Torque * Time.deltaTime;
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

