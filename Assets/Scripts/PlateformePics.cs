using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformePics : Plateforme {

    protected float HauteurPic;

    //public void Awake()
    //{
    //    AngleDébut = 0;
    //    Amplitude = 90;
    //    AngleDébut = 0;
    //    Amplitude = 90;
    //    Largeur = 3;
    //    Épaisseur = 1;
    //    Hauteur = 1;
    //    Inclinaison = 0;
    //    Rayon = 5;
    //    Rugosité = 0;
    //    CalculerDonnéesDeBase();
    //    GénérerTriangles();
    //}

    public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float rayon, float rugosité, float hauteurPic, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Rayon = rayon;
        Rugosité = rugosité;

        HauteurPic = hauteurPic;

        CréationObject(material);

        //Maillage = new Mesh
        //{
        //    name = "ObstaclePic"
        //};

        //CalculerDonnéesDeBase();
        //GénérerTriangles();

        //gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        ////gameObject.AddComponent<Rigidbody>().useGravity = false;
        //gameObject.AddComponent<MeshRenderer>().material = material;
        //gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        //GetComponent<MeshCollider>().convex = true;
        //GetComponent<MeshCollider>().isTrigger = true;
    }

    public void OnTriggerEnter(Collider other) // faire que seulement personnage trigger
    {
        DataÉtage.Personnage.GetComponent<Personnage>().Die();
    }

    override protected void CalculerDonnéesDeBase()
    {
        Origine = transform.position;
        nbTranches = (int)(Amplitude / 15);
        AngleDébut = DegréEnRadian(AngleDébut);
        Amplitude = DegréEnRadian(Amplitude);
        nbSommets = (nbTranches + 1) * 5 + NB_SOMMETS_BOUTS + nbTranches;
        nbTriangles = (nbTranches * 3 + NB_DE_BOUT) * NB_TRIANGLES_PAR_TUILE + nbTranches*4;
        DeltaAngle = Amplitude / nbTranches;
        DeltaTexture = DeltaAngle / DegréEnRadian(NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR);
    }

    override protected void GénérerTriangles()
    {
        GénérerSommets();
        GénérerCoordonnéesDeTextures();
        GénérerListeTriangles();
    }
    Vector3 SommetPic(float angleAjouté, float élévationAjouté)
    {
        return new Vector3(Origine.x + Mathf.Cos(AngleDébut + angleAjouté) * (Rayon + (Largeur/2)),
                           Origine.y + Hauteur+ élévationAjouté,
                           Origine.z + Mathf.Sin(AngleDébut + angleAjouté) * (Rayon + (Largeur/2)));
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
            Sommets[(nbTranches + 1) + n]                    = Sommet(angleAjouté, élévationAjouté, false, false);
            Sommets[(nbTranches + 1) * 2 + n]                = Sommet(angleAjouté, élévationAjouté, true, false);
            Sommets[(nbTranches + 1) * 3 + n]                = Sommet(angleAjouté, élévationAjouté, true, true);

            //  Sommets des pointes des pics
            Sommets[nbSommets-NB_SOMMETS_BOUTS+(n-nbTranches)] = SommetPic(angleAjouté + DeltaAngle/2, HauteurPic+Épaisseur);
        }

        // Sommets des deux bouts
        Sommets[nbSommets - 8] = Sommets[(nbTranches + 1) * 2];
        Sommets[nbSommets - 7] = Sommets[(nbTranches + 1) * 3];
        Sommets[nbSommets - 6] = Sommets[nbTranches + 1];
        Sommets[nbSommets - 5] = Sommets[0];
        Sommets[nbSommets - 4] = Sommets[(nbTranches + 1) * 3 - 1];
        Sommets[nbSommets - 3] = Sommets[(nbTranches + 1) * 4 - 1];
        Sommets[nbSommets - 2] = Sommets[(nbTranches + 1) * 2 - 1];
        Sommets[nbSommets - 1] = Sommets[nbTranches];

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
            CoordonnéesTexture[n] = new Vector2((n % (nbTranches + 1)) * DeltaTexture, nièmeArrête % 2 == 0 ? nièmeArrête / 2 * (1 + ratio) : (nièmeArrête - 1) / 2 * (1 + ratio) + 1);
        }

        for(int n = nbSommets - NB_SOMMETS_BOUTS - nbTranches; n < nbSommets - NB_SOMMETS_BOUTS; n++)
        {
            int t = n- (nbSommets - NB_SOMMETS_BOUTS - nbTranches);
            CoordonnéesTexture[n] = new Vector2(t+0.5f, 0.5f);
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
}
