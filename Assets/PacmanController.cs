using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PacmanController : MonoBehaviour
{
    private DetectedPlane detectedPlane;

    public GameObject pacmanPrefab;
    private GameObject pacmanInstance;

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
        if (pacmanInstance == null || pacmanInstance.activeSelf == false)
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
            pt.y = pacmanInstance.transform.position.y;
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
            pacmanInstance.transform.position) - 0.05f;
        if (dist < 0)
        {
            dist = 0;
        }

        Rigidbody rb = pacmanInstance.GetComponent<Rigidbody>();
        rb.transform.LookAt(pointer.transform.position);
        rb.velocity = pacmanInstance.transform.localScale.x *
            pacmanInstance.transform.forward * dist / .01f;
    }

    public void SetPlane(DetectedPlane plane)
    {
        detectedPlane = plane;
        // Spawn Pacman
        SpawnPacman();
    }

    void SpawnPacman()
    {
        if (pacmanInstance != null)
        {
            DestroyImmediate(pacmanInstance);
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        // Not anchored, it is rigidbody that is influenced by the physics engine.
        pacmanInstance = Instantiate(pacmanPrefab, pos,
                Quaternion.identity, transform);

        // Pass the head to the slithering component to make movement work.
        GetComponent<Slithering>().Head = pacmanInstance.transform;

        // After instantiating a new snake instance, add the FoodConsumer component.
        pacmanInstance.AddComponent<PelletConsumer>();
    }

    public int GetLength()
    {
        return GetComponent<Slithering>().GetLength();
    }
}
