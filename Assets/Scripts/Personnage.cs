using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Personnage : MonoBehaviour
{
    int rayonPersonnage = 3; //temporaire
    int rayonCamera = 7;

    //à changer mtfk
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


    void Awake() // pt pas dans 
    {
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
        transform.Rotate(new Vector2(avancer || reculer ? (reculer ? -déplacementVitesse : déplacementVitesse):0, 0)); 
    }

    void Jumper()
    {
        if (SautValide())
        {
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
        if ((transform.position - origine).magnitude != DataÉtage.RayonPersonnage)
        {
            transform.Translate(0, 0, (transform.position - origine).magnitude - DataÉtage.RayonPersonnage);
            transform.LookAt(new Vector3(origine.x, transform.position.y, origine.z));
        }
    }

    bool SautValide()
    {
        if (TouchingGround()) { nbJumps = 0; }
        return (nbJumps < 2);
    }

    bool TouchingGround()
    {
        return transform.position.y == transform.lossyScale.y / 2;
    }

    public void Die()
    {
        Debug.Log("Die");
        Awake();
    }
}