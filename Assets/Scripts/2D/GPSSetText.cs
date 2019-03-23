using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSSetText : MonoBehaviour
{

    public Text coordinates;

    void Update()
    {
        coordinates.text = "Latitude: " + GPS.Instance.latitude.ToString() + "\n  Longitude: " + GPS.Instance.longitude.ToString();
    }
}
