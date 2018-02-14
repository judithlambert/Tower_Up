using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusil : MonoBehaviour {

    GameObject trigger; // sert a savoir la distance avec la balle
    public const string String = "";
    public float Distance = 3; // en longueur d'arc
    static public Vector3 Position { get; private set; }

    public void Initialisation(float positionX, float hauteur, float positionZ, Material material)
    { // devrait toujours etre en haut (au plafond) dans le fond (accoter sur le mur) pas dans le chemin de la balle
        Position = new Vector3(positionX * DataÉtage.RayonTour + transform.lossyScale.x/2, 
                               DataÉtage.DeltaHauteur - transform.lossyScale.y/2, 
                               positionZ * DataÉtage.RayonTour + transform.lossyScale.z/2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.AddComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
        Destroy(cube);
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        gameObject.AddComponent<BoxCollider>();

        transform.position = Position;

   
        // création du trigger
        trigger = new GameObject("TriggerFusil");
        float angleMilieu = Maths.PositionXYàAngle(positionX, positionZ);
        float amplitude = Maths.ArcDeCercleÀAngle(Distance, DataÉtage.RayonTour);
        trigger.AddComponent<TriggerFusil>().Initialisation(Maths.RadianEnDegré(angleMilieu) - 30, 60, DataÉtage.LargeurPlatforme, DataÉtage.DeltaHauteur, (hauteur+1)*DataÉtage.DeltaHauteur, 0, DataÉtage.RayonTour,0, material);
        trigger.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
        Destroy(trigger.GetComponent<MeshRenderer>());
        Destroy(trigger.GetComponent<Rigidbody>());
        trigger.GetComponent<MeshCollider>().isTrigger = true;
    }
}

public class TriggerFusil: Plateforme
{
    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.name.Contains("Personnage"))
        //{
        //    if ((Time.time % 0.5) == 0 && Time.time > 0)
        //    {
                GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                proj.name = "Ammo";
                proj.AddComponent<Ammo>();
        //    }
        //}
    }
}


public class Ammo : MonoBehaviour
{
    private void Start()
    {
        Maths.SetGlobalScale(transform, new Vector3(0.5f, 0.5f, 0.5f));
        GetComponent<Renderer>().material.color = Color.red;
        transform.position = Fusil.Position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); Debug.Log("collision");
    }

    private void Update()
    {
        transform.forward=DataÉtage.Personnage.transform.position;

    }
}
