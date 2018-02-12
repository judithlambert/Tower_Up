using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

// class pour les material

public class DataÉtage : MonoBehaviour
{
    const string CHEMIN_DATA_ÉTAGE= "Assets/Data/";
    const char SÉPARATEUR = ';';

    static public GameObject Personnage, Plancher;
    static public Camera Caméra;
    public static Personnage PersonnageScript;
    public static Plateforme PlancherScript;
    [SerializeField] GameObject prefabPersonnage;
    [SerializeField] Material MaterialPlatforme;
    List<GameObject> ListGameObject;

    const float DISTANCE_CAMERA_PERSONNAGE=10; // ratio avec tour
    public static float RayonTour; // si distance < rayon
    public static float RayonTrajectoirePersonnage;
    public static float RayonCamera;
    public static float DeltaHauteur = 2; //Hauteur entre 2 plateformes
    public static float LargeurPlatforme = 3; //Largeur des objets

    public static Vector3 Origine = Vector3.zero;

    StreamReader étageReader;
    StreamWriter saveWriter;

    public static int nbÉtage { get; set; }
    

    private void Awake()
    {
        // for testing
        nbÉtage = 1;
        //---

        ListGameObject = new List<GameObject>();

        // instanciation du placher, personnage, camera
        RayonTour = gameObject.transform.lossyScale.x/2;
        Plancher = new GameObject("Plancher");
        Plancher.AddComponent<Plateforme>().Initialisation(0, 360, LargeurPlatforme, 1, 0, 0, RayonTour, 0,MaterialPlatforme);
        RayonTrajectoirePersonnage = RayonTour + Plancher.GetComponent<Plateforme>().Largeur / 2;
        RayonCamera = RayonTrajectoirePersonnage + DISTANCE_CAMERA_PERSONNAGE;
        Personnage = Instantiate(prefabPersonnage, new Vector3(RayonTrajectoirePersonnage, prefabPersonnage.transform.lossyScale.y/2, 0), Quaternion.Euler(Vector3.zero));
        Caméra = Camera.main;
        Caméra.gameObject.AddComponent<CameraControlleur>();


        //---test--------------------------------
        //GameObject cubeLanceur = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cubeLanceur.AddComponent<LanceurProjectiles>();

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

            ListGameObject.Add(new GameObject(NewName(obj)));
            switch (obj)
            {
                case "Plateforme":
                    ListGameObject.Last().AddComponent<Plateforme>().Initialisation(attributs[0], attributs[1], LargeurPlatforme, attributs[2], attributs[3]*DeltaHauteur, attributs[4], RayonTour, attributs[5], MaterialPlatforme);
                    break;
                case "PlateformeMobile":
                    ListGameObject.Last().AddComponent<PlateformeMobile>().Initialisation(attributs[0], attributs[1], LargeurPlatforme, attributs[2], attributs[3] * DeltaHauteur, attributs[4], RayonTour, attributs[5], MaterialPlatforme);
                    break;
                case "PlateformeTemporaire":
                    ListGameObject.Last().AddComponent<PlateformeTemporaire>().Initialisation(attributs[0], attributs[1], LargeurPlatforme, attributs[2], attributs[3] * DeltaHauteur, attributs[4], RayonTour, attributs[5], attributs[6], MaterialPlatforme);
                    break;
                case "PlateformePics":
                    ListGameObject.Last().AddComponent<PlateformePics>().Initialisation(attributs[0], attributs[1], LargeurPlatforme, attributs[2], attributs[3] * DeltaHauteur,attributs[4], RayonTour, attributs[5], MaterialPlatforme);
                    break;
                case "Pic":
                    ListGameObject.Last().AddComponent<Pic>().Initialisation(attributs[0], attributs[1] * DeltaHauteur, attributs[2], LargeurPlatforme/2 ,attributs[3]*DeltaHauteur, MaterialPlatforme);
                    break;
                case "LanceurProjecteurs":
                    ListGameObject.Last().AddComponent<LanceurProjectiles>().Initialisation(attributs[0], attributs[1] * DeltaHauteur, attributs[2], MaterialPlatforme);
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
        return n + (cptNaming++).ToString();
    }

}
