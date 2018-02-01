using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlleur : MonoBehaviour {
    float rayonCaméra;
    void Start () {
        transform.position = new Vector3(transform.position.x, 0, DataÉtage.Personnage.transform.position.z) * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage);
        transform.LookAt(DataÉtage.Personnage.transform);
    }
	
	void Update () {
        transform.position = new Vector3(DataÉtage.Personnage.transform.position.x * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage),
                                         DataÉtage.Personnage.transform.position.y,
                                         DataÉtage.Personnage.transform.position.z * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage)); 
        transform.LookAt(DataÉtage.Personnage.transform);
    }
}
