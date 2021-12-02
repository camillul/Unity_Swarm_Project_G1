using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    private float range = 100f;

    int destroyable; 
    int layer_mask;

    public RaycastHit[] hits;

    void FireLaser()
    {
        Vector3 gun_pos = transform.position + new Vector3(0.2f,0.04f,0.75f);

        Vector3 left = transform.TransformDirection(Vector3.left);
        RaycastHit hit;

        if (Physics.Raycast(gun_pos, left, out hit, range, layer_mask)) 
        {
            Destroy(hit.transform.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        destroyable = 8;
        layer_mask = 1 << destroyable; 
        //layer_mask = ~layer_mask;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 gun_pos = transform.position + new Vector3(0.2f,0.04f,0.75f);
        Vector3 left = transform.TransformDirection(Vector3.left);
        
        
        if(Input.GetMouseButtonDown(0))
        {
            Debug.DrawRay(transform.position, left, Color.blue);
            FireLaser();
        }
        
    }
}
