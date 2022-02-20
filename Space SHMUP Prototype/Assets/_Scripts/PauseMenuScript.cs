using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public Animator PauseButton;
    // Start is called before the first frame update
    public void PauseMenuActive()//Включит паузу 
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void PauseMenuResume()//Возвращатса в игру 
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseQuit()//когда возвращаемся на стартовый экран
    {
        Time.timeScale = 1f;
        PauseButton.SetTrigger("Close");
    }
  
}
