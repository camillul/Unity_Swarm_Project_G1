using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_handle : MonoBehaviour
{
    private float fan_rotation;
    GameObject parentDrone;
    // Start is called before the first frame update
    void Start()
    {
        parentDrone = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {


        if (gameObject.name == "Bras_Forward_Right")
        {
            fan_rotation = parentDrone.GetComponent<Drone_handle>().motor_Forward_Right;
        }

        if (gameObject.name == "Bras_Forward_Left")
        {
            fan_rotation = parentDrone.GetComponent<Drone_handle>().motor_Forward_Left;
        }

        if (gameObject.name == "Bras_Backward_Right")
        {
            fan_rotation = parentDrone.GetComponent<Drone_handle>().motor_Backward_Right;
        }

        if (gameObject.name == "Bras_Backward_Left")
        {
            fan_rotation = parentDrone.GetComponent<Drone_handle>().motor_Backward_Left;
        }


    }

    void FixedUpdate()
    {
        transform.GetChild(0).Rotate(0.0f, fan_rotation, 0.0f);
    }


}
