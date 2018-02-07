using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Personnage : MonoBehaviour
{
    int ANGULAR_DRAG = 5;

    // continue a rouler apres avoir ete en collison avec quelque chose
    // ajouté du drag

    public float RayonSphere { get { return transform.lossyScale.x / 2; } }

    float déplacementVitesse = 6; // devrait etre proportionnele a quelque chose
    float déplacementForce = 75;
  
    Quaternion rotationInitial;
    Vector3 positionInitial;

    bool jump,
         crouch,
         reculer,
         avancer,
         block;

    int nbJumps=0;

    Vector3 VecteurOrigineBalle
    {
        get { return transform.position - new Vector3(DataÉtage.Origine.x, transform.position.y, DataÉtage.Origine.z); }
    }

    void Awake() // pt pas dans 
    {
        //gameObject.AddComponent<Rigidbody>().useGravity = true;
        //gameObject.AddComponent<MeshRenderer>().material = material;
        //gameObject.AddComponent<SphereCollider>();
        GetComponent<Rigidbody>().angularDrag = 5;

        // ne dois pas bouger lors du respawn
        transform.rotation = rotationInitial = Quaternion.Euler(Vector3.zero);
        positionInitial = transform.position;
        GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
        //transform.position=positionInitial = new Vector3(DataÉtage.RayonTrajectoirePersonnage, transform.lossyScale.y+1, 0);
        //new WaitUntil(()=>TouchingGround());
        jump = crouch = reculer = avancer = block = false;
    }

    
    void InputMouvement()
    {
        jump = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
        crouch = Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        reculer = Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow);
        avancer = Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow);
        block = Input.GetKeyDown("q");
    }

    void EffectuerDéplacementEtRotation()
    {
        float angle = déplacementVitesse / DataÉtage.RayonTrajectoirePersonnage;
        transform.RotateAround(Vector3.zero, Vector3.down, (avancer || reculer ? (reculer ? -angle : angle) : 0));
    }

    // TO BE FIXED
    void Jumper()
    {
        if (nbJumps < 2) // est ce que le saut est valide
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce * 5));
            ++nbJumps;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        nbJumps = 0; //seulement parce que jump bug encore
        //if (collision.gameObject.name.Contains("Pla") && collision.gameObject.GetComponent<Plateforme>().CollisionDessus(collision)) { nbJumps = 0; }
        //else { Debug.Log("collision jump fail"); }
    }
   
    void Update()
    {
        //Debug.Log("jumps: " + nbJumps.ToString());
        InputMouvement();
        if (reculer|| avancer) { EffectuerDéplacementEtRotation(); }
        if (jump) { Jumper(); }

        // replacer la balle sur sa trajectoire
        Vector3 vecteurPolaireOrigine = Maths.VecteurPolaire(VecteurOrigineBalle);
        transform.right = VecteurOrigineBalle;
        transform.Rotate(Mathf.Atan(transform.right.z / transform.right.x) * 360 / (2 * Mathf.PI) * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x/2), 0, 0);
        transform.Translate(-(VecteurOrigineBalle.magnitude - DataÉtage.RayonTrajectoirePersonnage), 0, 0);
    }

    public void Die()
    {
        Debug.Log("Die");
        transform.position = positionInitial;
        transform.rotation = rotationInitial;
    }
}