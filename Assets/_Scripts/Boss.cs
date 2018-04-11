using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour {
    Animator animator;
    GameObject Tongue;
    void Start() {
        animator = GetComponent<Animator>();
        //Tongue = GameObject.FindGameObjectWithTag("Rino").GetComponentsInChildren<GameObject>().Where(x => x.name == "Tongue").First();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetHit();
    }

    float deltaTemps;

    void Update () {
        deltaTemps += Time.deltaTime;
        if (deltaTemps >= 2)
        {
            Attaquer();
            deltaTemps = 0;
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

    public void Attaquer()
    {
        transform.rotation= Maths.Vector3àQuaternion(new Vector3(DataÉtage.PersonnageGameObject.transform.position.x,0, DataÉtage.PersonnageGameObject.transform.position.z));
        Walk();
        //transform.Translate(DataÉtage.PersonnageGameObject.transform.position - (transform.position*2));
        Attack();
        CracheProjectile();
        transform.position = new Vector3(0, -4, 0);
    }
    public void CracheProjectile()
    {
        animator.SetTrigger("Shout");
        Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), transform.position + transform.forward, Quaternion.identity);

    }
}
