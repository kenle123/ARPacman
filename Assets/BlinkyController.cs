using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
public class BlinkyController : MonoBehaviour
{

    private DetectedPlane detectedPlane;
    public GameObject blinklyPrefab;
    private GameObject blinklyInstance;

    public GameObject pointer;
    public Camera firstPersonCamera;
    // Speed to move.
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blinklyInstance == null || blinklyInstance.activeSelf == false)
        {
            pointer.SetActive(false);
            return;
        }
        else
        {
            pointer.SetActive(true);
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            Vector3 pt = hit.Pose.position;
            //Set the Y to the Y of the pacmanInstance
            pt.y = blinklyInstance.transform.position.y;
            // Set the y position relative to the plane and attach the pointer to the plane
            Vector3 pos = pointer.transform.position;
            pos.y = pt.y;
            pointer.transform.position = pos;

            // Now lerp to the position                                         
            pointer.transform.position = Vector3.Lerp(pointer.transform.position, pt,
              Time.smoothDeltaTime * speed);
        }

        // Move towards the pointer, slow down if very close.                                                                                     
        float dist = Vector3.Distance(pointer.transform.position,
            blinklyInstance.transform.position) - 0.05f;
        if (dist < 0)
        {
            dist = 0;
        }

        Rigidbody rb = blinklyInstance.GetComponent<Rigidbody>();
        rb.transform.LookAt(pointer.transform.position);
        rb.velocity = blinklyInstance.transform.localScale.x *
            blinklyInstance.transform.forward * dist / .01f;
    }

    public void SetPlane(DetectedPlane plane)
    {
        detectedPlane = plane;
        // Spawn a new snake.
        SpawnBlinkly();
    }

    void SpawnBlinkly()
    {
        if (blinklyInstance != null)
        {
            DestroyImmediate(blinklyInstance);
        }


        List<Vector3> vertices = new List<Vector3>();
        detectedPlane.GetBoundaryPolygon(vertices);
        Vector3 pos = vertices[Random.Range(0, vertices.Count)];
        float dist = Random.Range(0.05f, 1f);
        Vector3 position = Vector3.Lerp(pos, detectedPlane.CenterPose.position, dist);
        // Move the object above the plane.
        position.y += .05f;


        Anchor anchor = detectedPlane.CreateAnchor(new Pose(position, Quaternion.identity));
        // Not anchored, it is rigidbody that is influenced by the physics engine.
        blinklyInstance = Instantiate(blinklyPrefab, pos,
                Quaternion.identity, transform);
        // After instantiating a new snake instance, add the FoodConsumer component.
        blinklyInstance.AddComponent<PacmanController>();


    }


}