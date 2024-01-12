using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HeldNavigation : MonoBehaviour
{
    public float heldGeschwindigkeit = 3.5f;
    public float heldWartenMin = 1.0f, heldWartenMax = 2.5f;
    public NavMeshAgent navMeshAgent;
    public GameObject boden;
    public float angriffsRadiusStart = 5f;
    private float angriffsRadius;
    public float schlagRadius = 1.2f;
    public int angriffsSperreFrames = 10;
    public int schlagSperreFramesMin = 50, schlagSperreFramesMax = 65;
    public float leben = 1000f;
    private float schaden = 0f;
    public float angriffsSchadenMin = 30f, angriffsSchadenMax = 60f;
    public float angriffsRadiusIncMin = 0.002f, angriffsRadiusIncMax = 0.004f;
    private int angriffsSperreFramesZaehler = 0;
    private Bounds bodenBounds;
    private bool heldHatZiel, fehlgeschlagen;
    private List<GegnerNavigation> gegnerListe = new List<GegnerNavigation>();
    private Material material;
    private Color standardFarbe;
    public float schadenAnzeigenFrames = 15;
    private float schadenAnzeigenZaehler = 0;
    public Camera kamera;
    public GameObject zielObjekt;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        standardFarbe = material.GetColor("_Color");
        angriffsRadius = angriffsRadiusStart;
        bodenBounds = boden.GetComponent<Renderer>().bounds;
        navMeshAgent.speed = heldGeschwindigkeit;
        fehlgeschlagen = true;
    }

    private void zufaelligesZielFestlegen()
    {
        Vector3 ziel;
        if (gegnerListe.Count > 0)
        {
            float minGegnerX = 10000000f;
            foreach (GegnerNavigation gegnerNavigation in gegnerListe)
                if (gegnerNavigation.transform.position.x < minGegnerX)
                    minGegnerX = gegnerNavigation.transform.position.x;
            ziel = new Vector3(Random.Range(minGegnerX - (bodenBounds.max.z - bodenBounds.min.z) / 2f, minGegnerX + (bodenBounds.max.z - bodenBounds.min.z) / 2f), 0f, Random.Range(bodenBounds.min.z, bodenBounds.max.z));
        }
        else ziel = zielObjekt.transform.position;
        navMeshAgent.SetDestination(ziel);
        //Debug.Log(ziel);
        //Debug.Log(navMeshAgent.destination);
        if (ziel.x < navMeshAgent.destination.x - 0.001f || ziel.x > navMeshAgent.destination.x + 0.001f || ziel.z < navMeshAgent.destination.z - 0.001f || ziel.z > navMeshAgent.destination.z + 0.001f)
        {
            heldHatZiel = false;
            fehlgeschlagen = true;
            navMeshAgent.isStopped = true;
        }
        else
        {
            heldHatZiel = true;
            fehlgeschlagen = false;
            navMeshAgent.isStopped = false;
        }
    }

    bool angreifen()
    {
        if (angriffsSperreFramesZaehler > 0)
            angriffsSperreFramesZaehler--;
        if (angriffsSperreFramesZaehler <= angriffsSperreFrames)
        {
            float heldX = transform.position.x, heldZ = transform.position.z;
            float minAbstand = 100000000f;
            GegnerNavigation anzugreifenderGegner = null;
            foreach (GegnerNavigation gegnerNavigation in gegnerListe)
            {
                float gegnerX = gegnerNavigation.transform.position.x, gegnerZ = gegnerNavigation.transform.position.z;
                if (Mathf.Abs(heldX - gegnerX) <= angriffsRadius && Mathf.Abs(heldZ - gegnerZ) <= angriffsRadius)
                {
                    float abstand = Mathf.Sqrt((heldX - gegnerX) * (heldX - gegnerX) + (heldZ - gegnerZ) * (heldZ - gegnerZ));
                    if (abstand <= angriffsRadius && abstand < minAbstand)
                    {
                        minAbstand = abstand;
                        anzugreifenderGegner = gegnerNavigation;
                    }
                }
            }
            if (minAbstand <= schlagRadius)
            {
                angriffsSperreFramesZaehler = (int)Random.Range(schlagSperreFramesMin, schlagSperreFramesMax);
                if (anzugreifenderGegner.schadenZufuegen(Random.Range(angriffsSchadenMin, angriffsSchadenMax)))
                {
                    gegnerListe.Remove(anzugreifenderGegner);
                    Destroy(anzugreifenderGegner.transform.gameObject);
                }
                heldHatZiel = false;
                fehlgeschlagen = true;
                navMeshAgent.isStopped = true;
                angriffsRadius = angriffsRadiusStart;
            }
            else if (angriffsSperreFramesZaehler <= 0 && minAbstand <= angriffsRadius)
            {
                navMeshAgent.SetDestination(anzugreifenderGegner.transform.position);
                heldHatZiel = true;
                fehlgeschlagen = false;
                navMeshAgent.isStopped = false;
            }
        }
        return (angriffsSperreFramesZaehler > 0);
    }

    void FixedUpdate()
    {
        if (!angreifen() && fehlgeschlagen)
            zufaelligesZielFestlegen();
        else if (!navMeshAgent.hasPath && heldHatZiel)
        {
            heldHatZiel = false;
            Invoke("zufaelligesZielFestlegen", Random.Range(heldWartenMin, heldWartenMax));
        }
        angriffsRadius += Random.Range(angriffsRadiusIncMin, angriffsRadiusIncMax);
        if (schadenAnzeigenZaehler > 0)
        {
            schadenAnzeigenZaehler--;
            if (schadenAnzeigenZaehler == 0)
            {
                material.SetColor("_Color", standardFarbe);
            }
        }
    }

    void Update()
    {
        kamera.transform.position = new Vector3(transform.position.x, kamera.transform.position.y, kamera.transform.position.z);
    }

    public void gegnerHinzufuegen(GegnerNavigation gegnerNavigation)
    {
        gegnerListe.Add(gegnerNavigation);
    }

    public void schadenZufuegen(float schaden)
    {
        Debug.Log(schaden);
        schadenAnzeigenZaehler = schadenAnzeigenFrames;
        material.SetColor("_Color", new Color(128f/255f, 193f/255f, 1f, 1f));
        this.schaden += schaden;
        if (this.schaden >= leben)
        {
            this.schaden = leben;
            transform.gameObject.SetActive(false);
        }
        Debug.Log("Leben: " + ((leben - this.schaden) / leben * 100) + " %");
    }
}
