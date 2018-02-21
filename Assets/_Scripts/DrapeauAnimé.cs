using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrapeauAnimé : MonoBehaviour {

    protected const int NB_TRIANGLES_PAR_TUILE = 2;
    protected const int NB_SOMMETS_PAR_TRIANGLE = 3;

    const float A = 1, S = 1;
    bool initialisé = false;
    protected Vector2 Étendue = new Vector2(1, 1), Charpente = new Vector2(50, 50);

    protected Mesh Maillage;
    protected Vector3[] Sommets;
    protected Vector3 Origine;
    Vector3 DeltaPosition, DeltaTexture;
    protected int NbColonnes, NbLignes, NbSommets, NbTriangles;


    public void Initialisation(Vector3 position)
    {
        transform.position = position;

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<MeshRenderer>().material = Materials.Get((int)NomMaterial.Drapeau);

        Maillage = new Mesh
        {
            name = "Drapeau"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        initialisé = true;
    }



    private void CalculerDonnéesDeBase()
    {
        //Origine = new Vector3((-Étendue.x / 2), (-Étendue.y / 2));
        Origine = transform.position;
        NbColonnes = (int)Charpente.x;
        NbLignes = (int)Charpente.y;

        NbSommets = (NbColonnes + 1) * (NbLignes + 1);
        NbTriangles = NbColonnes * NB_TRIANGLES_PAR_TUILE * NbLignes;

        DeltaPosition = new Vector3(Étendue.x / Charpente.x, Étendue.y / Charpente.y);
        DeltaTexture = new Vector3(1 / (float)NbColonnes, 1 / (float)NbLignes);
    }

    private void GénérerTriangles()
    {
        Maillage = new Mesh();
        GetComponent<MeshFilter>().mesh = Maillage;
        Maillage.name = "Surface";
        GénérerSommets();
        GénérerListeTriangle();
    }

    private void GénérerSommets()
    {
        Sommets = new Vector3[NbSommets];
        Vector2[] CoordonnéesTexture = new Vector2[NbSommets];

        for (int n = 0; n < Sommets.Length; ++n)
        {
            int c = (n % (NbColonnes + 1));
            int l = n / (NbColonnes + 1);

            // Position Sommets 
            Sommets[n] = new Vector3((c * DeltaPosition.x) + Origine.x,
                                     (l * DeltaPosition.y) + Origine.y);
            // Coordonnées Texture
            CoordonnéesTexture[n] = new Vector2(c * DeltaTexture.x,
                                                l * DeltaTexture.y);
        }
        Maillage.vertices = Sommets;
        Maillage.uv = CoordonnéesTexture;
    }

    protected void GénérerListeTriangle()
    {
        int[] triangles = new int[NbTriangles * NB_SOMMETS_PAR_TRIANGLE * 2];

        int cpt = 0;
        int m = triangles.Length / 2;

        for (int c = 0; c < NbColonnes; ++c)
        {
            for (int l = 0; l < NbLignes; ++l)
            {
                int p = l * (NbColonnes + 1) + c; 
            
                triangles[cpt] = triangles[cpt + 6] = p;
                triangles[cpt + 1] = triangles[cpt + 4] = triangles[cpt + 8] = triangles[cpt + 9] = triangles[cpt] + NbColonnes + 1;
                triangles[cpt + 2] = triangles[cpt + 3] = triangles[cpt + 7] = triangles[cpt + 10] = triangles[cpt] + 1;
                triangles[cpt + 5] = triangles[cpt + 11] = triangles[cpt + 1] + 1;

                cpt += NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE*2;
            }
        }
        Maillage.triangles = triangles;
        Maillage.RecalculateNormals();
    }

    void FonctionVent()
    {
        for (int i = 0; i < Sommets.Length; ++i)
        {
            Sommets[i].z = A * ((Sommets[i].x - Origine.x) / Étendue.x) * ((Mathf.Sin(-Sommets[i].x / S + Time.time)));
            Maillage.vertices = Sommets;
        }
    }

	void Update ()
    {
        if (initialisé)
            FonctionVent();
	}
}
