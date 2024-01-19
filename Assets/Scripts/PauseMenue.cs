using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Hosting;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenue : MonoBehaviour
{
    //Commented it out since it threw a bunch of errors
/*    public GameObject PauseMenue;
    public static bool IsPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        PauseMenue.SetActive(flalse);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKyDown(KeyCode.Escape))
        {
            if(IsPaused)
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PauseMenue.SetActive(true);
        Timeout.timeScale = 0f;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenue.SetActive(flase);
        Timeout.timeScale = 1f;
        IsPaused = false;
    }

    public void GoToMainMenu()
    {
        Timeout.timescale = 1f;
        SceneManager - LoadScene("GameMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
*/
}
