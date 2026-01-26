using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Room1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Le jeu se ferme (ne marche que dans le Build final)");
    }
}