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
        bool isWalkingPressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");
        if (!isWalking && isWalkingPressed)
        {
            animator.SetBool("isWalking", true);
        }
        if (isWalking && !isWalkingPressed)
        {
            animator.SetBool("isWalking", false);
        }
    }

}
