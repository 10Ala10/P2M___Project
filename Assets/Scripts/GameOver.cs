using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public GameObject GameOverUI;



    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");

    }

    public void LoadLevels()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level_Selection");
    }
}
