using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;
//using UnityEditor;

public class DataÉtage : MonoBehaviour
{
    [SerializeField] bool GODMOD;

    [SerializeField] int TEST_ÉTAGE;

    public const float HAUTEUR_TOUR = 35,
                       RAYON_TOUR = 10, // si distance < rayon
                       DELTA_HAUTEUR = 2, //Hauteur entre 2 plateformes
                       LARGEUR_PLATEFORME = 3; //Largeur des objets
    const string CHEMIN_DATA_ÉTAGE= "Assets/Data/";
    const char SÉPARATEUR = ';';
    const int ÉTAGE_BOSS = 5;
    const float DISTANCE_CAMERA_PERSONNAGE = 10; // ratio avec tour 
    public const int DIFFICULTÉ_DE_BASE = (int)Difficulté.Normale;


    int NB_MAX_JUMP = 2;

    static public GameObject PersonnageGameObject, PlancherGameObject, TourGameObject;
    static public Camera Caméra;
    public static Personnage PersonnageScript;
    public static Plateforme PlancherScript;
    //[SerializeField] GameObject prefabPersonnage;
    GameObject prefabPersonnage;
    public static GameObject Ui;
    public static UI UiScript;
    public static GameObject UiFinÉtage;
    public static UIFinÉtage UiFinÉtageScript;
    List<GameObject> ListGameObject;
    public static GameObject Plane;

    //[SerializeField] GameObject prefabBoss;
    static public GameObject BossGameObject;
    public static Boss BossScript;
    public static GameObject BarreDeVieBoss;
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;

    public static Vector3 Origine = Vector3.zero;

    StreamReader étageReader;
    StreamWriter saveWriter;

    static public bool étageFini = false;
    static public bool jeuFini = false;
    static public bool nouvelÉtage = false;

    public static int nbÉtage { get; set; }
    public static bool pause { get; private set; }
    public static bool étageEnCour { get; private set; }

    public static int difficulté = DIFFICULTÉ_DE_BASE;
    public enum Difficulté { GodMode, Normale, Difficile };

        
    private void Awake()
    {
        // for testing
        nbÉtage = TEST_ÉTAGE;
        if (GODMOD) { difficulté = (int)Difficulté.GodMode; }
        //---


        Materials.Init();

        ListGameObject = new List<GameObject>();

        // instanciation du placher, personnage, camera
        PlancherGameObject = new GameObject("Plancher");
        PlancherGameObject.AddComponent<Plateforme>().Initialisation(0, 360, 0, 0, 20, LARGEUR_PLATEFORME, RAYON_TOUR,0, Materials.Get((int)NomMaterial.Plateforme));
        TourGameObject = new GameObject("Tour");
        TourGameObject.AddComponent<Plateforme>().Initialisation(0, 360, HAUTEUR_TOUR * DELTA_HAUTEUR, 0, HAUTEUR_TOUR * DELTA_HAUTEUR, RAYON_TOUR,0, 0, Materials.Get((int)NomMaterial.Tour));
        RayonTrajectoirePersonnage = RAYON_TOUR + PlancherGameObject.GetComponent<Plateforme>().Largeur / 2;
        RayonCamera = RayonTrajectoirePersonnage + DISTANCE_CAMERA_PERSONNAGE;
        prefabPersonnage = Resources.Load<GameObject>("Prefabs/Personnage");
        PersonnageGameObject = Instantiate(prefabPersonnage, new Vector3(RayonTrajectoirePersonnage, prefabPersonnage.transform.lossyScale.y/2, 0), Quaternion.Euler(Vector3.zero));
        PersonnageScript = PersonnageGameObject.GetComponent<Personnage>();
        Ui = GameObject.FindGameObjectWithTag("UI");
        UiScript = Ui.GetComponent<UI>();
        UiFinÉtage = GameObject.FindGameObjectWithTag("UIFinÉtage");
        UiFinÉtageScript = UiFinÉtage.GetComponent<UIFinÉtage>();
        UiFinÉtage.SetActive(false);
        Caméra = Camera.main;
        Caméra.gameObject.AddComponent<CameraControlleur>();
        Plane = GameObject.Find("Plane");


     


        LoadÉtage();
        étageEnCour = true;
    }
    
    private void Start()
    {
        saveWriter = new StreamWriter(Menu.CHEMIN_SAVE);
        Save();
    }

