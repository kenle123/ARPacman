using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSSetText : MonoBehaviour
{

    public Text latLongCoords;
    public Text playerCoords;
    public GameObject player;

    void Update()
    {
        latLongCoords.text = "Latitude: " + GPS.Instance.latitude.ToString() + "\n  Longitude: " + GPS.Instance.longitude.ToString();
        playerCoords.text = "x: " + player.transform.position.x.ToString()
            + "\n  Y: " + player.transform.position.y.ToString();

    }
}
