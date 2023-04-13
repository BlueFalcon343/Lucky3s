using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceanThrowable : MonoBehaviour
{
    public float speed = 0f;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //Make Bullet Move

        transform.position += transform.forward * speed * Time.deltaTime;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        gameObject.name = "Player";
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

}
