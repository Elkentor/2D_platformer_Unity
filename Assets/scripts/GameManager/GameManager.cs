using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int maxLives = 9;
    public GameState currentState;

    public enum GameState { Title, Playing, GameOver }

    public int Score { get; private set; } = 0;
    public int PlayerLives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetState(GameState.Title);
    }

    private void Update()
    {
        if (currentState == GameState.GameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            LoadTitleMenu();
        }
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Title:
                SceneManager.LoadScene("TitleMenu");
                break;
            case GameState.Playing:
                PlayerLives = 3;
                Score = 0;
                SceneManager.LoadScene("GameScene");
                break;
            case GameState.GameOver:
                SceneManager.LoadScene("GameOverMenu");
                break;
        }
    }

    public void PlayerDied()
    {
        PlayerLives = Mathf.Clamp(PlayerLives - 1, 0, maxLives);
        Debug.Log($"Player died. Lives left: {PlayerLives}");

        if (PlayerLives <= 0)
        {
            SetState(GameState.GameOver);
        }
        else
        {
            RespawnPlayer();
        }
    }

    public void AddScore(int amount)
    {
        Score = Mathf.Max(0, Score + amount);
        Debug.Log($"Score updated: {Score}");
    }

    public void AddLife(int amount)
    {
        PlayerLives = Mathf.Clamp(PlayerLives + amount, 0, maxLives);
        Debug.Log($"Life updated: {PlayerLives}");
    }

    public void RespawnPlayer()
    {
        Debug.Log("Respawning player...");
        // You can expand this to find the player and reset their state if needed
    }

    public void StartGame()
    {
        Debug.Log("startgamecall");
        SetState(GameState.Playing);
    }

    public void LoadTitleMenu()
    {
        SetState(GameState.Title);
    }
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the editor
#endif
    }

}
