using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float vitesse = 50; // unité par sec
    Vector3 direction;
    float deltaTemps;

    void Start()
    {
        //direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        Destroy(gameObject, 20);
        //transform.localScale = new Vector3(Random.value * 4, Random.value * 4, Random.value * 4);
    }

    void Update()
    {
        if (deltaTemps >= 3)
        {
            //transform.Translate((DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized * vitesse * Time.deltaTime);
            transform.Translate(direction * vitesse * Time.deltaTime);
        }
        else
        {
            transform.localScale = transform.lossyScale * (1 + Time.deltaTime/1.5f);
            direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        }
        deltaTemps += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject Explosion = Instantiate(Resources.Load<GameObject>("Effects/ProjectileExplosion"), transform.position, Quaternion.identity);
        Destroy(Explosion, 5);
        if (collision.gameObject.name == "Personnage")
        {
            DataÉtage.PersonnageScript.Dommage(1, collision);
        }
        Destroy(gameObject);
    }
}
