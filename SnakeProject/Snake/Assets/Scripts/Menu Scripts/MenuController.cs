using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //play button
    public void PlayGame()
    {
        SceneManager.LoadScene("Snake");
    }
    //quit button
    public void QuitGame()
    {
        //print("QUITTING"); DEBUG
        Application.Quit();
    }
    //options button
    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

}
