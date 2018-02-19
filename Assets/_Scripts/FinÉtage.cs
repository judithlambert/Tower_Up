using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinÉtage : MonoBehaviour {

    // position et rotation

    public const string String = "FinEtage";
    public void Initialisation(float angle, float hauteur)
    {
        transform.position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage, 
                                         hauteur, 
                                         Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage);
        transform.rotation = Maths.Vector3àQuaternion(new Vector3(0, angle, 0));

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.AddComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
        Destroy(cube);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
        GetComponent<MeshCollider>().isTrigger = true;

        Maths.SetGlobalScale(transform, new Vector3(DataÉtage.LARGEUR_PLATEFORME * Mathf.Cos(angle),
                                                    DataÉtage.DELTA_HAUTEUR,
                                                    DataÉtage.LARGEUR_PLATEFORME * Mathf.Sin(angle)));
    }

    private void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("fin étage");
    }

    private void Update()
    {

    }
}
