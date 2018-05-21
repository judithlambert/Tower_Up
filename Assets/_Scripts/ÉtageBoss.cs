using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ÉtageBoss : MonoBehaviour
{
    public const int
        NB_STRUCTURES = 3;
    public const float
        AMPLITUDE_STRUCTURE_BASSE = 20f,
        AMPLITUDE_STRUCTURE_HAUTE = 10f,
        HAUTEUR_STRUCTURE_BASSE = 2.5f,
        HAUTEUR_STRUCTURE_HAUTE = 5f,
        LARGEUR_PILIER_BAS = 0.8f,
        LARGEUR_PILIER_HAUT = 0.6f,
        ÉPAISSEUR_STRUCTURE = 0.5f,
        INTERVALLE_APPARITION_PROJECTILE_2E = 1.5f,
        INTERVALLE_APPARITION_PROJECTILE_3E = 2.5f,
        HAUTEUR_ACTIVATION_PROJECTILES_2E = 7.5f,
        HAUTEUR_ACTIVATION_PROJECTILES_3E = 5f,
        DELTA_HAUTEUR_TOUR = -2f,
        ESPACEMENT_AU_DESSUS_APEX = 2f,
        ESPACEMENT_AUTOUR_PERSONNAGE = 0.6f;
    public const string
        PS = "PlateformeSupport",
        PÉ = "PlateformeÉlévation",
        PP = "PlateformePic",
        B = "Basse",
        H = "Haute";

    /// <summary> 
    /// NB_PICS, AMPLITUDE, HAUTEUR, INCLINAISON, ÉPAISSEUR, LARGEUR, RAYON_AJOUTÉ, HAUTEUR_PIC, ROTATION
    /// </summary>
    public int[,] Cercles { get { return Maths.CopieProfonde(ref cercles); } }
    int[,] cercles = new int[,] { { 7, 3, 1, 0, 50, 1, 6, 2, 0 },
                                  { 16, 7, 4, 0, 100, 2, 14, 4, 0 },
                                  { 5, 7, 20, 0, 150, 2, 35, 8, 0 } };
    /// <summary>
    /// DIAMÈTRE, VITESSE, TEMPS_APPARITION, TEMPS_MOURRIR
    /// </summary>
    public float[,] Projectiles { get { return Maths.CopieProfonde(ref projectiles); } }
    float[,] projectiles = new float[,] { { 1, 40, 3, 20 },
                                          { 1.5f, 60, 3, 20 } };

    float deltaTemps3e, deltaTemps2e, deltaAngleStructure, déphasageStructureHaute;
    float[] cercle;
    List<Vector3> ListApex1e, ListApex2e, ListApex3e;
    List<GameObject> ListGameObject;
    GameObject Boss, TxtVictoire, BarreDeVieBoss;

    public Vector3 PositionInitialeProjectilePersonnage
    {
        get { return (new Vector3(0, DataÉtage.PersonnageGameObject.transform.position.y, 0) - DataÉtage.PersonnageGameObject.transform.position).normalized * DataÉtage.PersonnageGameObject.transform.lossyScale.y * ESPACEMENT_AUTOUR_PERSONNAGE + DataÉtage.PersonnageGameObject.transform.position + new Vector3(0, DataÉtage.PersonnageGameObject.transform.lossyScale.y * ESPACEMENT_AUTOUR_PERSONNAGE, 0); }
    }

    private void Awake()
    {
        ListGameObject = new List<GameObject>();
        deltaAngleStructure = 360 / NB_STRUCTURES;
        déphasageStructureHaute = (deltaAngleStructure - AMPLITUDE_STRUCTURE_BASSE - AMPLITUDE_STRUCTURE_HAUTE) / 2 + AMPLITUDE_STRUCTURE_BASSE;
    }

    void Start()
    {       
        DataÉtage.Musique.Boss();
        DataÉtage.TourGameObject.transform.position = new Vector3(0, DataÉtage.PlancherGameObject.transform.position.y + DELTA_HAUTEUR_TOUR);
        Boss = Instantiate(Resources.Load<GameObject>("Prefabs/Boss"), new Vector3(0, DataÉtage.TourGameObject.transform.position.y, 0), Quaternion.Euler(Vector3.zero));
        BarreDeVieBoss = Instantiate(Resources.Load<GameObject>("Prefabs/BarreDeVieBoss"), new Vector2(0, 0), Quaternion.Euler(Vector3.zero));
        BarreDeVieBoss.transform.SetParent(DataÉtage.Ui.transform);

        GénérerStructures();
        GénérerPlateformePics();
        GénérerListeApex();   
    }

    void GénérerStructures()
    {     
        for (int i = 0; i < NB_STRUCTURES; ++i)
        {
            GénérerStructuresBasses(i);
            GénérerStructuresHautes(i);
        }
    }

    void GénérerStructuresBasses(int i)
    {
        //Support
        ListGameObject.Add(new GameObject(PS + B + i));
        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(deltaAngleStructure * i, AMPLITUDE_STRUCTURE_BASSE, (HAUTEUR_STRUCTURE_BASSE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, 0, (HAUTEUR_STRUCTURE_BASSE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, LARGEUR_PILIER_BAS, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
        //Plateforme
        ListGameObject.Add(new GameObject(PÉ + B + i));
        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(deltaAngleStructure * i, AMPLITUDE_STRUCTURE_BASSE, HAUTEUR_STRUCTURE_BASSE * DataÉtage.DELTA_HAUTEUR, 0, ÉPAISSEUR_STRUCTURE * DataÉtage.DELTA_HAUTEUR, DataÉtage.LARGEUR_PLATEFORME, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
    }

    void GénérerStructuresHautes(int i)
    {
        //Support gauche
        ListGameObject.Add(new GameObject(PS + H + i));
        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(deltaAngleStructure * i + déphasageStructureHaute, AMPLITUDE_STRUCTURE_HAUTE, (HAUTEUR_STRUCTURE_HAUTE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, 0, (HAUTEUR_STRUCTURE_HAUTE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, LARGEUR_PILIER_HAUT, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
        //Support droit
        ListGameObject.Add(new GameObject(PS + H + i + "'"));
        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(deltaAngleStructure * i + déphasageStructureHaute, AMPLITUDE_STRUCTURE_HAUTE, (HAUTEUR_STRUCTURE_HAUTE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, 0, (HAUTEUR_STRUCTURE_HAUTE - ÉPAISSEUR_STRUCTURE) * DataÉtage.DELTA_HAUTEUR, LARGEUR_PILIER_HAUT, DataÉtage.RAYON_TOUR + DataÉtage.LARGEUR_PLATEFORME - LARGEUR_PILIER_HAUT, 0, Materials.Get((int)NomMaterial.Plateforme));
        //Plateforme
        ListGameObject.Add(new GameObject(PÉ + H + i));
        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(deltaAngleStructure * i + déphasageStructureHaute, AMPLITUDE_STRUCTURE_HAUTE, HAUTEUR_STRUCTURE_HAUTE * DataÉtage.DELTA_HAUTEUR, 0, ÉPAISSEUR_STRUCTURE * DataÉtage.DELTA_HAUTEUR, DataÉtage.LARGEUR_PLATEFORME, DataÉtage.RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
    }

    void GénérerPlateformePics()
    {
        for(int i = 0; i < Cercles.GetLength(0); ++i)
        {
            cercle = new float[Cercles.GetLength(1)];
            for(int j = 0; j < Cercles.GetLength(1); ++j)
            {
                cercle[j] = Cercles[i, j];
            }
            GénérerCercle(cercle, i + 1);
        }   
    }

    void GénérerCercle(float[] cercle, int nièmeCercle)
    {
        for (int i = 0; i < cercle[0]; ++i)
        {
            ListGameObject.Add(new GameObject(PP + nièmeCercle + "e " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().InitialisationPP(360 / cercle[0] * i, cercle[1], cercle[2], cercle[3], cercle[4], cercle[5], DataÉtage.RAYON_TOUR + cercle[6], cercle[7], cercle[8], Materials.Get((int)NomMaterial.Tour));
        }
    }

    void GénérerListeApex()
    {
        ListApex1e = new List<Vector3>();
        ListApex2e = new List<Vector3>();
        ListApex3e = new List<Vector3>();
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)).Where(x=>x.name.Contains(PP)))
        {
            if (g.name.Contains("1e")) { ListApex1e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("2e")) { ListApex2e.Add(g.GetComponent<PlateformePics>().SommetPic); }
            else if (g.name.Contains("3e")) { ListApex3e.Add(g.GetComponent<PlateformePics>().SommetPic); }
        }
    }

    void Update()
    {
        if (!DataÉtage.pause && !DataÉtage.victoire)
        {
            LancerProjectile();
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.L))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileP"), PositionInitialeProjectilePersonnage, Random.rotation);
            }
        }
    }

    void LancerProjectile()
    {
        deltaTemps2e += Time.deltaTime;
        deltaTemps3e += Time.deltaTime;
        if (deltaTemps2e >= INTERVALLE_APPARITION_PROJECTILE_2E && DataÉtage.PersonnageGameObject.transform.position.y >= HAUTEUR_ACTIVATION_PROJECTILES_2E)
        {
            LancerProjectileAléatoire(ListApex2e);
            deltaTemps2e = 0;
        }
        if (deltaTemps3e >= INTERVALLE_APPARITION_PROJECTILE_3E && DataÉtage.PersonnageGameObject.transform.position.y <= HAUTEUR_ACTIVATION_PROJECTILES_3E)
        {
            LancerProjectileSynchronisé(ListApex3e);
            deltaTemps3e = 0;
        }
    }

    void LancerProjectileAléatoire(List<Vector3> apex)
    {
        bool instancié = false;
        while (!instancié)
        {
            int index = Mathf.RoundToInt(Random.value * (apex.Count - 1));
            if (!Physics.CheckSphere(apex[index] + new Vector3(0, ESPACEMENT_AU_DESSUS_APEX, 0), 1))
            {
                GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), apex[index] + new Vector3(0, ESPACEMENT_AU_DESSUS_APEX, 0), Quaternion.identity);
                projectile.AddComponent<Projectile>().Initialisation(Projectiles[0, 0], Projectiles[0, 1], Projectiles[0, 2], Projectiles[0, 3]);
                instancié = true;
            }
        }    
    }

    void LancerProjectileSynchronisé(List<Vector3> apex)
    {
        for(int i = 0; i < apex.Count; ++i)
        {
            if (!Physics.CheckSphere(apex[i] + new Vector3(0, ESPACEMENT_AU_DESSUS_APEX, 0), 1))
            {
                GameObject projectile = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), apex[i] + new Vector3(0, ESPACEMENT_AU_DESSUS_APEX, 0), Quaternion.identity);
                projectile.AddComponent<Projectile>().Initialisation(Projectiles[1, 0], Projectiles[1, 1], Projectiles[1, 2], Projectiles[1, 3]);
            }          
        }
    }

    public void Victoire()
    {
        foreach (GameObject g in FindObjectsOfType<GameObject>().Where(x=>x.name.Contains("Projectile"))) { Destroy(g); }
        foreach (Vector3 v in ListApex2e) { ListGameObject.Add(Instantiate(Resources.Load<GameObject>("Effects/ParticuleVictoire"), v, Quaternion.Euler(-90, 0, 0))); }
        DataÉtage.Musique.Victoire();
        DataÉtage.victoire = true;
        DataÉtage.Ui.SetActive(false);
        DataÉtage.UiFinÉtage.SetActive(true);
        DataÉtage.UiFinÉtage.GetComponentsInChildren<Button>().Where(x => x.name.Contains("Prochain")).First().interactable = false;
        TxtVictoire = Instantiate(Resources.Load<GameObject>("Prefabs/Victoire"),DataÉtage.UiFinÉtage.transform);
    }

    public void Recommencer()
    {
        foreach(GameObject g in ListGameObject) { Destroy(g);}
        Destroy(GameObject.FindGameObjectWithTag("Boss"));
        Destroy(BarreDeVieBoss);
        Destroy(TxtVictoire);
        DataÉtage.victoire = false;
        DataÉtage.NouvelÉtage(true);
        Destroy(this);
    }
}
