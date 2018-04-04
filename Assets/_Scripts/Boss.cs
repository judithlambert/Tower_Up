using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour {
    Animator animator;



    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        gameObject.AddComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        GetHit();
    }

    // Update is called once per frame
    void Update () {
		
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
}
