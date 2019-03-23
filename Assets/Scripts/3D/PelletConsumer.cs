using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletConsumer : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pellet")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
