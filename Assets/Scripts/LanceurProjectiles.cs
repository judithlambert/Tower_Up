using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceurProjectiles : MonoBehaviour
{

    static public Vector3 Position { get; private set; }

    public void Initialisation(Vector2 position, Material material)
    {
        Position = position;
        gameObject.AddComponent<MeshRenderer>().material = material;
    }


    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>();

        transform.position = Position = new Vector3(0, transform.lossyScale.y / 2, DataÉtage.RayonTrajectoirePersonnage);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if ((Time.time%5)==0 && Time.time>0)
        {
            GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            proj.AddComponent<Projectile>();
        }
    }
}

public class Projectile : MonoBehaviour
{
    bool revolutoinFni = false;

    private void Start()
    {
        Math.SetGlobalScale(transform,new Vector3(0.5f,0.5f,0.5f));
        GetComponent<Renderer>().material.color = Color.red;
        transform.position = LanceurProjectiles.Position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (revolutoinFni)
            Destroy(gameObject);
        else
            revolutoinFni = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 1);

    }
}
