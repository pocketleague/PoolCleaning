using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRandomIdleAnimation()
    {
        animator.SetInteger("Idle", Random.Range(1, 4));
    }
}
