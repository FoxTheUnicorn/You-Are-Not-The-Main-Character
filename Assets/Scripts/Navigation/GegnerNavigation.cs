using UnityEngine;
using UnityEngine.AI;

public class GegnerNavigation : MonoBehaviour
{
    public float gegnerGeschwindigkeit = 3.5f;
    public float gegnerWartenMin = 1f, gegnerWartenMax = 3f;
    public float minX, maxX, minZ, maxZ;
    public NavMeshAgent navMeshAgent;
    public GameObject boden;
    public float angriffsRadius = 7f;
    public float schlagRadius = 1.2f;
    public int angriffsSperreFrames = 10;
    public int schlagSperreFramesMin = 60, schlagSperreFramesMax = 90;
    public float leben = 150f;
    private float schaden = 0f;
    public float angriffsSchadenMin = 10f, angriffsSchadenMax = 20f;
    private int angriffsSperreFramesZaehler = 0;
    private Bounds bodenBounds;
    private bool gegnerHatZiel, fehlgeschlagen;
    int festhaengenErkennenAnzFrames;
    int festhaengenErkennenFramesZaehler;
    Vector3 festhaengenErkennenLetztePosition = new Vector3(0f, 0f, 0f);
    public HeldNavigation heldNavigation;
    public float angriffsRadiusIncMin = 0.002f, angriffsRadiusIncMax = 0.004f;
    private Material material;
    private Color standardFarbe;
    public float schadenAnzeigenFrames = 15;
    private float schadenAnzeigenZaehler = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Schrott für Testlevel
        if (maxX < transform.position.x)
        {
            minX += 92.5f;
            maxX += 92.5f;
        }
        else if (minX > transform.position.x)
        {
            minX -= 95.8f;
            maxX -= 95.8f;
        }

        material = GetComponent<Renderer>().material;
        standardFarbe = material.GetColor("_Color");
        heldNavigation.gegnerHinzufuegen(this);
        bodenBounds = boden.GetComponent<Renderer>().bounds;
        navMeshAgent.speed = gegnerGeschwindigkeit;
        fehlgeschlagen = true;
        festhaengenErkennenAnzFrames = (int)Random.Range(8f, 20f);
    }

    private void zufaelligesZielFestlegen()
    {
        Vector3 ziel;
        ziel = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        navMeshAgent.SetDestination(ziel);
        if (ziel.x < navMeshAgent.destination.x - 0.001f || ziel.x > navMeshAgent.destination.x + 0.001f || ziel.z < navMeshAgent.destination.z - 0.001f || ziel.z > navMeshAgent.destination.z + 0.001f)
        {
            gegnerHatZiel = false;
            fehlgeschlagen = true;
            navMeshAgent.isStopped = true;
        }
        else
        {
            gegnerHatZiel = true;
            fehlgeschlagen = false;
            navMeshAgent.isStopped = false;
            festhaengenErkennenFramesZaehler = 0;
        }
    }

    bool angreifen()
    {
        if (angriffsSperreFramesZaehler > 0)
            angriffsSperreFramesZaehler--;
        if (angriffsSperreFramesZaehler <= angriffsSperreFrames)
        {
            float gegnerX = transform.position.x, gegnerZ = transform.position.z;
            float heldX = heldNavigation.transform.position.x, heldZ = heldNavigation.transform.position.z;
            if (Mathf.Abs(heldX - gegnerX) <= angriffsRadius && Mathf.Abs(heldZ - gegnerZ) <= angriffsRadius)
            {
                float abstand = Mathf.Sqrt((heldX - gegnerX) * (heldX - gegnerX) + (heldZ - gegnerZ) * (heldZ - gegnerZ));
                if (abstand <= schlagRadius)
                {
                    heldNavigation.schadenZufuegen(Random.Range(angriffsSchadenMin, angriffsSchadenMax));
                    angriffsSperreFramesZaehler = (int)Random.Range(schlagSperreFramesMin, schlagSperreFramesMax);
                    gegnerHatZiel = false;
                    fehlgeschlagen = true;
                    navMeshAgent.isStopped = true;
                }
                else if (angriffsSperreFramesZaehler <= 0 && abstand <= angriffsRadius)
                {
                    navMeshAgent.SetDestination(heldNavigation.transform.position);
                    gegnerHatZiel = true;
                    fehlgeschlagen = false;
                    navMeshAgent.isStopped = false;
                }
            }
        }
        return (angriffsSperreFramesZaehler > 0);
    }

    void FixedUpdate()
    {
        if (!angreifen() && fehlgeschlagen)
            zufaelligesZielFestlegen();
        else if (!navMeshAgent.hasPath && gegnerHatZiel)
        {
            gegnerHatZiel = false;
            Invoke("zufaelligesZielFestlegen", Random.Range(gegnerWartenMin, gegnerWartenMax));
        }
        else if (gegnerHatZiel) festhaengenErkennen();

        float gegnerX = transform.position.x, gegnerZ = transform.position.z;
        float heldX = heldNavigation.transform.position.x, heldZ = heldNavigation.transform.position.z;
        float abstandQuadrat = (heldX - gegnerX) * (heldX - gegnerX) + (heldZ - gegnerZ) * (heldZ - gegnerZ);
        if (abstandQuadrat < (bodenBounds.max.z - bodenBounds.min.z) * (bodenBounds.max.z - bodenBounds.min.z))
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

    private void festhaengenErkennen()
    {
        if (festhaengenErkennenLetztePosition.x < transform.position.x - 0.001f || festhaengenErkennenLetztePosition.x > transform.position.x + 0.001f || festhaengenErkennenLetztePosition.z < transform.position.z - 0.001f || festhaengenErkennenLetztePosition.z > transform.position.z + 0.001f)
        {
            festhaengenErkennenLetztePosition = transform.position;
            festhaengenErkennenFramesZaehler = 0;
        }
        else
            festhaengenErkennenFramesZaehler++;
        if (festhaengenErkennenFramesZaehler >= festhaengenErkennenAnzFrames)
        {
            zufaelligesZielFestlegen();
            angriffsSperreFramesZaehler = angriffsSperreFrames * 2;
        }
    }

    public bool schadenZufuegen(float schaden)
    {
        schadenAnzeigenZaehler = schadenAnzeigenFrames;
        material.SetColor("_Color", new Color(1f, 0.5f, 0.5f, 1f));
        this.schaden += schaden;
        if (this.schaden >= leben)
        {
            this.schaden = leben;
            return true;
        }
        return false;
    }
}
