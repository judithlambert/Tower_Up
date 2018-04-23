using System.Collections.Generic;
using UnityEngine;

public class SphèreTexturée : MonoBehaviour
{
    const float LATITUDE_MIN = -Mathf.PI / 2;
    const float LATITUDE_MAX = Mathf.PI / 2;
    const float LONGITUDE_MIN = -Mathf.PI; 
    const float LONGITUDE_MAX = Mathf.PI; 
    const int NB_TRIANGLES_PAR_TUILE = 2;
    const int NB_SOMMETS_PAR_TRIANGLE = 3;
    const int VITESSE_TRANSLATION = 8; 
    const float DISTANTCE_POUR_ÊTRE_ARIVÉE_À_UN_POINT = 0.5f;

    [SerializeField] float Rayon;

    [SerializeField] Vector2 Charpente;

    protected Mesh Maillage;
    protected Vector3[] Sommets;
    protected Vector3 OrigineMaillage;
    Vector3 DeltaPosition, DeltaTexture;
    int NbColonneLongitude, NbLignesLatitude, NbSommets, NbTriangles;

    void Awake()
    {
        CalculerDonnéesDeBase();
        GénérerTriangle();
    }

    //--- MAILLAIGE ----------------------------------------------------------------------------------

    void CalculerDonnéesDeBase()
    {
        OrigineMaillage = Vector3.zero;
        NbColonneLongitude = (int)Charpente.x;
        NbLignesLatitude = (int)Charpente.y;

        NbSommets = (NbColonneLongitude + 1) * (NbLignesLatitude + 1);
        NbTriangles = NbColonneLongitude * NB_TRIANGLES_PAR_TUILE * NbLignesLatitude;

        DeltaPosition = new Vector3((LONGITUDE_MAX - LONGITUDE_MIN) / Charpente.x, (LATITUDE_MAX - LATITUDE_MIN) / Charpente.y);
        DeltaTexture = new Vector3(1 / (float)NbColonneLongitude, 1 / (float)NbLignesLatitude);
    }

    void GénérerTriangle()
    {
        Maillage = new Mesh();
        GetComponent<MeshFilter>().mesh = Maillage;
        Maillage.name = "Sphère";
        GénérerSommets();
        GénérerListeTriangle();
    }

    void GénérerSommets()
    {
        Sommets = new Vector3[NbSommets];
        Vector2[] CoordonnéesTexture = new Vector2[NbSommets];

        for (int n = 0; n < Sommets.Length; ++n)
        {
            int longitude = n % (NbColonneLongitude + 1);
            int latitude  = n / (NbColonneLongitude + 1);

            // Position Sommets 
            //Sommets[n] = new Vector3(Rayon * Mathf.Sin(latitude * DeltaPosition.y) * Mathf.Cos(longitude * DeltaPosition.x),
            //                         Rayon * Mathf.Cos(latitude * DeltaPosition.y),
            //                         Rayon * Mathf.Sin(latitude * DeltaPosition.y) * Mathf.Sin(longitude * DeltaPosition.x));
            Sommets[n] = new Vector3(Rayon * Mathf.Cos(latitude * DeltaPosition.y) * Mathf.Cos(longitude * DeltaPosition.x),
                                     Rayon * Mathf.Cos(latitude * DeltaPosition.y) * Mathf.Sin(longitude * DeltaPosition.y),
                                     Rayon * Mathf.Sin(longitude * DeltaPosition.x));

            // Coordonnées Texture
            CoordonnéesTexture[CoordonnéesTexture.Length-n-1] = new Vector2(longitude * DeltaTexture.x,
                                                                            latitude * DeltaTexture.y);
        }
        Maillage.vertices = Sommets;
        Maillage.uv = CoordonnéesTexture;
    }

    void GénérerListeTriangle()
    {
        int[] triangles = new int[NbTriangles * NB_SOMMETS_PAR_TRIANGLE];

        int cpt = 0;
        for (int c = 0; c < NbColonneLongitude; ++c)
        {
            for (int l = 0; l < NbLignesLatitude; ++l)
            {
                triangles[cpt] = l * (NbColonneLongitude + 1) + c; // point de départ
                triangles[cpt + 1] = triangles[cpt + 4] = triangles[cpt] + 1;
                triangles[cpt + 2] = triangles[cpt + 3] = triangles[cpt] + NbColonneLongitude + 1;
                triangles[cpt + 5] = triangles[cpt + 2] + 1;

                cpt += NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE;
            }
        }
        Maillage.triangles = triangles;
        Maillage.RecalculateNormals();
    }


    //--- ANIMATION ----------------------------------------------------------------------------------------------------------------------------

    bool play;
    const float A = 2,
                B = 2.5f * A,
                S = 10,
                T = 2 * S;
    float time;
    //void Start()
    //{
    //    play = true;
    //}


    /*
    * x = r * cos(latitude) * cos(longitude)
    * y = r * cos(latitude) * cos(longitude)
    * z = r * sin(latitude)
    * 
    * latitude : [-π/2, π/2]
    * longitude : [-π, π]
    */

    void FonctionVague()
    {
        for (int i = 0; i < Sommets.Length; ++i)
        {
            float angleX, angleY, angleZ;
            angleX = Mathf.Atan2(Sommets[i].z, Sommets[i].y);
            angleY = Mathf.Atan2(Sommets[i].z, Sommets[i].x);
            angleZ = Mathf.Atan2(Sommets[i].y, Sommets[i].x);

            //Sommets[i].z = A * ((Mathf.Sin(Sommets[i].x / S + time) + Mathf.Sin(Sommets[i].y / S + time))) 
            //             + B * ((Mathf.Sin(Sommets[i].x / T + time) + Mathf.Sin(Sommets[i].y / T + time)));
        }
        Maillage.vertices = Sommets;
        time += Time.deltaTime;
    }

    void Update()
    {
        FonctionVague();
    }
}
