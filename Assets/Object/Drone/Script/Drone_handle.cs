using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Drone_handle : MonoBehaviour
{
/*    public float speed;*/
    private float translationx;
    private float translationz;
    private float thrusty;

    public bool IsOn = true;

    private float speedxy = 8f;
    private float speedz = 16f;

    private Rigidbody rigidbodyComponent;

    public bool IsLeader = false;
    public bool IsAutonomous = false;


    public float motor_Forward_Right = 0.0f;
    public float motor_Forward_Left = 0.0f;
    public float motor_Backward_Right = 0.0f;
    public float motor_Backward_Left = 0.0f;


    private float max_angle = 20f;
    private Vector3 movement;

    public float gravity = 9.81f;
    public float speed = 2f;
    private float lastDeltaTime;

    Quaternion startRotation;
    Quaternion endRotation;

/*    public List<(Vector3, string)> ballPositions;

    public List<RobotAI> otherRobots;*/

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCommand();
    }

    void FixedUpdate()
    {

        Wheel_Update();
        Rotation_Update();

        if (IsLeader)
        {
            Movement_Update();
            ControlBehaviour();
        }
        else
        {
            SwarmBehaviour();
        }





    }

    void GetCommand()
    {


        if (Input.GetKeyDown(KeyCode.O))
        {
            if (IsOn == false)
            {
                IsOn = true;
            }
            else
            {
                IsOn = false;
            }
        }

        translationx = Input.GetAxis("Vertical");
        translationz = -Input.GetAxis("Horizontal");
        thrusty = Input.GetAxis("VerticalZ");
        Debug.Log("thrust = " + thrusty);
        /*        if (Input.GetKeyDown(KeyCode.Z))
                {
                    UpThrust = true;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    UpThrust = false;
                }*/


    }

    void Wheel_Update()
    {
        if (IsOn == true)
        {
            motor_Forward_Right = 100;
            motor_Forward_Left = 100;
            motor_Backward_Right = 100;
            motor_Backward_Left = 100;

        }

        else
        {
            motor_Forward_Right = 0;
            motor_Forward_Left = 0;
            motor_Backward_Right = 0;
            motor_Backward_Left = 0;
        }

    }







    void Movement_Update()
    {


        /*   Vector3 movement = new Vector3(translationx, rigidbodyComponent.velocity.y, translationz);*/
        /* rigidbodyComponent.AddForce(movement);*/


        /*   rigidbodyComponent.velocity.y */

        /*Rigibody way*/
        if (IsOn)

        {
            Debug.Log("true isOn");
            rigidbodyComponent.velocity = new Vector3(translationx * speedxy, thrusty * speedz, translationz * speedxy);
        }
        else
        { rigidbodyComponent.velocity = new Vector3(translationx * speedxy, rigidbodyComponent.velocity.y, translationz * speedxy); }





    }






    void Rotation_Update()
    {
        /*faire pencher le drone quand il avance
       x et z sont inversé car si il se déplace en Z il se penche en x et inversement,
      de plus nous allons calculer
       */
        Quaternion target = Quaternion.Euler(translationz * max_angle, 0.0f, -translationx * max_angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 5f);


    }

/*    void RandomRotation()
    {
        lastDeltaTime += Time.fixedDeltaTime;

        while (lastDeltaTime >= 1.5f)
        {
            lastDeltaTime -= 5f;
            startRotation = transform.rotation;
            endRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + Random.Range(-180, 180), transform.rotation.z);
        }
        transform.rotation = Quaternion.Slerp(startRotation, endRotation, lastDeltaTime);

    }

    void Forward()
    {
        Vector3 forward = transform.TransformDiretion(Vector3.forward);
        transform.position += forward * Time.fixedDeltaTime * speedxy;

    }*/


    void SwarmBehaviour()
    {

    }


    void ControlBehaviour()
    {

    }




}
