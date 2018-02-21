using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Personnage : MonoBehaviour
{
    const int ACCÉLÉRATION = 5;
    const int ANGULAR_DRAG = 5;

    public float RayonSphere { get { return transform.lossyScale.x / 2; } }

    float vitesse = 0;
    float Vitesse
    {
        get { return vitesse; }
        set
        {
            if (value < -DataÉtage.RAYON_TOUR) { vitesse = -DataÉtage.RAYON_TOUR; }
            else if (value > DataÉtage.RAYON_TOUR) { vitesse = DataÉtage.RAYON_TOUR; }
            else { vitesse = value; }
        }
    }

    float déplacementForce = 375;
  
    Quaternion rotationInitial;
    Vector3 positionInitial;

    bool jump,
         crouch,
         reculer,
         avancer,
         block;

    int nbJumps = 0;
    int nbWallJump = 0;

    bool wallJump = false;
    int côtéCollision; // -1=gauche 1=droit

    Vector3 VecteurOrigineBalle
    {
        get { return transform.position - new Vector3(DataÉtage.Origine.x, transform.position.y, DataÉtage.Origine.z); }
    }

    void Awake() // devrait pt pas etre instancier dans unity mais ici comme les autres objets
    {
        //gameObject.AddComponent<Rigidbody>().useGravity = true;
        //gameObject.AddComponent<MeshRenderer>().material = material;
        //gameObject.AddComponent<SphereCollider>();
        GetComponent<Rigidbody>().angularDrag = 5;

        // ne dois pas bouger lors du respawn
        transform.rotation = rotationInitial = Maths.Vector3àQuaternion(Vector3.zero);
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

    public void DéterminerVitesse()
    {
        if      (avancer) { Vitesse = Vitesse + Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, (Vitesse < 0 ? 2 : 1) * (jump ? 2 : 1)); }
        else if (reculer) { Vitesse = Vitesse - Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, (Vitesse > 0 ? 2 : 1) * (jump ? 2 : 1)); }
        else              { Vitesse = Vitesse + Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, 2)                    * (Vitesse < 0 ? 1 : -1); }
    }

    void EffectuerDéplacementEtRotation()
    {
        float angle = Vitesse / DataÉtage.RayonTrajectoirePersonnage;
        transform.RotateAround(Vector3.zero, Vector3.down, angle);
    }

    GameObject dernierCollisionObject;
    GameObject nouveauCollisionObject;

    // TO BE FIXED
    // wall jump toute à changer pour adam
    void Jumper()
    {
        if (nbJumps < 2) // est ce que le saut est valide
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce));
            ++nbJumps;
        }
        if(wallJump && nbWallJump < 2)
        {
            if (dernierCollisionObject == nouveauCollisionObject)
            { ++nbWallJump; }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce));
            Vitesse = Mathf.Abs(Vitesse) * ACCÉLÉRATION * côtéCollision;
            Debug.Log("wall jump successful");
            dernierCollisionObject = nouveauCollisionObject;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pla") && collision.gameObject.GetComponent<Plateforme>().CollisionDessusEtCôté(collision))
        {
            Debug.Log("collision jump");
            if (!collision.gameObject.name.Contains("Plancher") && collision.gameObject.GetComponent<Plateforme>().CollisionCôté(collision, ref côtéCollision))
            {
                nouveauCollisionObject = collision.gameObject;
                wallJump = true;
               
                Debug.Log("collision wall jump");
                dernierCollisionObject = collision.gameObject;

            }
            else { nbJumps = 0; nbWallJump = 0; }
        }
        else { Debug.Log("collision jump fail"); }
    }
    void OnCollisionExit(Collision collision)
    {
        wallJump = false; // meuhhhh
    }

    void Update()
    {
        //Debug.Log("jumps: " + nbJumps.ToString());
        InputMouvement();
        DéterminerVitesse();
        EffectuerDéplacementEtRotation();
        if (jump) { Jumper(); }
        jump = crouch = reculer = avancer = block = false;

        // replacer la balle sur sa trajectoire
        Vector3 vecteurPolaireOrigine = Maths.VecteurCartésienÀPolaire(VecteurOrigineBalle);
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