using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    RectTransform m_RectTransform;
    double m_xAxis, m_yAxis;
    static double start_xAxis, start_yAxis;

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        start_xAxis = GPS.Instance.latitude;
        start_yAxis = GPS.Instance.longitude;
    }

    // Update is called once per frame
    void Update()
    {
        m_xAxis = (GPS.Instance.latitude - start_xAxis) * 10000;
        m_yAxis = (GPS.Instance.longitude - start_yAxis ) * 10000;
        Debug.Log("x: " + m_xAxis);
        Debug.Log("y: " + m_yAxis);
        m_RectTransform.anchoredPosition = new Vector2((float) m_xAxis, (float)m_yAxis);
    }
}
