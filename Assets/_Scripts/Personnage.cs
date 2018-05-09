using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

public class Personnage : MonoBehaviour
{
    Vector3 rV3 = new Vector3(0, 0, 0);
    Vector3 arV3 = new Vector3(0, 0, 0);

    [SerializeField] AudioSource AudioSource;
    [SerializeField] AudioClip JumpClip;
    [SerializeField] AudioClip WinClip;
    [SerializeField] AudioClip DeathClip;
    [SerializeField] AudioClip CheckpointClip;
    [SerializeField] AudioClip RecommencerClip;

    const int ACCÉLÉRATION = 5;
    const float ANGULAR_DRAG = float.MaxValue;
    const int MULTIPLICATEUR_VITESSE = 50;


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
    Quaternion ancienneRotation;
    Vector3 positionInitiale;
    GameObject particule;
    public Vector3 PositionCheckPoint { get; set; }

    bool jump,
         crouch,
         reculer,
         avancer,
         block;

    public bool touchingPlateformeMobile = false;
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
        transform.rotation = rotationInitiale = Quaternion.Euler(Vector3.zero);
        PositionCheckPoint = transform.position;
        positionInitiale = transform.position;
        GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        jump = crouch = reculer = avancer = block = false;
        Vie = vieInitiale = DataÉtage.difficulté == (int)DataÉtage.Difficulté.Difficile ? 1 : 3;
    }
    
    void InputMouvement()
    {
        jump = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
        crouch = Input.GetKey("s")   || Input.GetKey(KeyCode.DownArrow)   || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        reculer = Input.GetKey("a")  || Input.GetKey(KeyCode.LeftArrow);
        avancer = Input.GetKey("d")  || Input.GetKey(KeyCode.RightArrow);
        block = Input.GetKeyDown("q");
    }

    public void DéterminerVitesse()
    {
        if      (avancer) { Vitesse = Vitesse + Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, (Vitesse < 0 ? 2 : 1) * (jump ? 2 : 1)); }
        else if (reculer) { Vitesse = Vitesse - Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, (Vitesse > 0 ? 2 : 1) * (jump ? 2 : 1)); }
        else              { float vitesse = Vitesse + Time.deltaTime * Mathf.Pow(ACCÉLÉRATION, 2) * (Vitesse < 0 ? 1 : -1);
                            Vitesse = vitesse * Vitesse <= 0 ? 0 : vitesse; }
    }

    void EffectuerDéplacement()
    {
        transform.RotateAround(Vector3.zero, Vector3.down, Vitesse * MULTIPLICATEUR_VITESSE * Time.deltaTime / DataÉtage.RAYON_TOUR);
    }

    GameObject dernierCollisionObject;
    GameObject nouveauCollisionObject;

    void Jumper()
    {
        if ((wallJump && nbWallJump < 2) || (DataÉtage.difficulté == (int)DataÉtage.Difficulté.Exploration && wallJump)) // BUG
        {
            if (dernierCollisionObject != null && dernierCollisionObject == nouveauCollisionObject) { ++nbWallJump; }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //AudioScript.PlayJumpSound();
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, déplacementForce));
            AudioJump();
            //Vitesse = Mathf.Abs(Vitesse) * 100 * côtéCollision;
            Vitesse = -vitesseWallJump * ACCÉLÉRATION;

            Debug.Log("wall jump successful");
            dernierCollisionObject = nouveauCollisionObject;
        }
        else if (nbJumps < 2 || DataÉtage.difficulté == (int)DataÉtage.Difficulté.Exploration) // est ce que le saut est valide
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(new Vector2(0, déplacementForce));
            AudioJump();
            Debug.Log("jumps succes");
            if (nbJumps == 1)
            {
                particule = Instantiate(Resources.Load<GameObject>("Effects/ParticuleDoubleSaut"), transform.position - new Vector3(0, transform.localScale.y, 0), Quaternion.Euler(-90, 0, 0));
                Destroy(particule, 2);
            }
            ++nbJumps;
        }
        Debug.Log("jump = " + nbJumps + ", wall jump = " + nbWallJump);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Plateforme") && collision.gameObject.GetComponent<Plateforme>().CollisionDessusOuCôté(collision))
        //if (collision.gameObject.name.Contains("Plateforme") && !collision.gameObject.GetComponent<Plateforme>().CollisionDessous(collision))
        {
            Debug.Log("collision jump detected");
            if (collision.gameObject.GetComponent<Plateforme>().CollisionCôté(collision, ref côtéCollision) && nbJumps != 0)
            {
                nouveauCollisionObject = collision.gameObject;
                vitesseWallJump = Vitesse;
                wallJump = true;
                Debug.Log("collision wall jump");
            }
            else // collision dessus
            {
                nouveauCollisionObject = collision.gameObject;
                nbJumps = 0;
                nbWallJump = 0;
                Debug.Log("collision jump");
            }
        }
        else if (collision.gameObject.name.Contains("Plancher"))
        {
            nbJumps = 0;
            nbWallJump = 0;
            wallJump = false;
            Debug.Log("collision plancher");
        }
        else { Debug.Log("collision personnage fail"); }
    }
    void OnCollisionExit(Collision collision)
    {
        wallJump = false; // meuhhhh
    }

    void Update()
    {
        if (!DataÉtage.pause)
        {
            //Debug.Log(nbJumps.ToString());            
            InputMouvement();
            DéterminerVitesse();
            EffectuerDéplacement();
            RepositionnementTrajectoire();
            RepositionnementRotation();
            RotationSurElleMême();

            if (jump) { Jumper(); }
            jump = crouch = reculer = avancer = block = false;
        }
    }

    void RepositionnementTrajectoire() // replacer la balle sur sa trajectoire
    {
        if (VecteurOrigineÀPosition.magnitude != DataÉtage.RayonTrajectoirePersonnage)
        {
            transform.position = new Vector3(VecteurOrigineÀPosition.normalized.x * DataÉtage.RayonTrajectoirePersonnage,
                                             transform.position.y,
                                             VecteurOrigineÀPosition.normalized.z * DataÉtage.RayonTrajectoirePersonnage);
        }
    }

    void RepositionnementRotation()
    {
        arV3 = rV3;
        rV3 = new Vector3(0, Mathf.Atan2(VecteurOrigineÀPosition.x, VecteurOrigineÀPosition.z) * Mathf.Rad2Deg - 90, 0);
        rV3 = rV3 + new Vector3(arV3.x, 0, 0);
        transform.eulerAngles = rV3;
    }

    void RotationSurElleMême()
    {                                   // meme que dans le déplacement
        rV3 = rV3 + new Vector3(Vitesse * MULTIPLICATEUR_VITESSE * Time.deltaTime / DataÉtage.RAYON_TOUR * DataÉtage.RayonTrajectoirePersonnage / (transform.lossyScale.x / 2), 0, 0);
        transform.eulerAngles = rV3;
    }

    public void Die()
    {
        Debug.Log("Die");
        AudioDeath();
        //Réinitialiser();
        DataÉtage.Recommencer();
    }

    public void Dommage(int dommage, Collision collision) // collision sert a fuckall??
    {
        Debug.Log("dommage");
        if(DataÉtage.nbÉtage != DataÉtage.ÉTAGE_BOSS) { StartCoroutine(FlashCouleur(Color.red)); }       
        if (!(DataÉtage.difficulté == (int)DataÉtage.Difficulté.Exploration))
        {
            Vie -= dommage;
            if (Vie <= 0) { Die(); }
            else if (DataÉtage.nbÉtage != 5) { RetourDernierCheckPoint(); }
        }
    }

    IEnumerator FlashCouleur(Color couleur) // ne rentre pas dans la methode
    {
        Color couleurOrigine = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = couleur;
        Debug.Log("outch");
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material.color = couleurOrigine;
    }

    public void Réinitialiser()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = false;
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
        DataÉtage.Checkpoint();
        AudioRecommencer();
    }

    public void AudioWin()
    {
        AudioSource.clip = WinClip;
        AudioSource.Play();
    }

    public void AudioJump()
    {
        AudioSource.clip = JumpClip;
        AudioSource.Play();
    }

    public void AudioDeath()
    {
        AudioSource.clip = DeathClip;
        AudioSource.Play();
    }

    public void AudioCheckpoint()
    {
        AudioSource.clip = CheckpointClip;
        AudioSource.Play();
    }

    public void AudioRecommencer()
    {
        if (AudioSource.clip != DeathClip)
        {
            AudioSource.clip = RecommencerClip;
            AudioSource.Play();
        }
    }
}