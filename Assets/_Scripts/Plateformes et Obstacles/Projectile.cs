using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // valeurs de Adam (bonnes valeurs) : vitesse = 50, temps apparition = 3, temps pour mourrir = 20
    Vector3 direction;
    float deltaTemps;
    GameObject gameObject;
    Vector3 Position;
    float Vitesse, TempsApparition, TempsMourrir; 

    public void Initialisation(Vector3 position, float vitesse, float tempsApparition, float tempsMourrir)
    {
        Position = position;
        Vitesse = vitesse;
        TempsApparition = tempsApparition;
        TempsMourrir = tempsMourrir;

        gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), Position, Quaternion.identity);

        //direction = (DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized;
        Destroy(gameObject, TempsMourrir);
        //transform.localScale = new Vector3(Random.value * 4, Random.value * 4, Random.value * 4);
    }

    void Update()
    {
        if (deltaTemps >= TempsApparition)
        {
            //transform.Translate((DataÉtage.PersonnageGameObject.transform.position - transform.position).normalized * vitesse * Time.deltaTime);
            gameObject.transform.Translate(direction * Vitesse * Time.deltaTime);
        }
        else
        {
            gameObject.transform.localScale = gameObject.transform.lossyScale * (1 + Time.deltaTime/1.5f); // apparition
            direction = (DataÉtage.PersonnageGameObject.transform.position - gameObject.transform.position).normalized;
        }
        deltaTemps += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected proj");
        Instantiate(Resources.Load<GameObject>("Effects/ProjectileExplosion"), gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.name == "Personnage")
        {
            DataÉtage.PersonnageScript.Dommage(1, collision);
            Debug.Log("collision pers proj");
        }
        Destroy(gameObject);
    }
}
