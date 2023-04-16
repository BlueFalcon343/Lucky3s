using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRotator : MonoBehaviour
{
    public bool invert;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!invert)
        {
            transform.Rotate(new Vector3(3, 6, 9) * Time.deltaTime);
        }
        else if (invert)
        {
            transform.Rotate(new Vector3(-3, -6, -9) * Time.deltaTime);
        }
    }
}
