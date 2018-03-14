using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Plateforme : MonoBehaviour
{
    // transform.position 
    // si position non a (0,y,0), rotate marche pas
    // transform.rotation
   
    public const string String = "Plateforme";

    public const float INCERTITUDE_COLLISION = 0.1f;

    protected const int NB_TRIANGLES_PAR_TUILE = 2, NB_SOMMETS_PAR_TRIANGLE = 3, NB_TUILES_PAR_CERCLE_COMPLET = 72, NB_SOMMETS_BOUTS = 8, NB_DE_BOUT = 2;
    protected const float NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR = 45;

    //protected float AngleDébut, Amplitude, Épaisseur, Largeur, Rayon, Élévation, Rugosité;
    public float AngleDébut { get; protected set; }
    public float Amplitude { get; protected set; }
    public float Épaisseur { get; protected set; }
    public float Largeur { get; protected set; }
    public float Rayon { get; protected set; }
    public float Hauteur { get; protected set; }
    public float Inclinaison { get; protected set; }
    public float Rotation { get; protected set; }

    protected Vector3 Origine; // est toujours égal a DataÉtage.Origine (Vector3.zero)
    protected Mesh Maillage;
    protected Vector3[] Sommets;
    protected float DeltaAngle, DeltaTexture, DeltaÉlévation;
    protected int nbTranches, nbSommets, nbTriangles;

    Vector3 SommetDroiteHautSuppérieur, SommetDroiteHautInférieur, SommetDroiteBasSuppérieur, SommetDroiteBasInférieur, SommetGaucheHautSuppérieur, SommetGaucheHautInférieur, SommetGaucheBasSuppérieur, SommetGaucheBasInférieur;
    float PositionDessus, PositionDessous;
    Vector3 PositionCôtéDébut, PositionCôtéFin;


    public void Initialisation(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float rotation, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        Rotation = rotation;

        PositionDessus = Hauteur;
        PositionDessous = Hauteur - Épaisseur;
        if(rotation==180)
        {
            PositionDessus = Hauteur - Épaisseur;
            PositionDessous = Hauteur;
        }


        CréationObject(material);
        Positionnement();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Vector3 PositionRelativeAuMonde = new Vector3((float)Mathf.Cos(Maths.DegréEnRadian(AngleDébut)),
                                                      Hauteur,
                                                      (float)Mathf.Sin(Maths.DegréEnRadian(AngleDébut)));


        SommetGaucheBasInférieur = Sommets[nbSommets - 8] + PositionRelativeAuMonde; 
        SommetGaucheBasSuppérieur = Sommets[nbSommets - 7] + PositionRelativeAuMonde;
        SommetGaucheHautInférieur = Sommets[nbSommets - 6] + PositionRelativeAuMonde;
        SommetGaucheHautSuppérieur = Sommets[nbSommets - 5] + PositionRelativeAuMonde;
        SommetDroiteBasInférieur = Sommets[nbSommets - 4] + PositionRelativeAuMonde;
        SommetDroiteBasSuppérieur = Sommets[nbSommets - 3] + PositionRelativeAuMonde;
        SommetDroiteHautInférieur = Sommets[nbSommets - 2] + PositionRelativeAuMonde;
        SommetDroiteHautSuppérieur = Sommets[nbSommets - 1] + PositionRelativeAuMonde;


        Debug.Log("SommetGaucheBasInférieur: (" + SommetGaucheBasInférieur.x + ", " + SommetGaucheBasInférieur.y + ", " + SommetGaucheBasInférieur.z + ")");
        Debug.Log("SommetGaucheBasSuppérieur: (" + SommetGaucheBasSuppérieur.x + ", " + SommetGaucheBasSuppérieur.y + ", " + SommetGaucheBasSuppérieur.z + ")");
        Debug.Log("SommetGaucheHautInférieur: (" + SommetGaucheHautInférieur.x + ", " + SommetGaucheHautInférieur.y + ", " + SommetGaucheHautInférieur.z + ")");
        Debug.Log("SommetGaucheHautSuppérieur: (" + SommetGaucheHautSuppérieur.x + ", " + SommetGaucheHautSuppérieur.y + ", " + SommetGaucheHautSuppérieur.z + ")");
        Debug.Log("SommetDroiteBasInférieur: (" + SommetDroiteBasInférieur.x + ", " + SommetDroiteBasInférieur.y + ", " + SommetDroiteBasInférieur.z + ")");
        Debug.Log("SommetDroiteBasSuppérieur: (" + SommetDroiteBasSuppérieur.x + ", " + SommetDroiteBasSuppérieur.y + ", " + SommetDroiteBasSuppérieur.z + ")");
        Debug.Log("SommetDroiteHautInférieur: (" + SommetDroiteHautInférieur.x + ", " + SommetDroiteHautInférieur.y + ", " + SommetDroiteHautInférieur.z + ")");
        Debug.Log("SommetDroiteHautSuppérieur: (" + SommetDroiteHautSuppérieur.x + ", " + SommetDroiteHautSuppérieur.y + ", " + SommetDroiteHautSuppérieur.z + ")");
    }

    public void Positionnement()
    {
        transform.position = new Vector3(0, Hauteur, 0);
        transform.RotateAround(Vector3.zero, Vector3.down, AngleDébut);
        transform.Rotate(new Vector3(Rotation, 0, 0));
    }

    public void CréationObject(Material material)
    {
        Maillage = new Mesh
        {
            name = "PlateformePic"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        //GetComponent<MeshCollider>().convex = true;                       <-- le mesh collider ne fit plus avec son mesh réel
        //GetComponent<MeshCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }


    bool IsPointDessus(Vector3 point)
    {
        return (Maths.EstDansLeRange(point.y, PositionDessus, PositionDessus));
    }
    bool IsPointDessous(Vector3 point)
    {
        return (Maths.EstDansLeRange(point.y, PositionDessous, PositionDessous));
    }
    bool IsPointCôté(Vector3 point)
    {
        int x = 0;
        return IsPointCôté(point, ref x);
    }
    bool IsPointCôté(Vector3 point, ref int côtéCollision)
    {
        bool c = false; // ne marche pas avec rotation
        if (Maths.EstDansLeRange(point.y, PositionDessus, PositionDessous, -INCERTITUDE_COLLISION))
        {
            if (Maths.EstDansLeRange(point.x, SommetGaucheBasInférieur.x, SommetGaucheBasSuppérieur.x, INCERTITUDE_COLLISION) &&
                Maths.EstDansLeRange(point.z, SommetGaucheBasInférieur.z, SommetGaucheBasSuppérieur.z, INCERTITUDE_COLLISION))
            { c = true; côtéCollision = -1; }
            else if (Maths.EstDansLeRange(point.x, SommetDroiteBasInférieur.x, SommetDroiteBasSuppérieur.x, INCERTITUDE_COLLISION) &&
                     Maths.EstDansLeRange(point.z, SommetDroiteBasInférieur.z, SommetDroiteBasSuppérieur.z, INCERTITUDE_COLLISION))
            { c = true; côtéCollision = 1; }
        }
        if (Rotation == 180) { côtéCollision = -côtéCollision; }
        return c;
    }
    //bool IsPointCôté(Vector3 point, ref int côtéCollision)
    //{
    //    bool estNiDessusNiDessous = false;
    //    if (!IsPointDessus(point) && !IsPointDessous(point)) { estNiDessusNiDessous = true; }
    //    if ((point - SommetGaucheBasInférieur).magnitude < (point - SommetDroiteBasInférieur).magnitude) { côtéCollision = -1; }
    //    else { côtéCollision = 1; }
    //    return estNiDessusNiDessous;
    //}


    public bool CollisionDessus(Collision collision)
    {
        bool auDessus = false;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (IsPointDessus(cp.point)) { auDessus = true; }
        }
        return auDessus;
    }
    public bool CollisionCôté(Collision collision, ref int côtéCollision) 
    {
        bool surCôté = false;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (IsPointCôté(cp.point, ref côtéCollision)) { surCôté = true; }
        }
        return surCôté;
    }
    public bool CollisionDessusEtCôté(Collision collision)
    {
        bool auPasDessous = false;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (!IsPointDessous(cp.point)) { auPasDessous = true; }
        }
        return auPasDessous;
    }




    // MAILLAGE

    virtual protected void CalculerDonnéesDeBase()
    {
        //Origine = transform.position; // l'origine et la position devrait etre pas etre la même chose (utilie pour la translation verticela de la plateforme mobile)
        Origine = DataÉtage.Origine; // le decalage se ferait ici
        //AngleDébut = Maths.DegréEnRadian(AngleDébut);
        nbTranches = (int)Mathf.Ceil(Amplitude * NB_TUILES_PAR_CERCLE_COMPLET / 360);
        nbSommets = (nbTranches + 1) * 5 + NB_SOMMETS_BOUTS;
        nbTriangles = (nbTranches * 4 + NB_DE_BOUT) * NB_TRIANGLES_PAR_TUILE;
        DeltaAngle = Maths.DegréEnRadian(Amplitude) / nbTranches;
        DeltaTexture = DeltaAngle / Maths.DegréEnRadian(NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR);
        DeltaÉlévation = Inclinaison / nbTranches;
    }

    virtual protected void GénérerTriangles()
    {
        GénérerSommets();
        GénérerCoordonnéesDeTextures();
        GénérerListeTriangles();
    }

    protected virtual void GénérerSommets()
    {
        Sommets = new Vector3[nbSommets];

        // Sommets du corps
        for(int n = 0; n < nbTranches + 1; ++n)
        {
            float angleAjouté = DeltaAngle * n;
            float élévationAjouté = DeltaÉlévation * n;
            Sommets[n] = Sommets[n + ((nbTranches + 1) * 4)] = Sommet(angleAjouté, élévationAjouté, false, true);
            Sommets[(nbTranches + 1) + n] = Sommet(angleAjouté, élévationAjouté, false, false);
            Sommets[(nbTranches + 1) * 2 + n] = Sommet(angleAjouté, élévationAjouté, true, false);
            Sommets[(nbTranches + 1) * 3 + n] = Sommet(angleAjouté, élévationAjouté, true, true);
        }

        Vector3 testerSommet = new Vector3(3, 3, 3);

        // Sommets des deux bouts
        Sommets[nbSommets - 8] = Sommets[(nbTranches + 1) * 2]; // gauche bas inférieur
        Sommets[nbSommets - 7] = Sommets[(nbTranches + 1) * 3]; // gauche bas suppérieur
        Sommets[nbSommets - 6] = Sommets[nbTranches + 1];       // gauche haut inférieur
        Sommets[nbSommets - 5] = Sommets[0];                    // gauche haut suppérieur

        Sommets[nbSommets - 4] = Sommets[(nbTranches + 1) * 3 - 1]; // droit bas inférieur
        Sommets[nbSommets - 3] = Sommets[(nbTranches + 1) * 4 - 1]; // droit bas suppérieur
        Sommets[nbSommets - 2] = Sommets[(nbTranches + 1) *2 - 1];  // droit haut inférieur
        Sommets[nbSommets - 1] = Sommets[nbTranches];               // droit haut suppérieur

        

        Maillage.vertices = Sommets;
    }

    protected Vector3 Sommet(float angleAjouté, float inclinaisonAjouté, bool sommetDuDessous, bool sommetSuppérieur)
    {
        return new Vector3((Mathf.Cos(angleAjouté)) * (Rayon + (sommetSuppérieur ? Largeur : 0)),
                            inclinaisonAjouté + (sommetDuDessous ? -Épaisseur : 0),
                           (Mathf.Sin(angleAjouté)) * (Rayon + (sommetSuppérieur ? Largeur : 0)));
    }


    protected virtual void GénérerCoordonnéesDeTextures()
    {
        Vector2[] CoordonnéesTexture = new Vector2[nbSommets];
        float ratio = Épaisseur / Largeur;

        // Coordonnées de texture du corps
        for (int n = 0; n < (nbTranches + 1) * 5; ++n)
        {
            int nièmeArrête = (n - n % (nbTranches + 1)) / (nbTranches + 1);
            CoordonnéesTexture[n] = new Vector2((n % (nbTranches + 1)) * DeltaTexture, nièmeArrête % 2 == 0 ? nièmeArrête / 2 * (1 + ratio) : (nièmeArrête - 1) / 2 * (1 + ratio) + 1);
        }

        // Coordonnées de textures des deux bouts
        CoordonnéesTexture[nbSommets - 8] = CoordonnéesTexture[nbSommets - 4] = new Vector2(0, 0);
        CoordonnéesTexture[nbSommets - 7] = CoordonnéesTexture[nbSommets - 3] = new Vector2(1, 0);
        CoordonnéesTexture[nbSommets - 6] = CoordonnéesTexture[nbSommets - 2] = new Vector2(0, ratio);
        CoordonnéesTexture[nbSommets - 5] = CoordonnéesTexture[nbSommets - 1] = new Vector2(1, ratio);

        Maillage.uv = CoordonnéesTexture;
    }

    protected virtual void GénérerListeTriangles()
    {
        int[] Triangles = new int[nbTriangles * NB_SOMMETS_PAR_TRIANGLE];

        // Triangles du corps
        int cpt = 0;
        for (int t = 0; t < nbTranches; ++t)
        {
            for (int f = 0; f < 4; ++f)
            {
                Triangles[cpt] = f * (nbTranches + 1) + t;
                Triangles[cpt + 1] = Triangles[cpt + 4] = Triangles[cpt] + nbTranches + 1;
                Triangles[cpt + 2] = Triangles[cpt + 3] = Triangles[cpt] + 1;
                Triangles[cpt + 5] = Triangles[cpt + 1] + 1;

                cpt += NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE;
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
