using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using System.IO;
//using UnityEditor;

public class DataÉtage : MonoBehaviour
{

    [SerializeField] bool GODMOD;
    public const string CHEMIN_DATA_ÉTAGE = "Assets/Resources/Data/";
    [SerializeField] int TEST_ÉTAGE;

    public const float HAUTEUR_TOUR = 35,
                       RAYON_TOUR = 10, // si distance < rayon
                       DELTA_HAUTEUR = 2, //Hauteur entre 2 plateformes
                       LARGEUR_PLATEFORME = 3; //Largeur des objets
    const char SÉPARATEUR = ';';
    const int ÉTAGE_BOSS = 5;
    const float DISTANCE_CAMERA_PERSONNAGE = 10; // ratio avec tour 
    public const int DIFFICULTÉ_DE_BASE = (int)Difficulté.Normale;

    public string PlayerName;
    static StreamReader étageReader;
    int NB_MAX_JUMP = 2;

    static public Musique Musique;
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
    static List<GameObject> ListGameObject;

    //[SerializeField] GameObject prefabBoss;
    //static public GameObject BossGameObject;
    //public static Boss BossScript;
    
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;

    public static Vector3 Origine = Vector3.zero;

    static public bool étageFini = false;
    static public bool jeuFini = false;
    static public bool nouvelÉtage = false;
    static public bool recommencer = false;

    public static int nbÉtage { get; set; }
    public static bool pause { get; private set; }
    public static bool étageEnCour { get; private set; }
    public static bool victoire { get; set; }

    public static int difficulté = DIFFICULTÉ_DE_BASE;
    public enum Difficulté { Exploration, Normale, Difficile };

    private void Awake()
    {
        // for testing
        //nbÉtage = TEST_ÉTAGE;
        //if (GODMOD) { difficulté = (int)Difficulté.Exploration; }
        //---

        Materials.Init();

        ListGameObject = new List<GameObject>();

        // instanciation du placher, personnage, camera
        PlancherGameObject = new GameObject("Plancher");
        PlancherGameObject.AddComponent<Plateforme>().InitialisationP(0, 360, 0, 0, 20, LARGEUR_PLATEFORME, RAYON_TOUR,0, Materials.Get((int)NomMaterial.Plateforme));
        TourGameObject = new GameObject("Tour");
        TourGameObject.AddComponent<Plateforme>().InitialisationP(0, 360, HAUTEUR_TOUR * DELTA_HAUTEUR, 0, HAUTEUR_TOUR * DELTA_HAUTEUR, RAYON_TOUR,0, 0, Materials.Get((int)NomMaterial.Tour));
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
        Musique = GameObject.FindGameObjectWithTag("Musique").GetComponent<Musique>();
        Musique.Niveaux();

        Sauvegarde.Save();
        LoadÉtage();
        étageEnCour = true;
        victoire = false;
    }
    
