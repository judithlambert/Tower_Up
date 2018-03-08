using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
//using UnityEditor;

public class DataÉtage : MonoBehaviour
{
    [SerializeField] bool GODMOD;

    static public bool GodMod;

    [SerializeField] int TEST_ÉTAGE;

    public const float HAUTEUR_TOUR = 35,
                       RAYON_TOUR = 10, // si distance < rayon
                       DELTA_HAUTEUR = 2, //Hauteur entre 2 plateformes
                       LARGEUR_PLATEFORME = 3; //Largeur des objets
    const string CHEMIN_DATA_ÉTAGE= "Assets/Data/";
    const char SÉPARATEUR = ';';

    static public GameObject Personnage, Plancher, Tour;
    static public Camera Caméra;
    public static Personnage PersonnageScript;
    public static Plateforme PlancherScript;
    public static UI UiScript;
    [SerializeField] GameObject prefabPersonnage;
    public static GameObject Ui;
    List<GameObject> ListGameObject;

    const float DISTANCE_CAMERA_PERSONNAGE=10; // ratio avec tour 
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;

    public static Vector3 Origine = Vector3.zero;

    StreamReader étageReader;
    StreamWriter saveWriter;

    static public bool étageFini = false;
    static public bool jeuFini = false;

    public static int nbÉtage { get; set; }
    

    private void Awake()
    {
        // for testing
        nbÉtage = TEST_ÉTAGE;
        //---

        GodMod = GODMOD;

        Materials.Init();

        ListGameObject = new List<GameObject>();

        // instanciation du placher, personnage, camera
        Plancher = new GameObject("Plancher");
        Plancher.AddComponent<Plateforme>().Initialisation(0, 360, 0, 0, 20, LARGEUR_PLATEFORME, RAYON_TOUR,0, Materials.Get((int)NomMaterial.Plateforme));
        Tour = new GameObject("Tour");
        Tour.AddComponent<Plateforme>().Initialisation(0, 360, HAUTEUR_TOUR * DELTA_HAUTEUR, 0, HAUTEUR_TOUR * DELTA_HAUTEUR, RAYON_TOUR,0, 0, Materials.Get((int)NomMaterial.Tour));
        RayonTrajectoirePersonnage = RAYON_TOUR + Plancher.GetComponent<Plateforme>().Largeur / 2;
        RayonCamera = RayonTrajectoirePersonnage + DISTANCE_CAMERA_PERSONNAGE;
        Personnage = Instantiate(prefabPersonnage, new Vector3(RayonTrajectoirePersonnage, prefabPersonnage.transform.lossyScale.y/2, 0), Quaternion.Euler(Vector3.zero));
        PersonnageScript = Personnage.GetComponent<Personnage>();
        Ui = GameObject.FindGameObjectWithTag("UI");
        UiScript = Ui.GetComponent<UI>();
        Caméra = Camera.main;
        Caméra.gameObject.AddComponent<CameraControlleur>();

        //---test--------------------------------
        //GameObject fusil = new GameObject("fusil");
        //fusil.AddComponent<Fusil>().Initialisation(-1, 0, 0, Materials.Get((int)NomMaterial.Plateforme));
        //---------------------------------------

        LoadÉtage();


        //do //meuhhhhh
        //{
        //    if (étageFini)
        //    {
        //        ++nbÉtage;
        //        Save();
        //        //personnageScript.DébutÉtage();
        //        ListGameObject.Clear();
        //        étageFini = false;
        //        LoadÉtage();
        //    }
        //} while (!jeuFini);
    }
    
    private void Start()
    {
        saveWriter = new StreamWriter(Menu.CHEMIN_SAVE);
        Save();
    }

    public void LoadÉtage()
    {       
        étageReader = new StreamReader(CHEMIN_DATA_ÉTAGE + "Étage" + nbÉtage.ToString() + ".txt");

        do
        {
            string obj = étageReader.ReadLine();
            string[] line = étageReader.ReadLine().Split(SÉPARATEUR);
            float[] attributs = new float[line.Length];
            for (int cpt = 0; cpt < line.Length; ++cpt)
            {
                attributs[cpt] = float.Parse(line[cpt]);
            }

            // if(!obj.Contains(' ')) { obj = NewName(obj); }
            ListGameObject.Add(new GameObject(obj));

            obj = obj.Split(' ')[0]; // obj.Remove(' ');
            switch (obj)
            {
                case Plateforme.String:
                    ListGameObject.Last().AddComponent<Plateforme>().Initialisation(Maths.GestionAngle(attributs[0]), 
                                                                                    attributs[1], 
                                                                                    attributs[2] * DELTA_HAUTEUR,
                                                                                    attributs[3], 
                                                                                    attributs[4] * DELTA_HAUTEUR, 
                                                                                    LARGEUR_PLATEFORME, 
                                                                                    RAYON_TOUR, 
                                                                                    attributs.Length >= 6 ? (int)attributs[5] : 0,
                                                                                    Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case PlateformeMobile.String:
                    ListGameObject.Last().AddComponent<PlateformeMobile>().Initialisation(Maths.GestionAngle(attributs[0]), 
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
                    ListGameObject.Last().AddComponent<PlateformeTemporaire>().Initialisation(Maths.GestionAngle(attributs[0]), 
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
                    ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(Maths.GestionAngle(attributs[0]), 
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
                    ListGameObject.Last().AddComponent<Pic>().Initialisation(Maths.GestionAngle(attributs[0]), 
                                                                             attributs[1] * DELTA_HAUTEUR, 
                                                                             attributs[2] * DELTA_HAUTEUR, 
                                                                             LARGEUR_PLATEFORME / 2.4f,
                                                                             Materials.Get((int)NomMaterial.Pic));
                    break;
                case LanceurProjectiles.String:
                    ListGameObject.Last().AddComponent<LanceurProjectiles>().Initialisation(Maths.GestionAngle(attributs[0]), 
                                                                                            attributs[1] * DELTA_HAUTEUR, 
                                                                                            Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case FinÉtage.String:
                    ListGameObject.Last().AddComponent<FinÉtage>().Initialisation(Maths.GestionAngle(attributs[0]), 
                                                                                  attributs[1] * DELTA_HAUTEUR);
                    break;
                case Point.String:
                    ListGameObject.Last().AddComponent<Point>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR + 0.5f * DELTA_HAUTEUR, attributs[2] == 0 ? false : true, attributs[3], attributs[2] == 0 ? Materials.Get((int)NomMaterial.Point) : Materials.Get((int)NomMaterial.Multiplicateur));
                    break;
            }

        } while (!étageReader.EndOfStream);
    }

    //static void ÉtageFini()
    //{
    //    nbÉtage++;
    //    é
    //}


    void Save()
    {
        saveWriter.Write(nbÉtage);
    }

    int cptNaming = 0;
    string NewName(string n)
    {
        return n + ' ' + (cptNaming++).ToString();
    }

}
