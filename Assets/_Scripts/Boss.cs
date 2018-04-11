﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour {
    Animator animator;
    GameObject Tongue;
    float différenceAngle;
    float angleRotation;

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
            Shout();
        }


        //deltaTemps += Time.deltaTime;
        //if (deltaTemps >= 2)
        //{
        //    Attaquer();
        //    deltaTemps = 0;
        //}
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

    public void CracheProjectile()
    {
        ////transform.LookAt(DataÉtage.PersonnageGameObject.transform);
        //animator.SetTrigger("Shout");
        //Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), Tongue.transform.localToWorldMatrix * Tongue.transform.position, Quaternion.identity);
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

    public void Attaquer()
    {
        transform.rotation= Maths.Vector3àQuaternion(new Vector3(DataÉtage.PersonnageGameObject.transform.position.x,0, DataÉtage.PersonnageGameObject.transform.position.z));
        Walk();
        //transform.Translate(DataÉtage.PersonnageGameObject.transform.position - (transform.position*2));
        Attack();
        transform.position = new Vector3(0, -4, 0);
    }
}