    public void LoadÉtage()
    {
        if(nbÉtage != ÉTAGE_BOSS)
        {
            étageReader = new StreamReader(CHEMIN_DATA_ÉTAGE + "Étage" + nbÉtage.ToString() + ".txt");

            do
            {
                string obj;
                do
                {
                    obj = étageReader.ReadLine();
                } while (obj == "" || obj[0] == '/');

                string[] line = étageReader.ReadLine().Split(SÉPARATEUR);
                float[] attributs = new float[line.Length];
                for (int cpt = 0; cpt < line.Length; ++cpt)
                {
                    attributs[cpt] = float.Parse(line[cpt]);
                }

                //if(obj.Contains("Point")) { ListPoint.Add(new GameObject(obj)); }
                ListGameObject.Add(new GameObject(obj));

                obj = obj.Split(' ')[0]; // obj.Remove(' ');
                switch (obj)
                {
                    //Gestion Angle ne sert à rien
                    case Plateforme.String:
                        ListGameObject.Last().AddComponent<Plateforme>().Initialisation(attributs[0],
                                                                                        attributs[1],
                                                                                        attributs[2] * DELTA_HAUTEUR,
                                                                                        attributs[3],
                                                                                        attributs[4] * DELTA_HAUTEUR,
                                                                                        LARGEUR_PLATEFORME,
                                                                                        RAYON_TOUR,
                                                                                        0,
                                                                                        Materials.Get((int)NomMaterial.Plateforme));
                        break;
                    case PlateformeMobile.String:
                        ListGameObject.Last().AddComponent<PlateformeMobile>().Initialisation(attributs[0],
                                                                                              attributs[1],
                                                                                              attributs[2] * DELTA_HAUTEUR,
                                                                                              attributs[3],
                                                                                              attributs[4] * DELTA_HAUTEUR,
                                                                                              LARGEUR_PLATEFORME,
                                                                                              RAYON_TOUR,
                                                                                              attributs[5],
                                                                                              attributs[6],
                                                                                              attributs.Length >= 8 ? (int)attributs[7] : 0,
                                                                                              attributs.Length >= 9 ? (int)attributs[8] : 0,
                                                                                              Materials.Get((int)NomMaterial.Plateforme));
                        break;
                    case PlateformeTemporaire.String:
                        ListGameObject.Last().AddComponent<PlateformeTemporaire>().Initialisation(attributs[0],
                                                                                                  attributs[1],
                                                                                                  attributs[2] * DELTA_HAUTEUR,
                                                                                                  attributs[3],
                                                                                                  attributs[4] * DELTA_HAUTEUR,
                                                                                                  LARGEUR_PLATEFORME,
                                                                                                  RAYON_TOUR,
                                                                                                  attributs[5],
                                                                                                  attributs.Length >= 7 ? (int)attributs[6] : 0,
                                                                                                  attributs.Length >= 8 ? (int)attributs[7] : 0,
                                                                                                  Materials.Get((int)NomMaterial.Plateforme));
                        break;
                    case PlateformePics.String:
                        ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(attributs[0],
                                                                                            attributs[1],
                                                                                            attributs[2] * DELTA_HAUTEUR,
                                                                                            attributs[3],
                                                                                            attributs[4] * DELTA_HAUTEUR,
                                                                                            LARGEUR_PLATEFORME,
                                                                                            RAYON_TOUR,
                                                                                            attributs[5] * DELTA_HAUTEUR,
                                                                                            attributs.Length >= 7 ? (int)attributs[6] : 0,
                                                                                            Materials.Get((int)NomMaterial.Plateforme));
                        break;
                    case Pic.String:
                        ListGameObject.Last().AddComponent<Pic>().Initialisation(attributs[0],
                                                                                 attributs[1] * DELTA_HAUTEUR,
                                                                                 attributs[2] * DELTA_HAUTEUR,
                                                                                 LARGEUR_PLATEFORME / 2.4f,
                                                                                 Materials.Get((int)NomMaterial.Pic));
                        break;
                    case FinÉtage.String:
                        ListGameObject.Last().AddComponent<FinÉtage>().Initialisation(attributs[0],
                                                                                      attributs[1] * DELTA_HAUTEUR);
                        break;
                    case Point.String:
                        ListGameObject.Last().AddComponent<Point>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR + 0.5f * DELTA_HAUTEUR, attributs[2] == 0 ? false : true, attributs[3], attributs[2] == 0 ? Materials.Get((int)NomMaterial.Point) : Materials.Get((int)NomMaterial.Multiplicateur));
                        break;
                    case CheckPoint.String:
                        ListGameObject.Last().AddComponent<CheckPoint>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR);
                        break;
                    case Flèche.String:
                        ListGameObject.Last().AddComponent<Flèche>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR, RAYON_TOUR, attributs[2]);
                        break;

                }

            } while (!étageReader.EndOfStream);
        }
        else { LoadÉtageBoss(); }
    }

    void Save()
    {
        saveWriter.Write(nbÉtage);
    }

    int cptNaming = 0;
    string NewName(string n)
    {
        return n + ' ' + (cptNaming++).ToString();
    }

    private void Update()
    {
        if (étageFini) { FinirÉtage(); }
        if (nouvelÉtage) { NouvelÉtage(); }
        if (étageEnCour && (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) { pause = !pause; PauseUnPause(); }

    }

    void FinirÉtage()
    {
        foreach (GameObject g in ListGameObject)
        {
            if (g != null && (g.name.Contains("Point")))
            { g.GetComponent<Point>().Destroy(); }
            else if (g != null && g.name.Contains("Check"))
            { g.GetComponent<CheckPoint>().Destroy(); }
            else { Destroy(g); };
        }
        ListGameObject.Clear();
        Save();
        UiFinÉtage.SetActive(true);
        UiFinÉtageScript.FinÉtage();
        Ui.SetActive(false);
        étageFini = false;
        étageEnCour = false;
    }

    void NouvelÉtage()
    {
        UiFinÉtage.SetActive(false);
        PersonnageScript.Réinitialiser();
        nbÉtage++;
        LoadÉtage();
        Ui.SetActive(true);
        UiScript.Réinitialiser();
        nouvelÉtage = false;
        étageEnCour = true;
    }

    void PauseUnPause()
    {
        Ui.SetActive(!Ui.activeSelf);
        UiFinÉtage.SetActive(!UiFinÉtage.activeSelf);
        if(UiFinÉtageScript != null) { UiFinÉtageScript.DonnéesDeBase(); }
        UiFinÉtage.GetComponentsInChildren<Button>().Where(x => x.name.Contains("Prochain")).First().enabled = false;
        PersonnageGameObject.GetComponent<Rigidbody>().isKinematic = !PersonnageGameObject.GetComponent<Rigidbody>().isKinematic;
    }

    void LoadÉtageBoss()
    {        
        TourGameObject.transform.position = new Vector3(0, PlancherGameObject.transform.position.y - 2);
        TourGameObject.AddComponent<ÉtageBoss>();
        BossGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Boss"), new Vector3(0, TourGameObject.transform.position.y,0), Quaternion.Euler(Vector3.zero));
        BossScript = BossGameObject.GetComponent<Boss>();
        Plane.transform.position = new Vector3(0, -250);

        
        BarreDeVieBoss = Instantiate(Resources.Load<GameObject>("Prefabs/BarreDeVieBoss"), new Vector2(0,0), Quaternion.Euler(Vector3.zero));
        BarreDeVieBoss.transform.SetParent(Ui.transform);

        string obj = "support";
        string obj1 = "élévation";
        string obj2 = "pic";
        for(int i = 0; i < 3; ++i)
        {
            ListGameObject.Add(new GameObject(obj + i));
            ListGameObject.Last().AddComponent<Plateforme>().Initialisation(120 * i, 20, 2 * DELTA_HAUTEUR, 0, 2 * DELTA_HAUTEUR, 0.8f, RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj1 + i));
            ListGameObject.Last().AddComponent<Plateforme>().Initialisation(120 * i, 20, 2.5f * DELTA_HAUTEUR, 0, 0.5f * DELTA_HAUTEUR, LARGEUR_PLATEFORME, RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));

            ListGameObject.Add(new GameObject(obj + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().Initialisation(120 * i + 55, 10, 4.5f * DELTA_HAUTEUR, 0, 4.5f * DELTA_HAUTEUR, 0.6f, RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().Initialisation(120 * i + 55, 10, 4.5f * DELTA_HAUTEUR, 0, 4.5f * DELTA_HAUTEUR, 0.6f, RAYON_TOUR + 2.4f, 0, Materials.Get((int)NomMaterial.Plateforme));
            ListGameObject.Add(new GameObject(obj1 + 3 + i));
            ListGameObject.Last().AddComponent<Plateforme>().Initialisation(120 * i + 55, 10, 5 * DELTA_HAUTEUR, 0, 0.5f * DELTA_HAUTEUR, LARGEUR_PLATEFORME, RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
        }
        int nbPic = 7;
        for(int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 1er " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(360 / nbPic * i, 3, 1, 0, 50, 1, RAYON_TOUR + 6, 2, 0, Materials.Get((int)NomMaterial.Tour));
        }
        nbPic = 16;
        for (int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 2e " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(360 / nbPic * i, 7, 4, 0, 100, 2, RAYON_TOUR + 14, 4, 0, Materials.Get((int)NomMaterial.Plateforme));
        }
        nbPic = 5;
        for (int i = 0; i < nbPic; ++i)
        {
            ListGameObject.Add(new GameObject(obj2 + " 3e " + i));
            ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(360 / nbPic * i, 7, 20, 0, 150, 2, RAYON_TOUR + 35, 8, 0, Materials.Get((int)NomMaterial.Tour));
        }
    }
}
