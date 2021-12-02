using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour
{


    public const uint scanLineWidth = 16; // Pixels
    public const uint nbScanLines = 16;
    public float horizontalFOV = 120f; // Degrees
    public float pixelScanTime = .01f; // Seconds per pixel

    public const int maxRayCastDistance = 50; // Meters

    private float lastPixelScanDeltaTime = 0; // Seconds

    private uint currentScanX = 0;
    private uint currentScanY = 0;

    private float scanStepX;
    private float scanStepY;
    private float scanStartX;
    private float scanStartY;

    public Color visualRayStartColor = new Color(1f, 0.5f, 0.15f, 1f);
    public Color visualRayEndColor = new Color(1f, 0.15f, 0.5f, 1f);
    public const float visualRayWidth = .1f;
    public const float visualRayFadeTime = .2f; // Seconds

    private Transform origin;

    private GameObject[,] visualRays;

    private float[,] rayCastBuffer;

    private List<((uint, uint), float)> lastRaysCasted; // ((x, y), remaining time)


    private List<(Vector3, string)> ballsFound;

    // Start is called before the first frame update
    void Start()
    {
        origin = this.transform;

        float verticalFOV = horizontalFOV * (nbScanLines / scanLineWidth);

        scanStepX = Mathf.Tan(Mathf.Deg2Rad * horizontalFOV / 2) * maxRayCastDistance / scanLineWidth;
        scanStepY = Mathf.Tan(Mathf.Deg2Rad * verticalFOV / 2) * maxRayCastDistance / nbScanLines;

        scanStartX = -0.5f * scanLineWidth * scanStepX;
        scanStartY = 0.25f * nbScanLines * scanStepY; //No need to look up //0.5f * nbScanLines * scanStepY;

        visualRays = this.createVisualRays();

        lastRaysCasted = new List<((uint, uint), float)>();

        rayCastBuffer = new float[scanLineWidth, nbScanLines];

        ballsFound = new List<(Vector3, string)>();
    }

    private GameObject[,] createVisualRays()
    {
        var visualRays = new GameObject[scanLineWidth, nbScanLines];

        var points = new Vector3[2];
        points[0] = new Vector3(0, 0, 0);

        for (uint j = 0; j < nbScanLines; j++)
        {
            for (uint i = 0; i < scanLineWidth; i++)
            {
                GameObject obj = new GameObject("visualRay_" + i.ToString() + "_" + j.ToString());
                obj.transform.SetParent(origin, false);

                obj.layer = 2;

                LineRenderer line = obj.AddComponent(typeof(LineRenderer)) as LineRenderer;

                line.useWorldSpace = false;
                points[1] = getRayVector(i, j);
                line.SetPositions(points.Clone() as Vector3[]);

                line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

                line.startWidth = visualRayWidth;
                line.endWidth = visualRayWidth;

                line.enabled = false;

                visualRays[i, j] = obj;
            }
        }

        return visualRays;
    }

    public void FixedUpdate()
    {
        lastPixelScanDeltaTime += Time.fixedDeltaTime;

        while (lastPixelScanDeltaTime >= pixelScanTime)
        {
            lastPixelScanDeltaTime -= pixelScanTime;
            currentScanX++;

            // Update current pixel coordinates
            if (currentScanX >= scanLineWidth)
            {
                currentScanX = 0;
                currentScanY++;
                if (currentScanY >= nbScanLines)
                {
                    currentScanY = 0;
                }
            }

            scanPixelAt(currentScanX, currentScanY);
        }
    }

    private Vector3 getRayVector(uint x, uint y)
    {
        return new Vector3(scanStartX + x * scanStepX, scanStartY - y * scanStepY, maxRayCastDistance);
    }

    private void scanPixelAt(uint x, uint y)
    {
        var lineRenderer = visualRays[x, y].GetComponent<LineRenderer>();

        lastRaysCasted.Add(((x, y), visualRayFadeTime));
        lineRenderer.enabled = true;

        Vector3 rayVector = getRayVector(x, y);
        RaycastHit raycastHit;

        Physics.Raycast(origin.position, origin.TransformVector(rayVector), out raycastHit, maxRayCastDistance);

        if (raycastHit.collider != null)
        {
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(1).normalized * raycastHit.distance); // Clip the visual ray to show the hit
            rayCastBuffer[x, y] = raycastHit.distance;

            if (raycastHit.collider.tag.ToLower().StartsWith("ball"))
            {
                ballsFound.Add((raycastHit.point, raycastHit.collider.tag));
            }
        }
        else
        {
            if (rayCastBuffer[x, y] != -1)
            {
                lineRenderer.SetPosition(1, getRayVector(x, y));
                rayCastBuffer[x, y] = -1;
            }
        }
    }

    private void Update()
    {

        /* Updating ray alpha */
        for (int i = (lastRaysCasted.Count - 1); i >= 0; i--)
        {
            var ((x, y), time) = lastRaysCasted[i];
            var visualRay = visualRays[x, y];
            var lineRenderer = visualRay.GetComponent<LineRenderer>();

            time -= Time.deltaTime; // The time field is a countdown timer

            if (time <= 0)
            {
                lineRenderer.enabled = false;
                lastRaysCasted.RemoveAt(i);
            }
            else
            {
                float t = time / visualRayFadeTime;

                lineRenderer.startColor = Color.Lerp(Color.clear, visualRayStartColor, t);
                lineRenderer.endColor = Color.Lerp(Color.clear, visualRayEndColor, t);

                lastRaysCasted[i] = ((x, y), time);
            }
        }
    }

    public float[,] getRayCastBuffer()
    {
        return rayCastBuffer.Clone() as float[,];
    }

    public (uint, uint) getRayCastBufferSize()
    {
        return (scanLineWidth, nbScanLines);
    }

    public (uint, uint) getCurrentScanPixel()
    {
        return (currentScanX, currentScanY);
    }

    public List<(Vector3, string)> getNewBallsFound()
    {
        var l = new List<(Vector3, string)>(ballsFound);
        ballsFound.Clear();
        return l;
    }
}