    static public void LoadÉtage()
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
                    case Plateforme.String:
                        ListGameObject.Last().AddComponent<Plateforme>().InitialisationP(attributs[0],
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
                        ListGameObject.Last().AddComponent<PlateformeMobile>().InitialisationPM(attributs[0],
                                                                                              attributs[1],
                                                                                              attributs[2] * DELTA_HAUTEUR,
                                                                                              attributs[3],
                                                                                              attributs[4] * DELTA_HAUTEUR,
                                                                                              LARGEUR_PLATEFORME - LARGEUR_PLATEFORME / 40,
                                                                                              RAYON_TOUR,
                                                                                              attributs[5],
                                                                                              attributs[6],
                                                                                              attributs.Length >= 8 ? (int)attributs[7] : 0,
                                                                                              attributs.Length >= 9 ? (int)attributs[8] : 0,
                                                                                              Materials.Get((int)NomMaterial.Plateforme));
                        break;
                    case PlateformeTemporaire.String:
                        ListGameObject.Last().AddComponent<PlateformeTemporaire>().InitialisationPT(attributs[0],
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
                        ListGameObject.Last().AddComponent<PlateformePics>().InitialisationPP(attributs[0],
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
                    
                    case Point.String:
                        ListGameObject.Last().AddComponent<Point>().Initialisation(attributs[0], 
                                                                                   attributs[1] * DELTA_HAUTEUR + 0.5f * DELTA_HAUTEUR, 
                                                                                   attributs[2] == 0 ? false : true, attributs[3], 
                                                                                   attributs[2] == 0 ? Materials.Get((int)NomMaterial.Point) : Materials.Get((int)NomMaterial.Multiplicateur));
                        break;
                    case Flèche.String:
                        ListGameObject.Last().AddComponent<Flèche>().Initialisation(attributs[0], 
                                                                                    attributs[1] * DELTA_HAUTEUR, 
                                                                                    RAYON_TOUR, 
                                                                                    attributs[2]);
                        break;

                    case CheckPoint.String:
                        ListGameObject.Last().AddComponent<CheckPoint>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR);
                        break;
                    case FinÉtage.String:
                        ListGameObject.Last().AddComponent<FinÉtage>().Initialisation2(attributs[0],
                                                                                      attributs[1] * DELTA_HAUTEUR);
                        break;
                }

            } while (!étageReader.EndOfStream);
            étageReader.Close();
        }
        else { TourGameObject.AddComponent<ÉtageBoss>(); ; }
        pause = false;
    }

    int cptNaming = 0;
    string NewName(string n)
    {
        return n + ' ' + (cptNaming++).ToString();
    }

    private void Update()
    {
        if (étageFini) { FinirÉtage(); }
        if (nouvelÉtage) { NouvelÉtage(false); }
        if (étageEnCour && (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !victoire) { PausePlay(); }
    }

    static void FinirÉtage() // detruire objet, gestion UI
    {
        Musique.PausePlay();
        foreach (GameObject g in ListGameObject)
        {
            Destroy(g);
        }
        ListGameObject.Clear();
        UiFinÉtage.SetActive(true);
        UiFinÉtageScript.FinÉtage();
        Ui.SetActive(false);
        étageFini = false;
        étageEnCour = false;
    }

    public static void NouvelÉtage(bool mêmeÉtage)
    {
        Musique.PausePlay();
        //UiFinÉtage.GetComponentInChildren<Image>().gameObject.SetActive(false);
        //UiFinÉtage.GetComponentsInChildren<Image>().Where(x => x.name.Contains("Background")).First().enabled = false;
        UiFinÉtage.SetActive(false);
        PersonnageScript.Réinitialiser();
        if(!mêmeÉtage) { nbÉtage++; }
        Sauvegarde.Save();
        LoadÉtage();
        Ui.SetActive(true);
        UiScript.Réinitialiser();
        nouvelÉtage = pause = false;
        étageEnCour = true;
        //PersonnageScript.AudioRecommencer();
    }

    public static void PausePlay()
    {
        pause = !pause;
        Musique.PausePlay();
        Ui.SetActive(!Ui.activeSelf);
        UiFinÉtage.SetActive(!UiFinÉtage.activeSelf);
        if(UiFinÉtageScript != null) { UiFinÉtageScript.DonnéesDeBase(); }
        UiFinÉtage.GetComponentsInChildren<Button>().Where(x => x.name.Contains("Prochain")).First().interactable = false;
        PersonnageGameObject.GetComponent<Rigidbody>().isKinematic = !PersonnageGameObject.GetComponent<Rigidbody>().isKinematic; // a mettre dans personnage?
    }

    public static void Recommencer()
    {
        if (nbÉtage == ÉTAGE_BOSS)
        {
            TourGameObject.GetComponent<ÉtageBoss>().Recommencer();
        }
        else
        {
            FinirÉtage();
            NouvelÉtage(true);
        }
    }

    public static void Checkpoint()
    {
        foreach(GameObject g in ListGameObject.FindAll(x=>x.name.Contains(PlateformeTemporaire.String)))
        {
            g.GetComponent<PlateformeTemporaire>().Réinitialiser();
        }
    }
}
