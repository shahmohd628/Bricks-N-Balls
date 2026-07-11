using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int startingLives = 3;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform paddle;

    private int score;
    private int lives;
    private int bricksRemaining;
    private BallController pendingBall;

    private void Awake() => Instance = this;

    private void Start()
    {
        lives = startingLives;
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateLives(lives);
        SpawnBallOnPaddle();
    }

    private void SpawnBallOnPaddle()
    {
        GameObject ballObj = Instantiate(ballPrefab, paddle.position, Quaternion.identity);
        BallController ball = ballObj.GetComponent<BallController>();
        ball.AttachToPaddle(paddle);
        pendingBall = ball;
    }

    public void TryLaunchPendingBall()
    {
        if (pendingBall == null) return;
        pendingBall.Launch();
        pendingBall = null;
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
        BallController[] allBalls = FindObjectsByType<BallController>(FindObjectsSortMode.None);
        int remainingAfterThis = allBalls.Length - 1; // this ball is about to be destroyed

        Destroy(ball);

        if (remainingAfterThis > 0) return; // other balls still in play, no life lost

        lives--;
        UIManager.Instance.UpdateLives(lives);

        if (lives <= 0)
            UIManager.Instance.ShowLoseScreen();
        else
            SpawnBallOnPaddle();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // must un-pause before loading, or the new scene loads frozen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}