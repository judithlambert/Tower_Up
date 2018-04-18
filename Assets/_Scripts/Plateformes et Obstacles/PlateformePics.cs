using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

// bug avec la position

public class PlateformePics : Plateforme
{
    public const string String = Plateforme.String + "Pics";

    Vector3 sommetPic;
    public Vector3 SommetPic
    {
        get
        {
           return new Vector3(Mathf.Cos(Mathf.Deg2Rad * (AngleDébut + Amplitude / 2)) * (Rayon + Largeur / 2),
                              Hauteur + HauteurPic,
                              Mathf.Sin(Mathf.Deg2Rad * (AngleDébut + Amplitude / 2)) * (Rayon + Largeur / 2));
        }
    }

    protected float HauteurPic;

    float PositionDessus, PositionDessous, PositionPics;
    Vector3 SommetDroiteHautSuppérieur, SommetDroiteHautInférieur, SommetDroiteBasSuppérieur, SommetDroiteBasInférieur, SommetGaucheHautSuppérieur, SommetGaucheHautInférieur, SommetGaucheBasSuppérieur, SommetGaucheBasInférieur;


    public void Initialisation(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float hauteurPic, float rotation, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        Rotation = rotation;

        HauteurPic = hauteurPic;

        //CréationObject(material);

        Maillage = new Mesh
        {
            name = "PlateformePics"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        //GetComponent<MeshCollider>().convex = true;         // <-- crée un mesh colider qui ne fit pas avec le mesh réel
        //GetComponent<MeshCollider>().isTrigger = true;
        //GetComponent<Rigidbody>().isKinematic = true;

        Positionnement();
        CréationPointCollision();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }


    public void CréationPointCollision()
    {

        PositionDessus = Hauteur;
        PositionDessous = Hauteur - Épaisseur;
        PositionPics = Hauteur + HauteurPic;
        if (Rotation == 180)
        {
            PositionDessus = Hauteur - Épaisseur;
            PositionDessous = Hauteur;
            PositionPics = Hauteur - Épaisseur - HauteurPic;
        }
    }



    // MAILLAGE
    override protected void CalculerDonnéesDeBase()
    {
        Origine = transform.position;
        nbTranches = (int)Mathf.Ceil((Mathf.Deg2Rad * Amplitude) * DataÉtage.RayonTrajectoirePersonnage / DataÉtage.LARGEUR_PLATEFORME);
        //AngleDébut = Maths.DegréEnRadian(AngleDébut);
        //Amplitude = Maths.DegréEnRadian(Amplitude);
        nbSommets = (nbTranches + 1) * 5 + NB_SOMMETS_BOUTS + nbTranches;
        nbTriangles = (nbTranches * 3 + NB_DE_BOUT) * NB_TRIANGLES_PAR_TUILE + nbTranches * 4;
        DeltaAngle = (Mathf.Deg2Rad * Amplitude) / nbTranches;
        DeltaTexture = DeltaAngle / (Mathf.Deg2Rad * NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR);
    }

    override protected void GénérerTriangles()
    {
        GénérerSommets();
        GénérerCoordonnéesDeTextures();
        GénérerListeTriangles();
    }

    Vector3 SommetPointePic(float angleAjouté, float hauteurAjouté, float inclinaisonAjouté)
    {
        return new Vector3(Mathf.Cos(angleAjouté) * (Rayon + (Largeur / 2)),
                           inclinaisonAjouté + hauteurAjouté,
                           Mathf.Sin(angleAjouté) * (Rayon + (Largeur / 2)));
    }

    protected override void GénérerSommets()
    {
        Sommets = new Vector3[nbSommets];

        // Sommets du corps
        for (int n = 0; n < nbTranches + 1; ++n)
        {
            float angleAjouté = DeltaAngle * n;
            float élévationAjouté = DeltaÉlévation * n;
            Sommets[n] = Sommets[n + ((nbTranches + 1) * 4)] = Sommet(angleAjouté, élévationAjouté, false, true);
            Sommets[(nbTranches + 1) + n] = Sommet(angleAjouté, élévationAjouté, false, false);
            Sommets[(nbTranches + 1) * 2 + n] = Sommet(angleAjouté, élévationAjouté, true, false);
            Sommets[(nbTranches + 1) * 3 + n] = Sommet(angleAjouté, élévationAjouté, true, true);

            //  Sommets des pointes des pics
            Sommets[nbSommets - NB_SOMMETS_BOUTS + (n - nbTranches)] = SommetPointePic(angleAjouté + DeltaAngle / 2, HauteurPic, élévationAjouté);
        }

        // Sommets des extrémités
        Sommets[nbSommets - 8] = Sommets[(nbTranches + 1) * 2]; // gauche bas inférieur
        Sommets[nbSommets - 7] = Sommets[(nbTranches + 1) * 3]; // gauche bas suppérieur
        Sommets[nbSommets - 6] = Sommets[nbTranches + 1];       // gauche haut inférieur
        Sommets[nbSommets - 5] = Sommets[0];                    // gauche haut suppérieur

        Sommets[nbSommets - 4] = Sommets[(nbTranches + 1) * 3 - 1]; // droit bas inférieur
        Sommets[nbSommets - 3] = Sommets[(nbTranches + 1) * 4 - 1]; // droit bas suppérieur
        Sommets[nbSommets - 2] = Sommets[(nbTranches + 1) * 2 - 1]; // droit haut inférieur
        Sommets[nbSommets - 1] = Sommets[nbTranches];               // droit haut suppérieur

        Maillage.vertices = Sommets;
    }

    protected override void GénérerCoordonnéesDeTextures()
    {
        Vector2[] CoordonnéesTexture = new Vector2[nbSommets];
        float ratio = Épaisseur / Largeur;

        // Coordonnées de texture du corps
        for (int n = 0; n < (nbTranches + 1) * 5; ++n)
        {
            int nièmeArrête = (n - n % (nbTranches + 1)) / (nbTranches + 1);
            CoordonnéesTexture[n] = new Vector2((n % (nbTranches + 1)) * DeltaTexture,
                                                 nièmeArrête % 2 == 0 ? nièmeArrête / 2 * (1 + ratio) : (nièmeArrête - 1) / 2 * (1 + ratio) + 1);
        }

        // Coordonnées de textures des pointes
        for (int n = nbSommets - NB_SOMMETS_BOUTS - nbTranches; n < nbSommets - NB_SOMMETS_BOUTS; n++)
        {
            CoordonnéesTexture[n] = new Vector2((n % (nbTranches + 1)) * DeltaTexture + DeltaTexture / 2, 0.5f);
        }

        // Coordonnées de textures des deux bouts
        CoordonnéesTexture[nbSommets - 8] = CoordonnéesTexture[nbSommets - 4] = new Vector2(0, 0);
        CoordonnéesTexture[nbSommets - 7] = CoordonnéesTexture[nbSommets - 3] = new Vector2(1, 0);
        CoordonnéesTexture[nbSommets - 6] = CoordonnéesTexture[nbSommets - 2] = new Vector2(0, ratio);
        CoordonnéesTexture[nbSommets - 5] = CoordonnéesTexture[nbSommets - 1] = new Vector2(1, ratio);

        Maillage.uv = CoordonnéesTexture;
    }

    protected override void GénérerListeTriangles()
    {
        int[] Triangles = new int[nbTriangles * NB_SOMMETS_PAR_TRIANGLE];

        // Triangles du corps
        int cpt = 0;
        for (int t = 0; t < nbTranches; ++t)
        {
            for (int f = 0; f < 4; ++f)
            {
                if (f == 0)
                {
                    Triangles[cpt] = f * (nbTranches + 1) + t;
                    Triangles[cpt + 1] = Triangles[cpt] + nbTranches + 1;

                    Triangles[cpt + 3] = Triangles[cpt] + nbTranches + 1;
                    Triangles[cpt + 4] = Triangles[cpt + 1] + 1;

                    Triangles[cpt + 6] = Triangles[cpt + 1] + 1;
                    Triangles[cpt + 7] = Triangles[cpt] + 1;

                    Triangles[cpt + 9] = Triangles[cpt] + 1;
                    Triangles[cpt + 10] = f * (nbTranches + 1) + t;

                    Triangles[cpt + 2] = Triangles[cpt + 5] = Triangles[cpt + 8] = Triangles[cpt + 11] = nbSommets - NB_SOMMETS_BOUTS + (t - nbTranches);
                    cpt += NB_SOMMETS_PAR_TRIANGLE * 4;
                }
                else
                {
                    Triangles[cpt] = f * (nbTranches + 1) + t;
                    Triangles[cpt + 1] = Triangles[cpt + 4] = Triangles[cpt] + nbTranches + 1;
                    Triangles[cpt + 2] = Triangles[cpt + 3] = Triangles[cpt] + 1;
                    Triangles[cpt + 5] = Triangles[cpt + 1] + 1;

                    cpt += NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE;
                }
            }
        }

        // Triangles des deux bouts
        int p = Triangles.Length - NB_SOMMETS_PAR_TRIANGLE * 4;
        Triangles[p] = nbSommets - 8;
        Triangles[p + 2] = Triangles[p + 3] = nbSommets - 7;
        Triangles[p + 1] = Triangles[p + 4] = nbSommets - 6;
        Triangles[p + 5] = nbSommets - 5;
        Triangles[p + 6] = nbSommets - 4;
        Triangles[p + 7] = Triangles[p + 9] = nbSommets - 3;
        Triangles[p + 8] = Triangles[p + 11] = nbSommets - 2;
        Triangles[p + 10] = nbSommets - 1;

        Maillage.triangles = Triangles;
        Maillage.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Personnage") && CollisionDessusAvecPics(collision))
        {
            Debug.Log("piquer");
            DataÉtage.PersonnageGameObject.GetComponent<Personnage>().Dommage(1, collision);
        }
    }

    public bool CollisionDessusAvecPics(Collision collision)
    {
        bool estAuPic = false;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (Maths.EstDansLeRange(cp.point.y, PositionDessus, PositionPics)) { estAuPic = true; }
        }
        return estAuPic;
    }
}
