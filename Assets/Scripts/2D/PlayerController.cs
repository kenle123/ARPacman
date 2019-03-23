using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    RectTransform m_RectTransform;
    float m_xAxis, m_yAxis;
    static float start_xAxis, start_yAxis;

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
        m_xAxis = (GPS.Instance.latitude - start_xAxis)*10;
        m_yAxis = (GPS.Instance.longitude - start_yAxis)*10;
        Debug.Log("x: " + m_xAxis);
        Debug.Log("y: " + m_yAxis);
        m_RectTransform.anchoredPosition = new Vector2(m_xAxis, m_yAxis);
    }
}
