using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Boid_handle : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 v1;
    private Vector3 v2;
    public Vector3 v3;
    public Vector3 vtot;

    public float alpha = 1f;
    public float beta = 1f;
    public float gamma = 0.1f;

    private Rigidbody rigidbodyComponent;
    private Vector3 DirectionObstable;
    float smooth;
    public List<GameObject> localSwarm;
    float collider_size;
    public float offsetDist;

    void Start()
    {
        localSwarm.Add(this.gameObject);
        collider_size = this.gameObject.GetComponent<SphereCollider>().radius;
        offsetDist = transform.parent.GetComponent<Drone_handle>().m_ScaleX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        v1 = Cohesion(localSwarm);
        v2 = Separation(localSwarm) * 5f ;
        v3 = Alignment(localSwarm);

        vtot = (alpha * v1 + beta * v2 + gamma * v3);
        transform.parent.GetComponent<Drone_handle>().boidCons = vtot;
        Debug.DrawLine(transform.position, transform.position + vtot, Color.blue);
        Debug.Log(vtot);
        Debug.Log("v1 : " + alpha * v1 + "v2 : " + beta * v2 + "v3 : " + gamma * v3);
        
    }

    Vector3 Cohesion(List<GameObject> flockmates)
    {
        if (flockmates.Count <= 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        float average_x = 0f;
        float average_y = 0f;
        float average_z = 0f;

        for (int i = 0; i < flockmates.Count; i++)
        {
            average_x += flockmates[i].transform.position.x;
            average_y += flockmates[i].transform.position.y;
            average_z += flockmates[i].transform.position.z;
        }

        Vector3 vcons = new Vector3(average_x / flockmates.Count, average_y / flockmates.Count, average_z / flockmates.Count) - transform.position;
        Debug.DrawLine(transform.position, new Vector3(average_x / flockmates.Count, average_y / flockmates.Count, average_z / flockmates.Count), Color.green);
        return vcons;
    }


    Vector3 Separation(List<GameObject> flockmates)
    {
        if (flockmates.Count <= 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        Vector3 vcons = Vector3.zero;

        for (int i = 0; i < flockmates.Count; i++)
        {
            Vector3 distance =  - (flockmates[i].transform.position - transform.position);
            if (!(distance.magnitude - 2*offsetDist <= 0.1))
            {
              /*  Debug.DrawLine(transform.position, flockmates[i].transform.position, Color.white);*/
                vcons += distance / (float)Math.Pow(distance.magnitude - 2 * offsetDist, 3);
            }
            else
            {
                /*  Debug.DrawLine(transform.position, flockmates[i].transform.position, Color.white);*/
                vcons += distance * 10;
            }

        }
        Debug.DrawLine(transform.position, transform.position + vcons, Color.red);
        return vcons;
    }


    Vector3 Alignment(List<GameObject> flockmates)
    {
        if (flockmates.Count <= 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return new Vector3(0f, 0f, 0f);


    }







    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains("Drone"))
        {
            localSwarm.Add(other.gameObject);
        }


    }


    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name.Contains("Drone"))
        {
            localSwarm.Remove(other.gameObject);

        }
    }

}
