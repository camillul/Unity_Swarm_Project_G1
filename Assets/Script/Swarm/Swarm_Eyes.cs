using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_Eyes : MonoBehaviour
{
    public Transform Swarm_target;
    public GameObject RoadSpline;
    // Start is called before the first frame update
    void Start()
    {
        OnChangeEyeNumber();
        foreach (Transform child in transform)
        {
            child.GetChild(7).transform.GetChild(0).gameObject.SetActive(true);
            child.GetChild(7).transform.GetChild(0).GetComponent<Camera_target_essaim>().target = Swarm_target;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnChangeEyeNumber()
    {
    int nodeCount = RoadSpline.transform.childCount;
    int eyeCount = transform.childCount;

    /* en temps normal il faudrait calculer / determiner l'interpolation entre chaque noeud cependant nous allons simplifier et simplement considérer des droites entre chaque noeuds */
    if (eyeCount < nodeCount)
    {
                                           
    }

    if (eyeCount == nodeCount)
    {
            for (int i = 0; i < transform.childCount; i++)
            {

                Vector3 dronePosition = transform.GetChild(i).transform.position;
                Vector3 nodePosition = RoadSpline.transform.GetChild(i).transform.position;
                Vector3 target = new Vector3(nodePosition.x, dronePosition.y, nodePosition.z);
                IEnumerator m_coroute = transform.GetChild(i).GetComponent<Drone_handle>().go_to(target);
                StartCoroutine(m_coroute);

            }
    if (eyeCount > nodeCount)
    {

    }




    }
     }
}