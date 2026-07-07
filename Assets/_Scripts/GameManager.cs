using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int startingLives = 3;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;

    private int score;
    private int lives;
    private int bricksRemaining;

    private void Awake() => Instance = this;

    private void Start()
    {
        lives = startingLives;
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateLives(lives);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }

    public void SetTotalBricks(int count) => bricksRemaining = count;

    public void BrickDestroyed()
    {
        bricksRemaining--;
        if (bricksRemaining <= 0) UIManager.Instance.ShowWinScreen();
    }

    public void LoseBall(GameObject ball)
    {
        Destroy(ball);

        BallController[] balls =
            FindObjectsByType<BallController>(FindObjectsSortMode.None);

        int remaining = 0;

        foreach (BallController b in balls)
        {
            if (b.gameObject != ball)
                remaining++;
        }

        if (remaining > 0)
            return;

        lives--;

        UIManager.Instance.UpdateLives(lives);

        if (lives <= 0)
        {
            UIManager.Instance.ShowLoseScreen();
            return;
        }

        Instantiate(ballPrefab,
            ballSpawnPoint.position,
            Quaternion.identity);
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void Menu() => SceneManager.LoadScene("Menu");
}