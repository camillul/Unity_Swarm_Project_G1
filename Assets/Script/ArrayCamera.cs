using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCamera : MonoBehaviour
{
    private float speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton (1)) {
            if (Input.GetAxis ("Mouse X") > 0) {
                transform.position += new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed*20, 
                                        0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed*20);
            }
    
            else if (Input.GetAxis ("Mouse X") < 0) {
                transform.position += new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed*20, 
                                        0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed*20);
            }
        }
    
        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
        }
        
    }
}
