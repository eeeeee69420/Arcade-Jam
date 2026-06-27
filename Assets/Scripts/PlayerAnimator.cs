using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void PlayAnimation(string Animation)
    {
        animator.Play(Animation);
    }
}
