using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{


    public void StartGame()
    {
        PlayerPrefs.SetInt("Online Game", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //change later probably
    public void OnlineGame()
    {
        PlayerPrefs.SetInt("Online Game", 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LocalPickCommando()
    {
        PlayerPrefs.SetInt("Player Class", (int)PlayerClass.Commando);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(FindObjectOfType<MenuMusic>().gameObject);
    }
    public void LocalPickAssault()
    {
        PlayerPrefs.SetInt("Player Class", (int)PlayerClass.Assault);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(FindObjectOfType<MenuMusic>().gameObject);
    }
    public void LocalPickScout()
    {
        PlayerPrefs.SetInt("Player Class", (int)PlayerClass.Scout);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(FindObjectOfType<MenuMusic>().gameObject);
    }
    public void LocalPickDemo()
    {
        PlayerPrefs.SetInt("Player Class", (int)PlayerClass.Demo);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(FindObjectOfType<MenuMusic>().gameObject);
    }

}
