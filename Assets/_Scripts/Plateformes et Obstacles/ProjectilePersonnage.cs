using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePersonnage : MonoBehaviour
{  
    const float VITESSE = 15;
    Vector3 direction;

    void Start()
    {
        Destroy(gameObject, 10);
        direction = transform.InverseTransformVector(new Vector3(0, GameObject.FindGameObjectWithTag("Boss").transform.lossyScale.y / 2, 0) - transform.position).normalized;
    }

    void Update()
    {
        gameObject.transform.Translate(direction * VITESSE * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected proj");
        GameObject Explosion = Instantiate(Resources.Load<GameObject>("Effects/ProjectilePersonnageExplosion"), transform.position, Quaternion.identity);
        Destroy(Explosion, 5);
        Destroy(gameObject);
    }
}
