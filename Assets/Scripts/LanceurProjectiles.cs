using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LanceurProjectiles : MonoBehaviour
{
    public const string String="LanceurProjectiles";

    static public Vector3 Position { get; private set; }

    public void Initialisation(float positionX, float hauteur, float positionZ, Material material)
    {
        Position = new Vector3(positionX * DataÉtage.RayonTrajectoirePersonnage, hauteur + transform.lossyScale.y / 2, positionZ * DataÉtage.RayonTrajectoirePersonnage);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.AddComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
        Destroy(cube);
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        gameObject.AddComponent<BoxCollider>();

        transform.position = Position;
    }

    void FixedUpdate()
    {
        if ((Time.time % 5) == 0 && Time.time > 0)
        {
            GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            proj.name = "Projectile";
            proj.AddComponent<Projectile>();
        }
    }
}


public class Projectile : MonoBehaviour
{
    bool revolutoinFni = false;

    private void Start()
    {
        Maths.SetGlobalScale(transform,new Vector3(0.5f,0.5f,0.5f));
        GetComponent<Renderer>().material.color = Color.red;
        transform.position = LanceurProjectiles.Position;
    }

    // marche pas avec le cube projecteur
    private void OnTriggerEnter(Collider other)
    {
        if (revolutoinFni)
        { Destroy(gameObject); Debug.Log("trigger"); }
        else
            revolutoinFni = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); Debug.Log("collision");
    }

    private void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 1);
    }
}
