using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    const int NB_SOMMETS_CUBE = 8;
    const int NB_TUILES_CUBE = 6;
    const int NB_TRIANGLES_PAR_TUILE = 2;
    const int NB_SOMMETS_PAR_TRIANGLE = 3;
    const int NB_DEGRÉS_ROTATION_PAR_SECONDE = 180;
    const int VITESSE_TRANSLATION = 4;
    public const string String = "Point";

    bool Multiplicateur;
    float Points;

    Mesh Maillage;
    Vector3[] Sommets;
    float Étendue;
    Vector3 Origine;
    Vector3 Position;
    GameObject ComposanteTexte;
    Text Texte;

    public void Initialisation(float angle, float hauteur, bool multiplicateur, float points, Material material)
    {
        Position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage, hauteur, Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage);
        Multiplicateur = multiplicateur;
        Points = points;

        CalculerDonnéesDeBase();
        Maillage = new Mesh { name = "Point" };
        GénérerTriangle();
        transform.position = Position;

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;

        ComposanteTexte = new GameObject();
        Texte = ComposanteTexte.AddComponent<Text>();
        Texte.text = points.ToString();
        Texte.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        Texte.alignment = TextAnchor.MiddleCenter;
        Texte.color = Color.yellow;
        ComposanteTexte.transform.SetParent(DataÉtage.Ui.transform);
    }

    void CalculerDonnéesDeBase()
    {
        Étendue = (DataÉtage.RayonTrajectoirePersonnage - DataÉtage.RAYON_TOUR) / 2;
        Origine = new Vector3(-Étendue / 2, -Étendue / 2, -Étendue / 2);
    }

    void GénérerTriangle()
    {
        GénérerSommets();
        GénérerCoordonnéesDeTextures();
        GénérerListeTriangle();
    }

    void GénérerSommets()
    {
        Sommets = new Vector3[14];

        Sommets[0] = Sommets[4] = Origine;
        Sommets[1] = new Vector3(Origine.x + Étendue, Origine.y, Origine.z);
        Sommets[2] = Sommets[11] = new Vector3(Origine.x + Étendue, Origine.y, Origine.z + Étendue);
        Sommets[3] = Sommets[10] = new Vector3(Origine.x, Origine.y, Origine.z + Étendue);
        Sommets[5] = Sommets[9] = new Vector3(Origine.x, Origine.y + Étendue, Origine.z);
        Sommets[6] = new Vector3(Origine.x + Étendue, Origine.y + Étendue, Origine.z);
        Sommets[7] = Sommets[13] = new Vector3(Origine.x + Étendue, Origine.y + Étendue, Origine.z + Étendue);
        Sommets[8] = Sommets[12] = new Vector3(Origine.x, Origine.y + Étendue, Origine.z + Étendue);

        Maillage.vertices = Sommets;
    }

    void GénérerCoordonnéesDeTextures()
    {
        Vector2[] CoordonnéesTexture = new Vector2[14];

        CoordonnéesTexture[0] = new Vector2(0, 0);
        CoordonnéesTexture[1] = new Vector2(1, 0);
        CoordonnéesTexture[2] = new Vector2(2, 0);
        CoordonnéesTexture[3] = new Vector2(3, 0);
        CoordonnéesTexture[4] = new Vector2(4, 0);
        CoordonnéesTexture[5] = new Vector2(0, 1);
        CoordonnéesTexture[6] = new Vector2(1, 1);
        CoordonnéesTexture[7] = new Vector2(2, 1);
        CoordonnéesTexture[8] = new Vector2(3, 1);
        CoordonnéesTexture[9] = new Vector2(4, 1);
        CoordonnéesTexture[10] = new Vector2(0, 1);
        CoordonnéesTexture[11] = new Vector2(1, 1);
        CoordonnéesTexture[12] = new Vector2(0, 2);
        CoordonnéesTexture[13] = new Vector2(1, 2);

        Maillage.uv = CoordonnéesTexture;
    }

    void GénérerListeTriangle()
    {
        int[] triangles = new int[NB_TUILES_CUBE * NB_TRIANGLES_PAR_TUILE * NB_SOMMETS_PAR_TRIANGLE];

        triangles[0] = triangles[25] = triangles[28] = 0; //Origine
        triangles[2] = triangles[3] = triangles[6] = triangles[29] = 1;
        triangles[8] = triangles[9] = triangles[12] = 2;
        triangles[14] = triangles[15] = triangles[18] = 3;
        triangles[20] = triangles[21] = 4;
        triangles[1] = triangles[4] = triangles[30] = 5;
        triangles[5] = triangles[7] = triangles[10] = triangles[32] = triangles[33] = 6;
        triangles[11] = triangles[13] = triangles[16] = 7;
        triangles[17] = triangles[19] = triangles[22] = 8;
        triangles[23] = 9;
        triangles[24] = 10;
        triangles[26] = triangles[27] = 11;
        triangles[31] = triangles[34] = 12;
        triangles[35] = 13;

        Maillage.triangles = triangles;
        Maillage.RecalculateNormals();
    }

    void Update()
    {
        if (EstVisible())
        {
            ComposanteTexte.SetActive(true);
            ComposanteTexte.GetComponent<RectTransform>().position = DataÉtage.Caméra.WorldToScreenPoint(gameObject.transform.position);
            Texte.fontSize = (int)(500 / (transform.position - DataÉtage.Caméra.transform.position).magnitude);
        }
        else
        {
            ComposanteTexte.SetActive(false);
        }
        transform.Rotate(new Vector3(0, NB_DEGRÉS_ROTATION_PAR_SECONDE, 0) * Time.deltaTime); 
        transform.position = new Vector3(Position.x, Étendue/3 * Mathf.Sin(Time.time * VITESSE_TRANSLATION) + Position.y, Position.z);
        // Y aurait-il une facon de rendre plus efficace: chaque cube doit faire le calcul à chaque update
    }

    bool EstVisible()
    {
        return !Physics.Raycast(DataÉtage.Caméra.transform.position, transform.position - DataÉtage.Caméra.transform.position, (transform.position - DataÉtage.Caméra.transform.position).magnitude - Étendue);
    }


    private void OnTriggerEnter(Collider other)
    {              
        if (other.gameObject == DataÉtage.Personnage)
        {
            if (Multiplicateur)
            {
                DataÉtage.UiScript.Multiplicateur += Points;
            }
            else
            {
                DataÉtage.UiScript.Points += (int)Points;
            }
            Destroy(gameObject);
            Destroy(ComposanteTexte,3);
            Destroy(this);
        }
    }
}
