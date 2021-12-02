using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AACscript : MonoBehaviour
{
    public float gravity = 9.81f;
    public float speed = 2f;
    private float lastDeltaTime;

    Quaternion startRotation;
    Quaternion endRotation;

    public List<(Vector3, string)> ballPositions;

    public List<AACscript> otherRobots;

    public float minBallDistanceThreshold = 1f; // Distance between two successful raycasts to consider them being separate

    // Start is called before the first frame update
    void Start()
    {
        Vector3 forward_world = transform.TransformDirection(Vector3.forward);
        transform.position += forward_world * speed * Time.deltaTime;

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //TODO: find other bots
    }

    private bool isObjectBallTarget(GameObject obj)
    {
        if (this.transform.tag.StartsWith("Robot"))
        {
            if (obj.transform.tag.StartsWith("Ball"))
            {
                if (obj.transform.tag.EndsWith(this.transform.tag.Substring(6)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            if (isObjectBallTarget(collision.gameObject))
            {
                print(this.transform.tag + " found " + collision.gameObject.transform.tag);
                Destroy(collision.gameObject);

                for (int i = ballPositions.Count - 1; i >= 0; i--)
                {
                    var (pos, tag) = ballPositions[i];
                    if ((collision.gameObject.transform.position - pos).magnitude <= minBallDistanceThreshold)
                    {
                        ballPositions.RemoveAt(i);
                    }
                }
            }
        }
    }

    private void broadcastBallPosition(Vector3 pos, string tag)
    {
        foreach (var r in otherRobots)
        {
            r.receiveBallPosition(pos, tag);
        }
    }

    public void receiveBallPosition(Vector3 pos, string tag)
    {
        if (!this.transform.tag.StartsWith("Robot"))
            return;

        if (!tag.StartsWith("Ball"))
            return;

        if (!tag.EndsWith(this.transform.tag.Substring(6)))
            return;

        storeFoundBall(pos, tag);
    }

    private void storeFoundBall(Vector3 foundPos, string foundTag)
    {
        bool foundSimilar = false;
        foreach (var (pos, tag) in ballPositions)
        {
            if ((foundPos - pos).magnitude <= minBallDistanceThreshold)
            {
                foundSimilar = true;
                break;
            }
        }
        if (!foundSimilar)
        {
            ballPositions.Add((foundPos, foundTag));
        }
    }

    //Go to a position if list_of_position is not empty
    private void Go_to() // Finish go_to function
    {
        if (this.ballPositions.Count > 0)
        {
            var (ball_position, tag) = this.ballPositions[0];
            this.ballPositions.RemoveAt(0);
            // transform.position = Vector3.MoveTowards(this.transform.position, ball_position)
        }
    }

    public void FixedUpdate()
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

    // Update is called once per frame
    void Update()
    {

        Vector3 forward_world = transform.TransformDirection(Vector3.forward);
        transform.position += forward_world * speed * Time.deltaTime;

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}