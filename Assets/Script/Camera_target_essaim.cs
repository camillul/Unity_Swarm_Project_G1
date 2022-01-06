using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_target_essaim : MonoBehaviour
{
    public Transform target;
    public RenderTexture myTexture;
    public RenderTexture myTexture_trash;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

    }


    void FixedUpdate()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(target.position);
        if (viewPos.x < 0 | viewPos.x > 1 | viewPos.y < 0 | viewPos.y > 1 | viewPos.z < 0)
        {
          /*  Debug.Log("Target out of range");*/
            cam.targetTexture = myTexture_trash;
        }
        else
        {
           /* Debug.Log("Ennemy Spotted !");*/
            cam.targetTexture = myTexture;
        }

    }
}