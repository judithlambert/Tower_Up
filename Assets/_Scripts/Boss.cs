using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour

{
    const int VITESSE_ROTATION = 30; //degré par sec
    const float ANGLE_VIS_À_VIS = 0.5f;
    Animator animator;
    GameObject Tongue;

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

    const float INTERVALLE_APPARITION_PROJECTILE = 5;

    void Start()
    {
        VitesseRotation = VITESSE_ROTATION;
        animator = GetComponent<Animator>();
        //Tongue = GameObject.FindGameObjectWithTag("Rino").GetComponentsInChildren<GameObject>().Where(x => x.name == "Tongue").First();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetHit();
    }

    float deltaTemps;

    void Update()
    {
        if (!VisÀVisPersonnage())
        {
            DéplacementVersPersonnage();
        }
        else
        {
            animator.GetCurrentAnimatorStateInfo(0).IsName("Shout");
            Shout();

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
        if (Mathf.Abs(AngleBossPersonnage) <= 0.5f) { b = true; }
        return b;
    }

    void DéplacementVersPersonnage()
    {
        Walk();
        transform.RotateAround(Vector3.zero, Vector3.up, (AngleBossPersonnage < 0 ? -1 : 1) * Time.deltaTime * VitesseRotation);            
    }
    
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void Walk()
    {
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
        animator.ResetTrigger("Walk");
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
