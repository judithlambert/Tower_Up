using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceurProjectiles : MonoBehaviour
{
    public static GameObject Lanceur;

    static public Vector3 positionInitial;
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        //GetComponent<BoxCollider>().isTrigger=true;

        transform.position = positionInitial = new Vector3(0, transform.lossyScale.y / 2, DataÉtage.RayonTrajectoirePersonnage);
        Lanceur = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if ((Time.time%5)==0)
        {
            GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            proj.AddComponent<Projectile>();
        }
    }
}

public class Projectile : MonoBehaviour
{

    bool révolutionFinie=false;

    private void Start()
    {
        transform.position = LanceurProjectiles.positionInitial;
        //GetComponent<SphereCollider>().isTrigger=true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (collision.gameObject == LanceurProjectiles.Lanceur) { révolutionFinie = true; }
        Debug.Log("triggered");
        Destroy(this);
    }

    private void onCollisionEnter(Collision collision)
    {
        //if (collision.gameObject == LanceurProjectiles.Lanceur) { révolutionFinie = true; }
        Debug.Log("triggered");
        Destroy(this);
    }

    private void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 1);

        //if (révolutionFinie)
        //{
        //    Destroy(this);
        //}
    }
}
