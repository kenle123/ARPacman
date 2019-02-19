using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PelletController : MonoBehaviour
{
    private DetectedPlane detectedPlane;
    private GameObject pelletInstance;
    private float pelletAge;
    private readonly float maxAge = 10f;

    public GameObject[] foodModels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectedPlane == null)
        {
            return;
        }

        if (detectedPlane.TrackingState != TrackingState.Tracking)
        {
            return;
        }

        // Check for the plane being subsumed
        // If the plane has been subsumed switch attachment to the subsuming plane.
        while (detectedPlane.SubsumedBy != null)
        {
            detectedPlane = detectedPlane.SubsumedBy;
        }

        if (pelletInstance == null || pelletInstance.activeSelf == false)
        {
            SpawnFoodInstance();
            return;
        }

        pelletAge += Time.deltaTime;
        if (pelletAge >= maxAge)
        {
            DestroyObject(pelletInstance);
            pelletInstance = null;
        }
    }

    public void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        detectedPlane = selectedPlane;
    }

    void SpawnFoodInstance()
    {
        GameObject foodItem = foodModels[Random.Range(0, foodModels.Length)];

        // Pick a location.  This is done by selecting a vertex at random and then
        // a random point between it and the center of the plane.
        List<Vector3> vertices = new List<Vector3>();
        detectedPlane.GetBoundaryPolygon(vertices);
        Vector3 pt = vertices[Random.Range(0, vertices.Count)];
        float dist = Random.Range(0.05f, 1f);
        Vector3 position = Vector3.Lerp(pt, detectedPlane.CenterPose.position, dist);
        // Move the object above the plane.
        position.y += .05f;


        Anchor anchor = detectedPlane.CreateAnchor(new Pose(position, Quaternion.identity));

        pelletInstance = Instantiate(foodItem, position, Quaternion.identity,
                 anchor.transform);

        // Set the tag.
        pelletInstance.tag = "food";

        pelletInstance.transform.localScale = new Vector3(.025f, .025f, .025f);
        pelletInstance.transform.SetParent(anchor.transform);
        pelletAge = 0;

        pelletInstance.AddComponent<FoodMotion>();
    }
}
