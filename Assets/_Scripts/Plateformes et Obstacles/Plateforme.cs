﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Plateforme : MonoBehaviour
{
    public const string String = "Plateforme";

    public const float INCERTITUDE_COLLISION = 0.3f;

    protected const int NB_TRIANGLES_PAR_TUILE = 2, NB_SOMMETS_PAR_TRIANGLE = 3, NB_TUILES_PAR_CERCLE_COMPLET = 72, NB_SOMMETS_BOUTS = 8, NB_DE_BOUT = 2;
    protected const float NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR = 45;

    public float AngleDébut { get; protected set; }
    public float Amplitude { get; protected set; }
    public float Épaisseur { get; protected set; }
    public float Largeur { get; protected set; }
    public float Rayon { get; protected set; }
    public float Hauteur { get; protected set; }
    public float Inclinaison { get; protected set; }
    public float Rotation { get; protected set; }

    //MAILLAGE
    protected Vector3 Origine; // est toujours égal a DataÉtage.Origine (Vector3.zero)
    protected Mesh Maillage;
    protected Vector3[] Sommets;
    protected float DeltaAngle, DeltaTexture, DeltaÉlévation;
    protected int nbTranches, nbSommets, nbTriangles;

    //COLLISION
    protected Vector3 SommetDroiteHautSupérieur, SommetDroiteHautIntérieur, SommetDroiteBasSupérieur, SommetDroiteBasIntérieur, SommetGaucheHautSupérieur, SommetGaucheHautIntérieur, SommetGaucheBasSupérieur, SommetGaucheBasIntérieur;
    protected float PositionDessus, PositionDessous;

    public void InitialisationP(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float rotation, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude;
        Largeur = inclinaison == 0 ? largeur : largeur - largeur / 40; // si une plateforme a une inclinaison, nous diminuons la largeur pour qu'il n'y ai pas de "clipping"
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        Rotation = rotation;

        CréationObject(material);
        Positionnement();
        CréationPointCollision();

        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void CréationObject(Material material)
    {
        Maillage = new Mesh
        {
            name = "Plateforme"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Positionnement()
    {
        transform.position = new Vector3(0, Hauteur, 0);
        transform.RotateAround(Vector3.zero, Vector3.down, AngleDébut);
        transform.Rotate(new Vector3(Rotation, 0, 0));
    }

    public void CréationPointCollision()
    {
        // position réel des points du mesh par rapport au World
        SommetGaucheBasIntérieur  = Sommet(Mathf.Deg2Rad *  AngleDébut,              0,           true, false)  + new Vector3(0, transform.position.y, 0);
        SommetGaucheBasSupérieur  = Sommet(Mathf.Deg2Rad *  AngleDébut,              0,           true, true)   + new Vector3(0, transform.position.y, 0);
        SommetGaucheHautIntérieur = Sommet(Mathf.Deg2Rad *  AngleDébut,              0,           false, false) + new Vector3(0, transform.position.y, 0);
        SommetGaucheHautSupérieur = Sommet(Mathf.Deg2Rad *  AngleDébut,              0,           false, true)  + new Vector3(0, transform.position.y, 0);
        SommetDroiteBasIntérieur  = Sommet(Mathf.Deg2Rad * (AngleDébut + Amplitude), Inclinaison, true, false)  + new Vector3(0, transform.position.y, 0);
        SommetDroiteBasSupérieur  = Sommet(Mathf.Deg2Rad * (AngleDébut + Amplitude), Inclinaison, true, true)   + new Vector3(0, transform.position.y, 0);
        SommetDroiteHautIntérieur = Sommet(Mathf.Deg2Rad * (AngleDébut + Amplitude), Inclinaison, false, false) + new Vector3(0, transform.position.y, 0);
        SommetDroiteHautSupérieur = Sommet(Mathf.Deg2Rad * (AngleDébut + Amplitude), Inclinaison, false, true)  + new Vector3(0, transform.position.y, 0);
    }

    // MAILLAGE
    protected Vector3 Sommet(float angleAjouté, float inclinaisonAjouté, bool sommetDuDessous, bool sommetSuppérieur)
    {
        return new Vector3(Mathf.Cos(angleAjouté) * (Rayon + (sommetSuppérieur ? Largeur : 0)),
                           inclinaisonAjouté + (sommetDuDessous ? -Épaisseur : 0),
                           Mathf.Sin(angleAjouté) * (Rayon + (sommetSuppérieur ? Largeur : 0)));
    }

    virtual protected void CalculerDonnéesDeBase()
    {
        Origine = DataÉtage.Origine;
        nbTranches = (int)Mathf.Ceil(Amplitude * NB_TUILES_PAR_CERCLE_COMPLET / 360);
        nbSommets = (nbTranches + 1) * 5 + NB_SOMMETS_BOUTS;
        nbTriangles = (nbTranches * 4 + NB_DE_BOUT) * NB_TRIANGLES_PAR_TUILE;
        DeltaAngle = (Mathf.Deg2Rad * Amplitude) / nbTranches;
        DeltaTexture = DeltaAngle / (Mathf.Deg2Rad * NB_DEGRÉ_PAR_TEXTURE_SELON_LARGEUR);
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
            Sommets[n]                        = Sommets[n + ((nbTranches + 1) * 4)] = Sommet(angleAjouté, élévationAjouté, false, true);
            Sommets[(nbTranches + 1) + n]     = Sommet(angleAjouté, élévationAjouté, false, false);
            Sommets[(nbTranches + 1) * 2 + n] = Sommet(angleAjouté, élévationAjouté, true, false);
            Sommets[(nbTranches + 1) * 3 + n] = Sommet(angleAjouté, élévationAjouté, true, true);
        }

        // Sommets des deux extrémités
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

    // COLLISION
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

    public bool CollisionDessusOuCôté(Collision collision)
    {
        bool dessusOuCoté = false;
        int x = 0;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (IsPointDessus(cp.point) || IsPointCôté(cp.point, ref x)) { dessusOuCoté = true; }
        }
        return dessusOuCoté;
    }

    public bool CollisionDessous(Collision collision)
    {
        bool enDessous = false;
        foreach (ContactPoint cp in collision.contacts)
        {
            if (IsPointDessous(cp.point)) { enDessous = true; }
        }
        return enDessous;
    }

    bool IsPointDessus(Vector3 point)
    {
        return (Maths.EstDansLeRange(point.y, SommetDroiteHautIntérieur.y, SommetGaucheHautIntérieur.y, INCERTITUDE_COLLISION));
    }

    bool IsPointDessous(Vector3 point)
    {
        return (Maths.EstDansLeRange(point.y, SommetDroiteBasIntérieur.y, SommetGaucheBasIntérieur.y, INCERTITUDE_COLLISION));
    }

    bool IsPointCôté(Vector3 point, ref int côtéCollision)
    {
        bool c = false; 
        if (Maths.EstDansLeRange(point.y, SommetDroiteBasIntérieur.y, SommetGaucheHautIntérieur.y, -INCERTITUDE_COLLISION))
        {
            if (Maths.EstDansLeRange(point.x, SommetGaucheBasIntérieur.x, SommetGaucheBasSupérieur.x, INCERTITUDE_COLLISION) &&
                Maths.EstDansLeRange(point.z, SommetGaucheBasIntérieur.z, SommetGaucheBasSupérieur.z, INCERTITUDE_COLLISION))
            { c = true; côtéCollision = -1; }
            else if (Maths.EstDansLeRange(point.x, SommetDroiteBasIntérieur.x, SommetDroiteBasSupérieur.x, INCERTITUDE_COLLISION) &&
                     Maths.EstDansLeRange(point.z, SommetDroiteBasIntérieur.z, SommetDroiteBasSupérieur.z, INCERTITUDE_COLLISION))
            { c = true; côtéCollision = 1; }
        }
        if (Rotation == 180) { côtéCollision = -côtéCollision; }
        return c;
    }
}
