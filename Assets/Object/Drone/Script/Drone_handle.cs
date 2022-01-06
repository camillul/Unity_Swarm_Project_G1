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

    private float speedxy = 20f;
    private float speedz = 20f;

    private Rigidbody rigidbodyComponent;

   
    public bool IsAutonomous = true;


    public float motor_Forward_Right = 0.0f;
    public float motor_Forward_Left = 0.0f;
    public float motor_Backward_Right = 0.0f;
    public float motor_Backward_Left = 0.0f;


    private float max_angle = 20f;
    private Vector3 movement;

    public float gravity = 9.81f;
    public float speed = 20f;
    private float lastDeltaTime;

    Quaternion startRotation;
    Quaternion endRotation;


    BoxCollider m_Collider;
    public float m_ScaleX, m_ScaleY, m_ScaleZ;

    public Vector3 boidCons;
    public Vector3 preyCons;
    public Vector3 resultant;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_ScaleX = m_Collider.size.x;
        m_ScaleY = m_Collider.size.y;
        m_ScaleZ = m_Collider.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        /*L'aquisition des commande doit être dans l'update contrairement à la physique car on veut être fluide sur la prise de commande*/
        GetCommand();
    }

    void FixedUpdate()
    {
        resultant = Vector3.zero;

        /*Wheel_Update();*/
        Rotation_Update();

        if(IsAutonomous)
        {
            Debug.Log("Drone MANUAL CONTROL");
            Movement_Update();
        }
        else
        {
            /*en mode autonome : il ajoute toute les vecteur consigne dans la variable résultante et ensuite SwarmBehaviour s'occupe de réaliser la physique*/
            resultant += boidCons;
            resultant += preyCons;
            SwarmBehaviour();
        }

        



    }

    void GetCommand()
    {


        if (Input.GetKeyDown(KeyCode.O))
        {
            if (IsAutonomous == false)
            {
                IsAutonomous = true;
            }
            else
            {
                IsAutonomous = false;
            }
        }

        translationx = Input.GetAxis("drone_Vertical");
        translationz = -Input.GetAxis("drone_Horizontal");
        thrusty = Input.GetAxis("VerticalZ");
   


    }

    void Wheel_Update()
    {

            motor_Forward_Right = 100;
            motor_Forward_Left = 100;
            motor_Backward_Right = 100;
            motor_Backward_Left = 100;

    }







    void Movement_Update()
    {

            rigidbodyComponent.velocity = new Vector3(translationx * speedxy, thrusty * speedz, translationz * speedxy);


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


            rigidbodyComponent.velocity = resultant;


    }



/*Un IEnumerator est comparable à thread et/ou à du "tick" coding, permettant ainsi de réaliser*/

    public IEnumerator go_to(Vector3 target)
    {
/*permet de déplacer le drône d'un point A à un point B */

        float duration = 3f;
        float time = 0;
        Vector3 initial = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(initial, target, time / duration);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = target;
        yield return null;
        
    }



}
