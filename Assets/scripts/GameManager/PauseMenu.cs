using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pauseMenuPanel;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void Pause()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }
    }

    public void Resume()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadTitleMenu();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ExitGame();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("PauseMenu script received P key");
        }
    }


}
