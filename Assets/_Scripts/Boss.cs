using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class Boss : MonoBehaviour
{
    float lastTimeShout;
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
        Tongue = GameObject.Find("Tongue");
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
        else { // vis à vis le personnage
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                if (lastTimeShout + 4.667f <= Time.time)
                {
                    CracheProjectile();
                    lastTimeShout = Time.time;
                }
            }
        }
    }

    bool VisÀVisPersonnage()
    {
        bool b = false;
        if (Mathf.Abs(AngleBossPersonnage) <= ANGLE_VIS_À_VIS) { b = true; }
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
        Shout();
        StartCoroutine(Wait());
       
        //GameObject proj = new GameObject("projectile");
        //proj.AddComponent<Projectile>().Initialisation(transform.position + 10*transform.forward + 10*Vector3.up, 1, 50, 2, 20);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.3f);
        Vector3 PositionTongue = GameObject.FindGameObjectWithTag("Rino").transform.worldToLocalMatrix * Tongue.transform.position;
        //Vector3 PositionTongue = GameObject.FindGameObjectWithTag("Rino").transform.worldToLocalMatrix * (Tongue.transform.localToWorldMatrix * Tongue.transform.position);
        GameObject proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), PositionTongue, Quaternion.identity);
        proj.AddComponent<Projectile>().Initialisation(0.5f, 30, 0, 20);
    }
}
