using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
