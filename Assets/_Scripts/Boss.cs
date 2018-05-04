using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour
{
    float lastTimeShout;
    const int VITESSE_ROTATION_BASE = 40; //degré par sec
    const int VITESSE_ROTATION_MIN = 40; //degré par sec
    const int VITESSE_ROTATION_MAX = 90;
    const float VITESSE_AVANCÉ_Z = 1; //unité par sec
    const float AVANCÉ_MIN = -4.8f;
    const float AVANCÉ_MAX = 1.7f;
    const float ANGLE_VIS_À_VIS = 0.5f;
    const float INTERVALLE_APPARITION_PROJECTILE = 5;

    [SerializeField] AudioClip ShoutClip0;
    [SerializeField] AudioClip ShoutClip1;
    [SerializeField] AudioClip ShoutClip2;
    [SerializeField] AudioSource AudioSource;
    List<AudioClip> ShoutClip;

    //[SerializeField] AudioClip Victoire;

    public bool isDead = false;

    public const float NbDeVieInitial = 1000, DommageParCoup = 5;
    float nbDeVie;
    public float NbDeVie { get { return nbDeVie; } private set { nbDeVie = value; if (nbDeVie <= 0) { nbDeVie = 0; Die(); } } }

    Animator animator;
    GameObject Tongue;

    float avancé;
    float nouvelleAvancé;


    public float VitesseRotation { get; private set; }
    public float Rotation
    {
        get { float angle = transform.rotation.eulerAngles.y - 90;
              return angle += (angle < 0 ? 360 : 0); }
    }
    public float AngleBossPersonnage
    {
        get { float angle = DataÉtage.PersonnageScript.AngleOrigineÀPosition - Rotation;
              return angle += angle > 180 ? -360 : (angle < -180 ? 360 : 0); }
    }

    private void Awake()
    {
        ShoutClip = new List<AudioClip>();
        ShoutClip.Add(ShoutClip0);
        ShoutClip.Add(ShoutClip1);
        ShoutClip.Add(ShoutClip2);
    }

    void Start()
    {
        VitesseRotation = VITESSE_ROTATION_MIN;
        animator = GetComponent<Animator>();
        Tongue = GameObject.Find("Tongue");
        NbDeVie = NbDeVieInitial;

    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    //GetHit();
    //    Dommage();
    //}

    void Update()
    {
        if (!DataÉtage.pause && !isDead)
        {
            if (VisÀVisPersonnage())
            {
                Shout();
                //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) // fusionner les if
                //{

                    if (lastTimeShout + 4.667f <= Time.time || lastTimeShout == 0)
                    {
                        StartCoroutine(CracherProjectile());
                        lastTimeShout = Time.time;
                    }
                //}
                NouvelleVitesseAléatoire();
                NouvelleAvancéAléatoire();
            }
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Shout"))
            {
                RotationVersPersonnage();
            }
        }     
    }

    public void Dommage(int dommage, Collision collision)
    {
        if (collision.gameObject.name.Contains("ProjectileP"))
        {
            NbDeVie -= dommage;
            Debug.Log(NbDeVie);
        }
    }

    bool VisÀVisPersonnage()
    {
        bool b = false;
        if (Mathf.Abs(AngleBossPersonnage) <= ANGLE_VIS_À_VIS) { b = true; }
        return b;
    }

    void RotationVersPersonnage()
    {
        Walk();
        transform.RotateAround(Vector3.zero, Vector3.up, (AngleBossPersonnage < 0 ? -1 : 1) * Time.deltaTime * VitesseRotation);
        float déplacementAvancé = VITESSE_AVANCÉ_Z * Time.deltaTime * (nouvelleAvancé - avancé);
        transform.Translate(0, 0, déplacementAvancé);
        avancé += déplacementAvancé;
        
    }

    void NouvelleVitesseAléatoire()
    {
        VitesseRotation = Random.Range(VITESSE_ROTATION_MIN, VITESSE_ROTATION_MAX);
    }

    void NouvelleAvancéAléatoire()
    {
        nouvelleAvancé = Random.Range(AVANCÉ_MIN, AVANCÉ_MAX);
    }
    
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void Walk()
    {
        animator.ResetTrigger("Shout");
        animator.SetTrigger("Walk");
    }
    public void Run()
    {
        animator.SetTrigger("Run");
    }
    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        DataÉtage.TourGameObject.GetComponent<ÉtageBoss>().Victoire();
    }
    public void Shout()
    {
        //lastTimeShout + 4.667f <= Time.time
        animator.SetTrigger("Shout");
    }
    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }


    IEnumerator CracherProjectile()
    {
        yield return new WaitForSeconds(1.3f);
        //Vector3 PositionTongue = transform.TransformPoint(Tongue.transform.localPosition);
        Vector3 PositionTongue = transform.TransformPoint(new Vector3(0, 0.8f, 1.6f));
        //Vector3 PositionTongue = GameObject.Find("Rino").transform.worldToLocalMatrix * (Tongue.transform.localToWorldMatrix * Tongue.transform.position);
        GameObject proj = Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileB"), PositionTongue, Quaternion.identity);
        proj.AddComponent<ProjectileBoss>().Initialisation(0.8f, Random.Range(4,7), 40);
        AudioShout();
    }

    public void AudioShout()
    {
        AudioSource.clip = ShoutClip[(int)Mathf.Floor(Random.Range(0, ShoutClip.Count))];
        AudioSource.Play();
    }
}
