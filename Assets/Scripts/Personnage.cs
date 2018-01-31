using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Personnage : MonoBehaviour
{
    int rayonCamera = 7;


    //***
    float déplacementVitesse;
    float déplacementForce;
    [SerializeField] float force;
    [SerializeField] float vitesse;
    [SerializeField] float déplacement;

    Vector3 origine;


    Quaternion rotationInitial;
    Vector3 positionInitial;

    int nbÉtage = 0; int hauteurÉtage = 0; // devra faire le lien

    bool jump,
         crouch,
         reculer,
         avancer;

    int nbJumps;

    Vector3 VecteurOrigineBalle
    {
        get { return transform.position - new Vector3(origine.x, transform.position.y, origine.z); }
    }

    // ajout ailleur
    void Awake() // pt pas dans 
    {
       
        //gameObject.AddComponent<Rigidbody>().useGravity = true;
        //gameObject.AddComponent<MeshRenderer>().material = material;
        //gameObject.AddComponent<SphereCollider>().isTrigger=true;

        origine = DataÉtage.Origine;
        // ne dois pas bouger lors du respawn
        transform.rotation =rotationInitial= Quaternion.Euler(Vector3.zero);
        transform.position=positionInitial = new Vector3(DataÉtage.RayonPersonnage, transform.lossyScale.y+1, 0);
        //new WaitUntil(()=>TouchingGround());
        ArrêterMouvement();
    }


    void Start()
    {
        déplacementVitesse = transform.localScale.x * vitesse;
        déplacementForce = transform.localScale.x * force;
        nbJumps = 0;
    }

    void ArrêterMouvement()
    {
        jump = crouch = reculer = avancer = false;
    }


    bool Déplacement()
    {
        return reculer || avancer;
    }


    void DéterminerMouvement()
    {
        jump = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
        crouch = Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow);
        reculer = Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow);
        avancer = Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow);
    }

    void EffectuerDéplacementEtRotation()
    {
        float angle = déplacementVitesse / DataÉtage.RayonPersonnage;
        transform.RotateAround(Vector3.zero, Vector3.down, (avancer || reculer ? (reculer ? -angle : angle) : 0));
        //ridigbody.AddForce(new Vector3(0,0,avancer || reculer ? (reculer ? -(déplacementForce * 5) : déplacementForce * 5) : 0));
        //transform.Rotate(new Vector2(avancer || reculer ? (reculer ? -déplacementVitesse : déplacementVitesse) : 0, 0));
    }

    void Jumper()
    {
        if (SautValide())
        {
            isTouchingGround = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce * 5));
            ++nbJumps;
        }
    }

    void Update()
    {
        ////---
        //if (transform.position.y <= transform.localScale.y / 2) { nbJumps = 0; }
        //Debug.Log("jump: " + nbJumps.ToString());
        ////---
        DéterminerMouvement();
        if (Déplacement()) { EffectuerDéplacementEtRotation(); }
        if (jump) { Jumper(); }

        Vector3 vecteurPolaireOrigine = VecteurPolaire(VecteurOrigineBalle);
        transform.right = VecteurOrigineBalle;
        transform.Rotate(Mathf.Atan(transform.right.z / transform.right.x) * 360 / (2 * Mathf.PI) * DataÉtage.RayonTour / DataÉtage.RayonPersonnage, 0, 0);
        transform.Translate(-(VecteurOrigineBalle.magnitude - DataÉtage.RayonTour), 0, 0);
    }

    bool SautValide()
    {
        if (isTouchingGround) { nbJumps = 0; }
        return (nbJumps < 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        isTouchingGround = true;
        Debug.Log("touched ground");
    }



    bool isTouchingGround;

    public void Die()
    {
        Debug.Log("Die");
        Awake();
    }

    public Vector3 VecteurPolaire(Vector3 vecteur)
    {
        return new Vector3(Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2)),
                           Mathf.Acos(vecteur.z / Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2))),
                           Mathf.Atan(vecteur.y / vecteur.x));
    }
}