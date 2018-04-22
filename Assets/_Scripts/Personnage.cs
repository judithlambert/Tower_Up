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
    int vieInitiale;

    bool wallJump = false;
    int côtéCollision; // -1=gauche 1=droit

    public Vector3 VecteurOrigineÀPosition
    {
        get { return transform.position - new Vector3(DataÉtage.Origine.x, transform.position.y, DataÉtage.Origine.z); }
    }

    float angleOrigineÀPosition;
    public float AngleOrigineÀPosition
    {
        get { angleOrigineÀPosition = Mathf.Rad2Deg * Mathf.Atan2(-transform.position.z, transform.position.x);
            return angleOrigineÀPosition += angleOrigineÀPosition < 0 ? 360 : 0; }
    }

    void Awake() // devrait pt pas etre instancier dans unity mais ici comme les autres objets
    {
        //gameObject.AddComponent<Rigidbody>().useGravity = true;
        //gameObject.AddComponent<MeshRenderer>().material = material;
        //gameObject.AddComponent<SphereCollider>();

        // ne dois pas bouger lors du respawn
        transform.rotation = rotationInitiale = Quaternion.Euler(Vector3.zero);
        PositionCheckPoint = transform.position;
        positionInitiale = transform.position;
        GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //transform.position=positionInitial = new Vector3(DataÉtage.RayonTrajectoirePersonnage, transform.lossyScale.y+1, 0);
        //new WaitUntil(()=>TouchingGround());
        jump = crouch = reculer = avancer = block = false;
        Vie = vieInitiale = DataÉtage.difficulté == (int)DataÉtage.Difficulté.Difficile ? 1 : 3;
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
        else              { float vitesse = Vitesse + Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, 2) * (Vitesse < 0 ? 1 : -1);
                            Vitesse = vitesse * Vitesse <= 0 ? 0 : vitesse; }
    }

    void EffectuerDéplacementEtRotation()
    {
        //Vector3 vecPosIn, VecPosFin;
        //vecPosIn = new Vector3(VecteurOrigineÀPosition.x, VecteurOrigineÀPosition.y, VecteurOrigineÀPosition.z); // pour pas que transmet réference ???
        //transform.RotateAround(Vector3.zero, Vector3.down, Vitesse / DataÉtage.RayonTrajectoirePersonnage);
        //VecPosFin = new Vector3(VecteurOrigineÀPosition.x, VecteurOrigineÀPosition.y, VecteurOrigineÀPosition.z); // pour pas que transmet réference ???
        //transform.Rotate(VecteurOrigineÀPosition.normalized, Mathf.Atan2((VecPosFin - vecPosIn).z, (VecPosFin - vecPosIn).x));

        transform.RotateAround(Vector3.zero, Vector3.down, Vitesse / DataÉtage.RayonTrajectoirePersonnage);
        //transform.right = VecteurOrigineÀPosition.normalized;
        ////transform.Rotate(VecteurOrigineÀPosition.normalized, Vitesse / RayonSphere);
        //transform.Rotate(Mathf.Atan2(VecteurOrigineÀPosition.z, VecteurOrigineÀPosition.x),0, 0);
        //Debug.Log("VecteurOrigineÀPosition: (" + VecteurOrigineÀPosition.x + ", " + VecteurOrigineÀPosition.y + ", " + VecteurOrigineÀPosition.z + ")");

    }

    GameObject dernierCollisionObject;
    GameObject nouveauCollisionObject;

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
            if(nbJumps == 1)
            {
                Instantiate(Resources.Load<GameObject>("Effects/ParticuleSaut"), transform.position - new Vector3(0, transform.localScale.y, 0), Quaternion.Euler(-90,0,0));
            }
            ++nbJumps;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pla") || collision.gameObject.GetComponent<Plateforme>().CollisionDessusEtCôté(collision))
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

    //void Repositionnement() // replacer la balle sur sa trajectoire
    //{
    //    transform.Translate(-(VecteurOrigineÀPosition.magnitude - DataÉtage.RayonTrajectoirePersonnage), 0, 0);
    //    transform.right = VecteurOrigineÀPosition.normalized;
    //    transform.Rotate(Mathf.Atan(transform.right.z / transform.right.x) * 360 / (2 * Mathf.PI) * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x / 2), 0, 0);

    //            transform.Rotate(Mathf.Atan2(VecteurOrigineÀPosition.z, VecteurOrigineÀPosition.x),0, 0);

    //}
    void Repositionnement() // replacer la balle sur sa trajectoire
    {
        //transform.right = VecteurOrigineÀPosition.normalized;
        //transform.Translate(-(VecteurOrigineÀPosition.magnitude - DataÉtage.RayonTrajectoirePersonnage), 0, 0);
        if (VecteurOrigineÀPosition.magnitude != DataÉtage.RayonTrajectoirePersonnage)
        {
            transform.position = new Vector3(VecteurOrigineÀPosition.normalized.x * DataÉtage.RayonTrajectoirePersonnage,
                                             transform.position.y,
                                             VecteurOrigineÀPosition.normalized.z * DataÉtage.RayonTrajectoirePersonnage);
        }
        transform.right = VecteurOrigineÀPosition.normalized;

        transform.Rotate(new Vector3(Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x / 2), 0, 0));
        //transform.Translate(VecteurOrigineÀPosition.normalized * (-VecteurOrigineÀPosition.magnitude - DataÉtage.RayonTrajectoirePersonnage));
    }

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
            else if (DataÉtage.nbÉtage != 5) { RetourDernierCheckPoint(); }
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
        PositionCheckPoint = transform.position;
        Vie = vieInitiale;
    }

    public void RetourDernierCheckPoint()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vitesse = 0;
        transform.position = PositionCheckPoint;
        transform.rotation = rotationInitiale;
    }
}