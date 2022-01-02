using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Swarm_target : MonoBehaviour
{
    public GameObject prey;
    public Vector3 offset;
    public Vector3 IntegrationError;
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
        /*Cette partie de donnée un consigne (un vecteur) sur la direction que le drône doit prendre afin de se diriger vers la cible 

         On verifie aussi si celle-ci à un vitesse supérieur à 90 km/h    (le *3.6f permet de juste de gonfler le chiffre de la vitesse*/
        /*Debug.Log(prey.GetComponent<Rigidbody>().velocity.magnitude);*/
    
            if (prey.GetComponent<Rigidbody>().velocity.magnitude * 3.6f >= 90)
            {
            foreach (Transform child in transform)
            {
                
                Vector3 lookAtPrey = prey.transform.position - child.transform.position + offset ;

                /*Cette partie de créer une intégration sur l'erreur et faire un semblant de PID*//*
                Vector3 IntegrationError += 0.001f*lookAtPrey;
                lookAtPrey += IntegrationError;*/

                child.GetComponent<Drone_handle>().preyCons = 4f * lookAtPrey;


            }

        }
    else
        {
            foreach (Transform child in transform)
            {


                child.GetComponent<Drone_handle>().preyCons = Vector3.zero;


            }
        }


    }

    void OnChangeEyeNumber()
    {
/*        for (int i = 0; i < transform.childCount; i++)
        {

            Vector3 dronePosition = transform.GetChild(i).transform.position;
        Vector3 targetPosition = prey.transform.position;

        IEnumerator m_coroute = transform.GetChild(i).GetComponent<Drone_handle>().go_to(targetPosition);
        StartCoroutine(m_coroute);
        }*/
    }

}
