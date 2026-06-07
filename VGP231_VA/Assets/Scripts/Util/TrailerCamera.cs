using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class TrailerCamera : MonoBehaviour
{
    public Animator animator;
    public List<Animation> animations = new List<Animation>();
    public string animName;
    public bool PlayCamera;

    void Update()
    {
        if (PlayCamera)
        {
            animator.Play(animations[0].name);
            //animator.Play(animName);
            PlayCamera = false;
        }
    }
}
