﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioClip ProjectileClip;

    // valeurs de Adam (bonnes valeurs) : vitesse = 50, temps apparition = 3, temps pour mourrir = 20
    protected Vector3 direction;
    protected float deltaTemps;
    protected float Diamètre, Vitesse, TempsApparition, TempsMourrir;


    string String = "Projectile";

    public void Initialisation(float diamètre, float vitesse, float tempsApparition, float tempsMourrir)
    {
        Debug.Log("initialisation");
        deltaTemps = 0;
        Diamètre = diamètre;
        Vitesse = vitesse;
        TempsApparition = tempsApparition;
        TempsMourrir = tempsMourrir;

        if (TempsApparition == 0)
        {
            Debug.Log("apparue");
            transform.SetGlobalScale(new Vector3(Diamètre, Diamètre, Diamètre)); // apparition
            direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        }

        //direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        Destroy(gameObject, TempsMourrir);
        //transform.localScale = new Vector3(Random.value * 4, Random.value * 4, Random.value * 4);
    }

    void Update()
    {
        if (!DataÉtage.pause)
        {
            if (deltaTemps >= TempsApparition)
            //if (pourcentageTemps >= 1)
            {
                //transform.Translate((DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized * vitesse * Time.deltaTime);
                transform.Translate(direction * Vitesse * Time.deltaTime);
                //Son = true;
            }
            else
            {
                float pourcentageTemps = deltaTemps / TempsApparition;
                Debug.Log("en apparition");
                transform.SetGlobalScale(new Vector3(Diamètre * pourcentageTemps, Diamètre * pourcentageTemps, Diamètre * pourcentageTemps)); // apparition
                direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
            }
            deltaTemps += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected proj");
        GameObject Explosion = Instantiate(Resources.Load<GameObject>("Effects/ProjectileExplosion"), transform.position, Quaternion.identity);
        Destroy(Explosion, 5);
        if (collision.gameObject.name.Contains("Personnage"))
        {
            DataÉtage.PersonnageScript.Dommage(1, collision);
            Debug.Log("collision pers proj");
        }
        Destroy(gameObject);
    }

    //public void AudioProjectile()
    //{
    //    AudioSource.clip = ProjectileClip;
    //    AudioSource.Play();
    //}
}
