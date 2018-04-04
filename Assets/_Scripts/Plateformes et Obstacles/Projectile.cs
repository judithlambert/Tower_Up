using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float vitesse = 10; // unité par sec
    Vector3 direction;
    float deltaTemps;

    void Start()
    {
        direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        Destroy(gameObject, DataÉtage.RAYON_TOUR * 6 / vitesse);
    }

    void Update()
    {
        transform.Translate(direction * vitesse * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Personnage")
        {
            DataÉtage.PersonnageScript.Dommage(1, collision);
        }
        Destroy(gameObject);
    }
}
