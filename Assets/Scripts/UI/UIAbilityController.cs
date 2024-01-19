using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAbilityController : MonoBehaviour
{
    [SerializeField] TMP_Text CooldownText;
    [SerializeField] Image CooldownOverlay;
    private int mins;
    private int secs;
    public void UpdateUI(float cooldown)
    {
        if(cooldown > 0)
        {
            CooldownText.gameObject.SetActive(true);
            CooldownOverlay.gameObject.SetActive(true);
            secs = (int) (cooldown % 60);
            mins = (int) (cooldown / 60);

            CooldownText.text = string.Format("{0:0}:{1:00}", mins, secs);
        }
        else
        {
            CooldownOverlay.gameObject.SetActive(false); ;
            CooldownText.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        CooldownOverlay.gameObject.SetActive(false); ;
        CooldownText.gameObject.SetActive(false);
    }
}
