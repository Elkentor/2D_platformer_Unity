using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.StartGame();
    }

    public void LoadTitleMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.LoadTitleMenu();
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

