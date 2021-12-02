using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autonomous_FPS : MonoBehaviour
{

    private Rigidbody  rigibBodyComponent;
    public float c_speed = 1.0f;
    private Vector3 moveDirection = Vector3.zero;
    public Vector3 destination;
    private int repel_force = 20;
    public float distance = 2.0f;

    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;



    void go_to(Vector3 to){
        Vector3 position = rigibBodyComponent.position;
        Vector3 dir = (to - position).normalized;

            Vector3 forward = transform.TransformDirection(Vector3.forward) * distance;
            Vector3 left = transform.TransformDirection(Vector3.left) * distance;
            Vector3 right = transform.TransformDirection(Vector3.right) * distance;
            Vector3 forward_right = Quaternion.AngleAxis(-45, Vector3.up) * forward;
            Vector3 forward_left = Quaternion.AngleAxis(45, Vector3.up) * forward;
            
            Vector3[] rays = {forward,left,right,forward_right,forward_left};
            
            RaycastHit hit;
            for(int i =0;i<rays.Length;i++){
                Debug.DrawRay(transform.position, rays[i], Color.red);
                if (Physics.Raycast(transform.position, rays[i], out hit,distance)){
                    dir += hit.normal * repel_force; // repel_force is force to repel by
                }
            }
            
            // rotation
            Quaternion  rot = Quaternion.LookRotation (dir);
            transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime);
            //position
            transform.position += transform.forward * (2 * Time.deltaTime); // 20 is speed
    }

    void CameraMove(){
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void OnGUI(){
        GUI.Box(new Rect(Screen.width/2,Screen.height/2, 10, 10), "");
    }

    void Start()
    {
        rigibBodyComponent = gameObject.GetComponent<Rigidbody>() ;
        destination = new Vector3(-8f,1f,-30f); 
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = Camera.main;
    }
    

    void Update()
    {   
        Vector3 position = rigibBodyComponent.position;
        if(Vector3.Distance(position, destination)>0.5){
            go_to(destination);
        }
        CameraMove();
    }

    void FixedUpdate () {
        
    }

    
}
