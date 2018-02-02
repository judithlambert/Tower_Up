using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class DataÉtage : MonoBehaviour
{
    const string CHEMIN_DATA_ÉTAGE= "Assets/Data/";
    static public GameObject Personnage, Plancher;
    static public Camera Caméra;
    public static Personnage PersonnageScript;
    public static Platforme PlancherScript;
    //[SerializeField] public static int RAYON_TOUR, LARGUEUR_PLATFORM, DISTANCE_PERSONNAGE_CAMERA;
    public static float RayonTour; // si distance < rayon
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;
    public static Vector3 Origine;
    const float DISTANCE_CAMERA_PERSONNAGE=10; // ratio avec tour

    [SerializeField] GameObject prefabPersonnage;
    [SerializeField] Material MaterialPlatforme;
    public static int nbÉtage { get; set; }
    StreamReader étageReader;
    StreamWriter saveWriter;

    const char SÉPARATEUR = ';';

    int cptNaming=0;

    List<GameObject> ListGameObject;

    float largeurPlatforme;


    private void Awake()
    {
        // for testing
        nbÉtage = 1;
        //---
        cptNaming = 0;

        Origine = Vector3.zero;

        ListGameObject = new List<GameObject>();

        Caméra = Camera.main;

        RayonTour = gameObject.transform.lossyScale.x/2;

        largeurPlatforme = 5;

        Plancher = new GameObject("Plancher");
        Plancher.AddComponent<Platforme>().Initialisation(0, 360, largeurPlatforme, 1, 0, RayonTour, 0, MaterialPlatforme);

        RayonTrajectoirePersonnage = RayonTour + Plancher.GetComponent<Platforme>().Largeur / 2;
        RayonCamera = RayonTrajectoirePersonnage + DISTANCE_CAMERA_PERSONNAGE;

        Personnage = Instantiate(prefabPersonnage, new Vector3(0, 0.5f, 3), Quaternion.Euler(Vector3.zero));

        Caméra.gameObject.AddComponent<CameraControlleur>();


        GameObject cubeLanceur = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeLanceur.AddComponent<LanceurProjectiles>();


        LoadÉtage();

    }



    private void Start()
    {
        étageReader = new StreamReader(CHEMIN_DATA_ÉTAGE + NuméroÉtageToString(nbÉtage)+".txt");
        //saveWriter = new StreamWriter(Menu.CHEMIN_SAVE);
        //Save();

    }

    void LoadÉtage()
    {
        étageReader = new StreamReader(CHEMIN_DATA_ÉTAGE + NuméroÉtageToString(nbÉtage)+".txt");

        do
        {
            string obj = étageReader.ReadLine();
            string[] line = étageReader.ReadLine().Split(SÉPARATEUR);
            float[] attributs = new float[line.Length];
            for (int cpt = 0; cpt < line.Length; ++cpt)
            {
                attributs[cpt] = float.Parse(line[cpt]);
            }
            // quoi faire pour material

            ListGameObject.Add(new GameObject(NewName(obj)));


            switch (obj)
            {
                case "Platforme":
                    ListGameObject.Last().AddComponent<Platforme>().Initialisation(attributs[0], attributs[1], largeurPlatforme, attributs[2], attributs[3], RayonTour, attributs[4], MaterialPlatforme);
                    break;
                case "PlatformeMobile":
                    ListGameObject.Last().AddComponent<PlatformeMobile>().Initialisation(attributs[0], attributs[1], largeurPlatforme, attributs[2], attributs[3], RayonTour, attributs[4], MaterialPlatforme);
                    break;
                case "PlatformeTemporaire":
                    ListGameObject.Last().AddComponent<PlatformeTemporaire>().Initialisation(attributs[0], attributs[1], largeurPlatforme, attributs[2], attributs[3], RayonTour, attributs[4], attributs[5], MaterialPlatforme);
                    break;
                case "ObstaclePic":
                    ListGameObject.Last().AddComponent<ObstaclePic>().Initialisation(attributs[0], attributs[1], largeurPlatforme, attributs[2], attributs[3], RayonTour, attributs[4], attributs[5], MaterialPlatforme);
                    break;

            }

        } while (!étageReader.EndOfStream);
    }

    void ÉtageFini()
    {
        nbÉtage++;
        Start();
    }


    void Save()
    {
        //saveWriter.Write(nbÉtage);
    }
    string NuméroÉtageToString(int num)
    {
        return "Étage" + num.ToString();
    }


    string NewName(string n)
    {
        return n + (cptNaming++).ToString();
    }

}
