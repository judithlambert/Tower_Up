//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LanceurProjectiles : MonoBehaviour {

//    Vector3 positionInitial;
//    GameObject proj;
//	// Use this for initialization
//	void Start () {
        
//        transform.position = positionInitial = new Vector3(0, transform.lossyScale.y/2, DataÉtage.RayonPersonnage);
//        proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            proj.transform.position = positionInitial;
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//        if ((Time.time > 2))
//        {
            
//            proj.transform.RotateAround(Vector3.zero, Vector3.down, 3);

//        }
//    }
//}
