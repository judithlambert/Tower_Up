using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlleur : MonoBehaviour
{
    void Start ()
    {
        transform.position = new Vector3(transform.position.x, 0, DataÉtage.Personnage.transform.position.z) * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage);
        transform.LookAt(DataÉtage.Personnage.transform);
    }
	
	void Update () {
        transform.position = new Vector3(DataÉtage.Personnage.transform.position.x * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage),
                                         DataÉtage.Personnage.transform.position.y + DataÉtage.RayonCamera / 10,
                                         DataÉtage.Personnage.transform.position.z * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage));
        transform.LookAt(new Vector3(DataÉtage.Origine.x, DataÉtage.Personnage.transform.position.y, DataÉtage.Origine.z));
    }
}
