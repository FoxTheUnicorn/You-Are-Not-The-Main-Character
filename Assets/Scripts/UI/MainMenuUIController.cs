using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    public List<GameObject> UIScenes;

    public void ShowScene(string name)
    {
        foreach (GameObject UI in UIScenes)
        {
            if(UI.name.Equals(name))
            {
                UI.SetActive(true);
                continue;
            }
            UI.SetActive(false);
        }
    }

    public void ShowScene(int id)
    {
        HideAll();
        UIScenes[id].SetActive(true);
    }

    public void HideAll()
    {
        foreach(GameObject UI in UIScenes)
        {
            UI.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
