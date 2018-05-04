using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musique : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioClip ClipNiveaux;
    [SerializeField] AudioClip ClipBoss;

    public void Niveaux()
    {
        AudioSource.clip = ClipNiveaux;
        AudioSource.Play();
    }

    public void Boss()
    {
        AudioSource.clip = ClipBoss;
        AudioSource.Play();
    }

    public void PausePlay()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Pause();
        }
        else
        {
            AudioSource.UnPause();
        }
    }
}
