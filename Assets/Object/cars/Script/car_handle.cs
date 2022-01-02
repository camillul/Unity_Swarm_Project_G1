using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_handle : MonoBehaviour
{
    public bool IsLeader = false;
    public bool IsAutonomous = false;
    private float translationx;
    private float translationz;
    private float thrusty;
    public bool IsOn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
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

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (IsAutonomous == false)
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

    }


}
