using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    float distCovered;
    float fracJourney;
    float newPosition;
    float oldPosition;
    public bool isInverted;
    public int platform;
    //Vector3 movement;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        //newPosition = 
    }

    void Update()
    {
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, Mathf.PingPong(fracJourney, 1));
        Positioning();
        CheckDirection();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().ChangeType(platform);
        }
        /*if (other.gameObject.CompareTag("Parent"))
        {
            other.transform.parent = transform;
        }*/
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().ChangeType(0);
        }
        /*if (other.gameObject.CompareTag("Parent"))
        {
            other.transform.parent = null;
        }*/
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().ToggleInvert(isInverted);
        }
    }

    void Positioning()
    {
        oldPosition = newPosition;
        newPosition = Mathf.PingPong(fracJourney, 1);
    }

    void CheckDirection()
    {
        if (newPosition > oldPosition)
        {
            isInverted = false;
        }
        else 
        {
            isInverted = true;
        }
    }
/*
    void OnTriggerEnter(Collider other)
    {
        /* if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        /* if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        } 
    } */
}
