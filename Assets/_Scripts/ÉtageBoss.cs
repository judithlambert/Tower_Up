using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class ÉtageBoss : MonoBehaviour
{
    //[SerializeField] AudioSource AudioSource;
    //[SerializeField] AudioClip VictoireClip;

    const float INTERVALLE_APPARITION_PROJECTILE = 0.6f;
    const float HAUTEUR_ACTIVATION_PROJECTILES = 7.5f;
    float deltaTemps;
    public List<Vector3> ListSommetsPics1e, ListSommetsPics2e, ListSommetsPics3e;
    List<GameObject> ListGameObject = new List<GameObject>();
    GameObject Boss, TxtVictoire;

    public static GameObject BarreDeVieBoss;

    GameObject canvas;
    static public GameObject MessagePanel;

    void Start()
    {
        //AudioSource.clip = ÉtageBossClip;
        //AudioSource.Play();
        DataÉtage.Musique.Boss();
        DataÉtage.TourGameObject.transform.position = new Vector3(0, DataÉtage.PlancherGameObject.transform.position.y - 2);
        Boss = Instantiate(Resources.Load<GameObject>("Prefabs/Boss"), new Vector3(0, DataÉtage.TourGameObject.transform.position.y, 0), Quaternion.Euler(Vector3.zero));
        BarreDeVieBoss = Instantiate(Resources.Load<GameObject>("Prefabs/BarreDeVieBoss"), new Vector2(0, 0), Quaternion.Euler(Vector3.zero));
        BarreDeVieBoss.transform.SetParent(DataÉtage.Ui.transform);
        //DataÉtage.BossScript = Boss.GetComponent<Boss>();
        //Plane.transform.position = new Vector3(0, -250);

        //do
        //{
            //canvas = GameObject.Find("Canvas");
            //MessagePanel = Instantiate(Resources.Load<GameObject>("Prefabs/PnlMessageProjectilesBoss"), new Vector2(0, 0), Quaternion.Euler(Vector2.zero), canvas.transform);
        //    Maths.messageProjDéjaAfficher = true;
        //} while (!Maths.messageProjDéjaAfficher);

        string obj = "PlateformesSupport";
        string obj1 = "PlateformeÉlévation";
        string obj2 = "PlateformePic";
        for (int i = 0; i < 3; ++i)
        {
            ListGameObject.Add(new GameObject(obj + i));
            ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(120 * i, 20, 2 * DataÉtage.DELTA_HAUTEUR, 0, 2 * DataÉtage.DELTA_HAUTEUR, 0.8f, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj1 + i));
            ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(120 * i, 20, 2.5f * DataÉtage.DELTA_HAUTEUR, 0, 0.5f * DataÉtage.DELTA_HAUTEUR, DataÉtage.LARGEUR_PLATEFORME, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));

            ListGameObject.Add(new GameObject(obj + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(120 * i + 55, 10, 4.5f * DataÉtage.DELTA_HAUTEUR, 0, 4.5f * DataÉtage.DELTA_HAUTEUR, 0.6f, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(120 * i + 55, 10, 4.5f * DataÉtage.DELTA_HAUTEUR, 0, 4.5f * DataÉtage.DELTA_HAUTEUR, 0.6f, DataÉtage.RAYON_TOUR + 2.4f, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj1 + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(120 * i + 55, 10, 5 * DataÉtage.DELTA_HAUTEUR, 0, 0.5f * DataÉtage.DELTA_HAUTEUR, DataÉtage.LARGEUR_PLATEFORME, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
        }
        int nbPic = 7;
        for (int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 1er " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().InitialisationPP(360 / nbPic * i, 3, 1, 0, 50, 1, DataÉtage.RAYON_TOUR + 6, 2, 0, Materials.Get((int)NomMaterial.Tour));
        }
        nbPic = 16;
        for (int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 2e " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().InitialisationPP(360 / nbPic * i, 7, 4, 0, 100, 2, DataÉtage.RAYON_TOUR + 14, 4, 0, Materials.Get((int)NomMaterial.Plateforme));
        }
        nbPic = 5;
        for (int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 3e " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().InitialisationPP(360 / nbPic * i, 7, 20, 0, 150, 2, DataÉtage.RAYON_TOUR + 35, 8, 0, Materials.Get((int)NomMaterial.Tour));
        }

        ListSommetsPics1e = new List<Vector3>();
        ListSommetsPics2e = new List<Vector3>();
        ListSommetsPics3e = new List<Vector3>();
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (g.name.Contains("Pic 1er")) { ListSommetsPics1e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("Pic 2e")) { ListSommetsPics2e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("Pic 2e")) { ListSommetsPics3e.Add(g.GetComponent<PlateformePics>().SommetPic); }
        }
    }

    void Update()
    {
        if (!DataÉtage.pause && !DataÉtage.victoire)
        {
            deltaTemps += Time.deltaTime;
            if (deltaTemps >= INTERVALLE_APPARITION_PROJECTILE && DataÉtage.PersonnageGameObject.transform.position.y >= HAUTEUR_ACTIVATION_PROJECTILES)
            {
                CercleSeulAléatoire(ListSommetsPics2e);
                deltaTemps = 0;
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.L))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileP"), (new Vector3(0, DataÉtage.PersonnageGameObject.transform.position.y, 0) - DataÉtage.PersonnageGameObject.transform.position).normalized * DataÉtage.PersonnageGameObject.transform.lossyScale.y * 0.6f + DataÉtage.PersonnageGameObject.transform.position + new Vector3(0, DataÉtage.PersonnageGameObject.transform.lossyScale.y * 0.6f, 0), Random.rotation);
            }
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
                GameObject proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[index] + new Vector3(0, 2, 0), Quaternion.identity);
                proj.AddComponent<Projectile>().Initialisation(1, 40, 3, 20);
                //Projectile proj = new Projectile();
                //proj.Initialisation(list[index] + new Vector3(0, 2, 0), 1, 50, 3, 20);
                //Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[index] + new Vector3(0, 2, 0), Quaternion.identity);
                instancié = true;
            }
        }    
    }

    void CercleSeul(List<Vector3> list)
    {
        for(int i = 0; i < list.Count; ++i)
        {
            GameObject proj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[i] + new Vector3(0, 2, 0), Quaternion.identity);
            proj.AddComponent<Projectile>().Initialisation(1, 40, 3, 20);
            //Projectile proj = new Projectile();
            //proj.Initialisation(list[i] + new Vector3(0, 2, 0), 1, 50, 3, 20);
            //Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), list[i] + new Vector3(0, 2, 0), Quaternion.identity);
        }
    }

    public void Victoire()
    {
        DataÉtage.Musique.Victoire();
        DataÉtage.victoire = true;
        foreach(GameObject g in FindObjectsOfType<GameObject>())
        {
            if (g.name.Contains("Projectile")) { Destroy(g); }
        }
        DataÉtage.Ui.SetActive(false);
        DataÉtage.UiFinÉtage.SetActive(true);
        TxtVictoire = Instantiate(Resources.Load<GameObject>("Prefabs/Victoire"),DataÉtage.UiFinÉtage.transform);
        DataÉtage.UiFinÉtage.GetComponentsInChildren<Button>().Where(x => x.name.Contains("Prochain")).First().interactable = false;

        foreach (Vector3 p in ListSommetsPics2e)
        {
            ListGameObject.Add(Instantiate(Resources.Load<GameObject>("Effects/ParticuleVictoire"), p, Quaternion.Euler(-90,0,0)));
        }
    }

    public void Recommencer()
    {
        foreach(GameObject g in ListGameObject)
        {
            Destroy(g);
        }
        Destroy(GameObject.FindGameObjectWithTag("Boss"));
        Destroy(BarreDeVieBoss);
        Destroy(TxtVictoire);
        DataÉtage.victoire = false;
        DataÉtage.NouvelÉtage(true);
        Destroy(this);
        Destroy(MessagePanel);
    }
}
