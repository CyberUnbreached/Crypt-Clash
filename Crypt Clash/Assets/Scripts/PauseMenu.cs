using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject gameplayUICanvas; 
    [SerializeField] private GameObject optionsMenuCanvas;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuCanvas.SetActive(true);
        if (gameplayUICanvas != null)
            gameplayUICanvas.SetActive(false);  // Hide gameplay UI

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (optionsMenuCanvas != null)
            optionsMenuCanvas.SetActive(false); // Hide options menu if it's open
        pauseMenuCanvas.SetActive(false);
        if (gameplayUICanvas != null)
            gameplayUICanvas.SetActive(true);  // Show gameplay UI again

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
