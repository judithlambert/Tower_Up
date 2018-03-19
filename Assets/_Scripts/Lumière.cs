using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumière : MonoBehaviour {
	void Update () {
        transform.position = DataÉtage.Caméra.transform.position;
        transform.rotation = DataÉtage.Caméra.transform.rotation;
    }
}
