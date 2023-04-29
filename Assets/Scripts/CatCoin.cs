using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCoin : MonoBehaviour
{
    //public AudioClip Catnip;
    public int rotateSpeed;

    public AudioSource coinSound;

    void Start()
    {
        rotateSpeed = 2;
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);

    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //AudioSource.PlayClipAtPoint(Catnip, transform.position);
            coinSound.Play();
            transform.position += new Vector3(0, 1, 0);
            gameObject.GetComponent<Collider>().enabled = false;
            rotateSpeed = 4;
            
            yield return new WaitForSeconds(1f);

            Destroy(gameObject);
        }
    }
    //GetComponent<Collider>().isTrigger = false;
}
