using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCoin : MonoBehaviour
{
    //public AudioClip Catnip;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //AudioSource.PlayClipAtPoint(Catnip, transform.position);
            Destroy(gameObject);
        }
    }
}
