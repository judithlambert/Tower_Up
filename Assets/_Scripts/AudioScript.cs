using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip JumpClip;
    static public AudioSource JumpSource;

    void Start()
    {
        //JumpSource.clip = JumpClip;
        //JumpSource = GetComponentInChildren<AudioSource>();
        //JumpSource = GetComponent<AudioSource>();

    }

    static public void PlayJumpSound()
    {
        //JumpSource.Play();
    }
}
