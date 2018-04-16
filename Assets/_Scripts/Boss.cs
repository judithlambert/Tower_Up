using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour

{
    const int VITESSE_ROTATION_MIN = 20; //degré par sec
    const int VITESSE_ROTATION_MAX = 90;
    const float VITESSE_AVANCÉ_Z = 1; //unité par sec
    const float AVANCÉ_MIN = -4.8f;
    const float AVANCÉ_MAX = 1.7f;
    const float ANGLE_VIS_À_VIS = 0.5f;
    const float INTERVALLE_APPARITION_PROJECTILE = 5;

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

    void Start()
    {
        VitesseRotation = VITESSE_ROTATION_MIN;
        animator = GetComponent<Animator>();
        //Tongue = GameObject.FindGameObjectWithTag("Rino").GetComponentsInChildren<GameObject>().Where(x => x.name == "Tongue").First();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetHit();
    }

    void Update()
    {
        if (VisÀVisPersonnage())
        {
            //GetHit();
            //Attack();
            Shout();
            NouvelleVitesseAléatoire();
            NouvelleAvancéAléatoire();
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Shout"))
        {
            RotationVersPersonnage();
            

            //deltaTemps += Time.deltaTime;
            //if (deltaTemps >= INTERVALLE_APPARITION_PROJECTILE)
            //{
            //    CracheProjectile();
            //    deltaTemps = 0;
            //}
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
        animator.SetTrigger("Die");
    }
    public void Shout()
    {
        animator.SetTrigger("Shout");
    }
    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }

    public void CracheProjectile()
    {
        animator.SetTrigger("Shout");
        Projectile proj = new Projectile();
        proj.Initialisation(transform.position + 10*transform.forward + 10*Vector3.up, 50, 1, 20);
    }
}
