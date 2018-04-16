﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉtageBoss : MonoBehaviour
{
    float deltaTemps;
    public List<Vector3> ListSommetsPics1e, ListSommetsPics2e, ListSommetsPics3e;

    void Start()
    {
        ListSommetsPics1e = new List<Vector3>();
        ListSommetsPics2e = new List<Vector3>();
        ListSommetsPics3e = new List<Vector3>();
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (g.name.Contains("pic 1e")) { ListSommetsPics1e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("pic 2e")) { ListSommetsPics2e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("pic 2e")) { ListSommetsPics3e.Add(g.GetComponent<PlateformePics>().SommetPic); }
        }
    }

    //void Update()
    //{
    //    deltaTemps += Time.deltaTime;
    //    if (deltaTemps >= 1 / 3f)
    //    {
    //        CercleSeulAléatoire(ListSommetsPics2e);
    //        deltaTemps = 0;
    //    }
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/ProjectilePersonnage"), (new Vector3(0, DataÉtage.PersonnageGameObject.transform.position.y, 0) - DataÉtage.PersonnageGameObject.transform.position).normalized * DataÉtage.PersonnageGameObject.transform.lossyScale.y * 0.6f + DataÉtage.PersonnageGameObject.transform.position + new Vector3(0, DataÉtage.PersonnageGameObject.transform.lossyScale.y * 0.6f, 0), Random.rotation);
        }
    }

    void CercleSeulAléatoire(List<Vector3> list)
    {
        bool instancié = false;
        while (!instancié)
        {
            int index = Mathf.RoundToInt(Random.value * (list.Count - 1));
            if (!Physics.CheckSphere(list[index] + new Vector3(0, 2, 0), 1))
            {
                Projectile proj = new Projectile();
                proj.Initialisation(list[index] + new Vector3(0, 2, 0), 50, 3, 20);
                //Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[index] + new Vector3(0, 2, 0), Quaternion.identity);
                instancié = true;
            }
        }    
    }

    void CercleSeul(List<Vector3> list)
    {
        for(int i = 0; i < list.Count; ++i)
        {
            Projectile proj = new Projectile();
            proj.Initialisation(list[i] + new Vector3(0, 2, 0), 50, 3, 20);
            //Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[i] + new Vector3(0, 2, 0), Quaternion.identity);
        }
    }
}
