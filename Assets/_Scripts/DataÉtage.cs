using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
//using UnityEditor;

public class DataÉtage : MonoBehaviour
{

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
    [SerializeField] GameObject prefabPersonnage;
    [SerializeField] Material MaterialPlatforme;
    List<GameObject> ListGameObject;

    const float DISTANCE_CAMERA_PERSONNAGE=10; // ratio avec tour 
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;

    public static Vector3 Origine = Vector3.zero;

    StreamReader étageReader;
    StreamWriter saveWriter;


    public static int nbÉtage { get; set; }
    

    private void Awake()
    {
        // for testing
        nbÉtage = TEST_ÉTAGE;
        //---

        Materials.Init();

        ListGameObject = new List<GameObject>();

        // instanciation du placher, personnage, camera
        Plancher = new GameObject("Plancher");
        Plancher.AddComponent<Plateforme>().Initialisation(0, 360, LARGEUR_PLATEFORME, 20, 0, 0, RAYON_TOUR, 0, Materials.Get((int)NomMaterial.Plateforme));
        Tour = new GameObject("Tour");
        Tour.AddComponent<Plateforme>().Initialisation(0, 360, RAYON_TOUR, HAUTEUR_TOUR * DELTA_HAUTEUR, HAUTEUR_TOUR * DELTA_HAUTEUR, 0, 0, 0, Materials.Get((int)NomMaterial.Tour));
        RayonTrajectoirePersonnage = RAYON_TOUR + Plancher.GetComponent<Plateforme>().Largeur / 2;
        RayonCamera = RayonTrajectoirePersonnage + DISTANCE_CAMERA_PERSONNAGE;
        Personnage = Instantiate(prefabPersonnage, new Vector3(RayonTrajectoirePersonnage, prefabPersonnage.transform.lossyScale.y/2, 0), Quaternion.Euler(Vector3.zero));
        Caméra = Camera.main;
        Caméra.gameObject.AddComponent<CameraControlleur>();

        //---test--------------------------------
        //GameObject fusil = new GameObject("fusil");
        //fusil.AddComponent<Fusil>().Initialisation(-1, 0, 0, Materials.Get((int)NomMaterial.Plateforme));
        //---------------------------------------

        LoadÉtage();
    }
    
    private void Start()
    {
        //saveWriter = new StreamWriter(Menu.CHEMIN_SAVE);
        //Save();
    }

    void LoadÉtage()
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
                    ListGameObject.Last().AddComponent<Plateforme>().Initialisation(attributs[0], attributs[1], LARGEUR_PLATEFORME, attributs[2] * DELTA_HAUTEUR, 
                                                                                    attributs[3]*DELTA_HAUTEUR, attributs[4], RAYON_TOUR, attributs[5], 
                                                                                    Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case PlateformeMobile.String:
                    ListGameObject.Last().AddComponent<PlateformeMobile>().Initialisation(attributs[0], attributs[1], LARGEUR_PLATEFORME, attributs[2] * DELTA_HAUTEUR, 
                                                                                          attributs[3] * DELTA_HAUTEUR, attributs[4], RAYON_TOUR, attributs[5], attributs[6], attributs[7],
                                                                                          Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case PlateformeTemporaire.String:
                    ListGameObject.Last().AddComponent<PlateformeTemporaire>().Initialisation(attributs[0], attributs[1], LARGEUR_PLATEFORME, attributs[2] * DELTA_HAUTEUR,
                                                                                              attributs[3] * DELTA_HAUTEUR, attributs[4], RAYON_TOUR, attributs[5],
                                                                                              attributs[6], Materials.Get((int)NomMaterial.Plateforme), attributs.Length >= 8 ? (int)attributs[7] : 0);
                    break;
                case PlateformePics.String:
                    ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(attributs[0], attributs[1], LARGEUR_PLATEFORME, attributs[2] * DELTA_HAUTEUR, 
                                                                                        attributs[3] * DELTA_HAUTEUR,attributs[4], RAYON_TOUR, attributs[5],
                                                                                        attributs[6] * DELTA_HAUTEUR, Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case Pic.String:
                    ListGameObject.Last().AddComponent<Pic>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR, LARGEUR_PLATEFORME / 2.4f,
                                                                             attributs[2] * DELTA_HAUTEUR, Materials.Get((int)NomMaterial.Pic));
                    break;
                case LanceurProjectiles.String:
                    ListGameObject.Last().AddComponent<LanceurProjectiles>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR, 
                                                                                            Materials.Get((int)NomMaterial.Plateforme));
                    break;
                case FinÉtage.String:
                    ListGameObject.Last().AddComponent<FinÉtage>().Initialisation(attributs[0], attributs[1] * DELTA_HAUTEUR);
                    break;
            }

        } while (!étageReader.EndOfStream);
    }

    // quand l'étage à été traverser
    void ÉtageFini()
    {
        nbÉtage++;
        LoadÉtage();
    }


    void Save()
    {
        //saveWriter.Write(nbÉtage);
    }

    int cptNaming = 0;
    string NewName(string n)
    {
        return n + ' ' + (cptNaming++).ToString();
    }

}
