using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExplosion : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioClip Explosion0;
    [SerializeField] AudioClip Explosion1;
    [SerializeField] AudioClip Explosion2;
    [SerializeField] AudioClip Explosion3;
    [SerializeField] AudioClip Explosion4;
    List<AudioClip> Explosion;

    void Awake ()
    {
		Explosion = new List<AudioClip>();
        Explosion.Add(Explosion0);
        Explosion.Add(Explosion1);
        Explosion.Add(Explosion2);
        Explosion.Add(Explosion3);
        Explosion.Add(Explosion4);
        AudioSource.clip = Explosion[(int)Mathf.Floor(Random.Range(0, Explosion.Count))];
        AudioSource.Play();
    }
}
