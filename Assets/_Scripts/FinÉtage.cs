using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinÉtage : MonoBehaviour // : Plateforme
{
    GameObject drapeau;

    // position et rotation

    public const string String = "FinEtage";
    public void Initialisation(float angle, float hauteur)
    {
        Vector3 position = transform.position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage, 
                                         hauteur + DataÉtage.DELTA_HAUTEUR/2, 
                                         Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage);

        transform.rotation = Maths.Vector3àQuaternion(new Vector3(0, angle, 0));



        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.AddComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
        Destroy(cube);
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        gameObject.AddComponent<MeshRenderer>(); // doit etre enlever quand fini
        gameObject.AddComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;

        Maths.SetGlobalScale(transform, new Vector3(DataÉtage.LARGEUR_PLATEFORME, DataÉtage.DELTA_HAUTEUR, 1));

        drapeau = new GameObject("Drapeau");
        drapeau.AddComponent<DrapeauAnimé>().Initialisation(position);
        drapeau.transform.rotation = Maths.Vector3àQuaternion(new Vector3(0, angle, 0));

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("fin étage");
        DataÉtage.étageFini = true;
    }

    private void Update()
    {

    }
}
