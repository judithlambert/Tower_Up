using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

public class Personnage : MonoBehaviour
{
    const int ACCÉLÉRATION = 5;
    const int ANGULAR_DRAG = 5;
    const int VIE_INITIALE = 3;

    public float RayonSphere { get { return transform.lossyScale.x / 2; } }

    int vie;
    public int Vie { get { return vie; } private set { vie = value; updateVie = true; } }
    public bool updateVie;

    float vitesseWallJump;

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
  
    Quaternion rotationInitiale;
    Vector3 positionInitiale;
    public Vector3 PositionCheckPoint { get; set; }

    bool jump,
         crouch,
         reculer,
         avancer,
         block;

    int nbJumps = 0;
    int nbWallJump = 0;

    bool wallJump = false;
    int côtéCollision; // -1=gauche 1=droit

    Vector3 VecteurOrigineÀPosition
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
        transform.rotation = rotationInitiale = Maths.Vector3àQuaternion(Vector3.zero);
        PositionCheckPoint = transform.position;
        positionInitiale = transform.position;
        GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //transform.position=positionInitial = new Vector3(DataÉtage.RayonTrajectoirePersonnage, transform.lossyScale.y+1, 0);
        //new WaitUntil(()=>TouchingGround());
        jump = crouch = reculer = avancer = block = false;
        Vie = VIE_INITIALE;
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
        float angleAutourTour = Vitesse / DataÉtage.RayonTrajectoirePersonnage;
        transform.RotateAround(Vector3.zero, Vector3.down, angleAutourTour);
        float angleAvancementPersonnage = Vitesse / transform.lossyScale.y;
        //transform.RotateAround(transform.position, transform.right, angleAvancementPersonnage);
    }

    GameObject dernierCollisionObject;
    GameObject nouveauCollisionObject;

    //bool JumpValide(int n)
    //{
    //    bool x;
    //    if (DataÉtage.GodMode)
    //        x = truè;
    //    else
    //    {
    //        if()
    //    }
    //}

    // TO BE FIXED
    // wall jump toute à changer pour adam :'(
    void Jumper()
    {
        if ((wallJump && nbWallJump < 2) || (DataÉtage.difficulté == (int)DataÉtage.Difficulté.GodMode && wallJump)) // BUG
        {
            if (dernierCollisionObject != null && dernierCollisionObject == nouveauCollisionObject) { ++nbWallJump; }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, déplacementForce));
            //Vitesse = Mathf.Abs(Vitesse) * 100 * côtéCollision;
            Vitesse = -vitesseWallJump * ACCÉLÉRATION;

            Debug.Log("wall jump successful");
            dernierCollisionObject = nouveauCollisionObject;
        }
        else if (nbJumps < 2 || DataÉtage.difficulté == (int)DataÉtage.Difficulté.GodMode) // est ce que le saut est valide
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce));
            ++nbJumps;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Plateforme>().CollisionDessusEtCôté(collision))
        {
            Debug.Log("collision jump");
            if (collision.gameObject.GetComponent<Plateforme>().CollisionCôté(collision, ref côtéCollision) && nbJumps!=0)
            {
                nouveauCollisionObject = collision.gameObject;
                vitesseWallJump = Vitesse;
                wallJump = true;
                Debug.Log("collision wall jump");
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
        if (!DataÉtage.pause)
        {
            //Debug.Log("jumps: " + nbJumps.ToString());
            InputMouvement();
            DéterminerVitesse();
            EffectuerDéplacementEtRotation();
            Repositionnement();

            if (jump) { Jumper(); }
            jump = crouch = reculer = avancer = block = false;
        }       
    }

    void Repositionnement() // replacer la balle sur sa trajectoire
    {
        transform.Translate(-(VecteurOrigineÀPosition.magnitude - DataÉtage.RayonTrajectoirePersonnage), 0, 0);
        transform.right = VecteurOrigineÀPosition.normalized;
        transform.Rotate(Mathf.Atan(transform.right.z / transform.right.x) * 360 / (2 * Mathf.PI) * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x / 2), 0, 0);
    }
    //void Repositionnement() // replacer la balle sur sa trajectoire
    //{
    //    transform.Translate(-(VecteurOrigineÀPosition.magnitude - DataÉtage.RayonTrajectoirePersonnage), 0, 0);
    //    //if (VecteurOrigineÀPosition.magnitude != DataÉtage.RayonTrajectoirePersonnage) { float angle = Mathf.Atan2(VecteurOrigineÀPosition.z, VecteurOrigineÀPosition.x); transform.position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage, transform.position.y, Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage); }

    //    if (transform.right != VecteurOrigineÀPosition.normalized)
    //    {
    //        float rotationX = transform.rotation.eulerAngles.x;
    //        transform.right = VecteurOrigineÀPosition.normalized;
    //        transform.RotateAround(transform.position, transform.right, rotationX);

    //        //Vector3 VecteurPositionÀRight = -transform.right +VecteurOrigineÀPosition.normalized;
    //        //transform.Rotate(new Vector3(0, -Mathf.Atan2(VecteurPositionÀRight.z, VecteurPositionÀRight.x), 0));
    //        //transform.Rotate(Mathf.Atan(transform.right.z / transform.right.x) * 360 / (2 * Mathf.PI) * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x / 2), 0, 0);
    //    }
    //}

    public void Die()
    {
        Debug.Log("Die");
        Réinitialiser();
        DataÉtage.UiScript.Réinitialiser();
    }

    public void Dommage(int dommage, Collision collision)
    {
        Debug.Log("dommage");
        if (!(DataÉtage.difficulté == (int)DataÉtage.Difficulté.GodMode))
        {
            Vie -= dommage;
            if (Vie <= 0) { Die(); }
            else { DernierCheckPoint(); }
        }
    }

    //IEnumerator FlashCouleur(Color couleur) // ne rentre pas dans la methode
    //{
    //    Color couleurOrigine = GetComponent<Renderer>().material.color;
    //    GetComponent<Renderer>().material.color = couleur;
    //    Debug.Log("outch");
    //    yield return new WaitForSeconds(5);
    //    Debug.Log("5 sec");
    //    GetComponent<Renderer>().material.color = couleurOrigine;
    //}

    public void Réinitialiser()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vitesse = 0;
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;
        Vie = VIE_INITIALE;
    }

    public void DernierCheckPoint()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vitesse = 0;
        transform.position = PositionCheckPoint;
        transform.rotation = rotationInitiale;
    }
}