using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileB : Projectile
{
    public void Initialisation(float diamètre, float vitesse, float tempsMourrir)
    {
        Debug.Log("initialisation");
        Diamètre = diamètre;
        Vitesse = vitesse;
        TempsMourrir = tempsMourrir;

        //direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        Destroy(gameObject, TempsMourrir);
        //transform.localScale = new Vector3(Random.value * 4, Random.value * 4, Random.value * 4);
    }

    void Update()
    {
        if (!DataÉtage.pause)
        {
            transform.Translate((DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized * Vitesse * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected proj");
        if (collision.gameObject.name.Contains("Personnage"))
        {
            Destroy(gameObject);
            DataÉtage.PersonnageScript.Dommage(1, collision);
            Debug.Log("collision pers proj");
        }
    }
}
