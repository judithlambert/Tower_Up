using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip JumpClip;
    public AudioSource JumpSource;

    void Start()
    {
        JumpSource.clip = JumpClip;

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            JumpSource.Play();
        }
    }
    //static public void PlayJumpSound()
    //{
    //    JumpSource.Play();
    //}
}
