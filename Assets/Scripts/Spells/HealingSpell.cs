using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpell : MonoBehaviour
{
    [SerializeField] ParticleSystem HealingBurstEffect;

    [SerializeField] float EnemyHealingMaxHP = 0.5f; //Percentage
    [SerializeField] float EnemyHealingFlat = 0.5f; //Flat
    [SerializeField] float PlayerHealingMaxHP = 5.0f; //Percentage
    [SerializeField] float PlayerHealingFlat = 0.0f; //Flat
    [SerializeField] float HeroHealingMaxHP = 2.0f; //Percentage
    [SerializeField] float HeroHealingFlat = 0.0f; //Flat
    [SerializeField] int HealingBursts = 6;
    [SerializeField] float HealingDuration = 5.5f;
    [SerializeField] AudioSource Sound;

    public float HealingBurstDelay;
    int BurstsSent = 0;

    [SerializeField] List<GameObject> Entities = new List<GameObject>();    //Don't add anything to this

    private void Start()
    {
        HealingBurstDelay = HealingDuration / HealingBursts;
        Sound.Play(); 
        Invoke("HealingBurst", HealingBurstDelay);
    }

    public void HealingBurst()
    {
        if (BurstsSent >= HealingBursts)
        {
            Invoke("KillObject", 1.0f);
            return;
        }
        if(BurstsSent > 0) HealingBurstEffect.Play();
        HealEntities();
        BurstsSent++;
        Invoke("HealingBurst", HealingBurstDelay);
    }

    private void HealEntities()
    {
        foreach(GameObject Entity in Entities)
        {
            //TODO Jörn
            /*EntityScript script = GetComponent<EntityScript>()
            float MaxHealth = script.GetMaxHealth();  //GetMaxHealth
            if (script.type == "Enemy")
            {
                script.Heal((EnemyHealingMaxHP * MaxHealth) + EnemyHealingFlat);
            }
            else if ( script.type == "Player")
            {
                script.Heal((PlayerHealingMaxHP * MaxHealth) + PlayerHealingFlat);
            }
            else if ( script.type == "Hero")
            {
                script.Heal((HeroHealingMaxHP * MaxHealth) + HeroHealingFlat);
            }*/
            Character characterScript = Entity.GetComponent<Character>();
            if (characterScript == null) return;
            Debug.Log("Healing " + Entity.gameObject.name);
            float MaxHealth = characterScript.getMaxHealth();  //GetMaxHealth
            if (characterScript is EnemyCharacter)
            {
                characterScript.heal((EnemyHealingMaxHP / 100f * MaxHealth) + EnemyHealingFlat);
            }
            else if (characterScript is PlayerCharacter)
            {
                characterScript.heal((PlayerHealingMaxHP / 100f * MaxHealth) + PlayerHealingFlat);
            }
            else if (characterScript is HeroCharacter)
            {
                characterScript.heal((HeroHealingMaxHP / 100f * MaxHealth) + HeroHealingFlat);
            }
        }
    }

    private bool isHealable(GameObject obj)
    {
        bool IsHealable = false;
        if (obj.CompareTag("Enemy")) IsHealable = true;
        if (obj.CompareTag("Player")) IsHealable = true;
        if (obj.CompareTag("Hero")) IsHealable = true;
        return IsHealable;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isHealable(other.gameObject)) return;
        Entities.Remove(other.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isHealable(other.gameObject)) return;
        Debug.Log("Entered: " + other.gameObject.name);
        Entities.Add(other.gameObject);
    }

    public void KillObject()
    {
        Sound.Stop();
        Destroy(gameObject);
    }
}
