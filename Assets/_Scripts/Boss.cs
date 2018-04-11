using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour {
    Animator animator;
    GameObject Tongue;
    float différenceAngle;
    float angleRotation;

    const float INTERVALLE_APPARITION_PROJECTILE = 5;

    void Start()
    {
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
        angleRotation = transform.rotation.eulerAngles.y - 90;
        angleRotation += (angleRotation < 0 ? 360 : 0);
        différenceAngle = DataÉtage.PersonnageScript.AngleOrigineÀPosition - angleRotation;
        if (Mathf.Abs(différenceAngle) >= 0.5f)
        {
            transform.RotateAround(Vector3.zero, Vector3.up, (différenceAngle < 0 ? -1 : 1) * (Mathf.Abs(différenceAngle) > 180 ? -1 : 1) * Time.deltaTime * 25);
            Walk();
        }
        else
        {
            animator.ResetTrigger("Walk");
            //Shout();

            deltaTemps += Time.deltaTime;
            if (deltaTemps >= INTERVALLE_APPARITION_PROJECTILE)
            {
                CracheProjectile();
                deltaTemps = 0;
            }
        }


        
    }

    void DéplacementVersPersonnage()
    {
        angleRotation = transform.rotation.eulerAngles.y - 90;
        angleRotation += (angleRotation < 0 ? 360 : 0);
        différenceAngle = DataÉtage.PersonnageScript.AngleOrigineÀPosition - angleRotation;
        if (Mathf.Abs(différenceAngle) >= 0.5f)
        {
            transform.RotateAround(Vector3.zero, Vector3.up, (différenceAngle < 0 ? -1 : 1) * (Mathf.Abs(différenceAngle) > 180 ? -1 : 1) * Time.deltaTime * 25);
            Walk();
        }
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
