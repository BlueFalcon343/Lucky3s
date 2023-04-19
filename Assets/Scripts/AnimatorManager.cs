using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*bool isWalkingPressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetButton("Horizontal") || Input.GetButton("Vertical");
        bool isWalking = animator.GetBool("isWalking");*/
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            animator.SetBool("isWalking", true);
        }
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.SetBool("isWalking", false);
        }
    }

}