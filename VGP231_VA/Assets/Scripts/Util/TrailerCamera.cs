using System.Diagnostics.Contracts;
using UnityEngine;

public class TrailerCamera : MonoBehaviour
{
    public Animator animator;
    public string animName;
    public bool PlayCamera;

    void Update()
    {
        if (PlayCamera)
        {
            animator.Play(animName);
            PlayCamera = false;
        }
    }
}
