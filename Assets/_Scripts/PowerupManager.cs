using System.Collections;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform paddle;

    [Header("Durations (seconds) — DoubleBall has none, it's permanent")]
    [SerializeField] private float paddleGrowDuration = 4f;
    [SerializeField] private float paddleShrinkDuration = 4f;
    [SerializeField] private float slowDownDuration = 4f;

    private Vector3 originalPaddleScale;
    private Coroutine paddleScaleRoutine;
    private Coroutine slowDownRoutine;

    private void Awake()
    {
        Instance = this;
        originalPaddleScale = paddle.localScale;
    }

    public void ActivatePowerup(PowerupType type)
    {
        Debug.Log($"ActivatePowerup called with type: {type}");
        switch (type)
        {
            case PowerupType.DoubleBall:
                SpawnExtraBall();
                break;
            case PowerupType.SlowDown:
                if (slowDownRoutine != null) StopCoroutine(slowDownRoutine);
                slowDownRoutine = StartCoroutine(SlowDownBalls());
                break;
            case PowerupType.PaddleGrow:
                if (paddleScaleRoutine != null) StopCoroutine(paddleScaleRoutine);
                paddleScaleRoutine = StartCoroutine(ScalePaddle(1.5f, paddleGrowDuration));
                break;
            case PowerupType.PaddleShrink:
                if (paddleScaleRoutine != null) StopCoroutine(paddleScaleRoutine);
                paddleScaleRoutine = StartCoroutine(ScalePaddle(0.6f, paddleShrinkDuration));
                break;
        }
    }

    private void SpawnExtraBall()
    {
        BallController[] existing = FindObjectsByType<BallController>(FindObjectsSortMode.None);
        if (existing.Length == 0) return;

        BallController source = existing[0];
        if (!source.IsLaunched) return;

        Vector2 sourceDir = source.GetComponent<Rigidbody2D>().linearVelocity.normalized;

        float splitAngle = Random.Range(25f, 50f);
        Vector2 dirA = Quaternion.Euler(0, 0, splitAngle / 2f) * sourceDir;
        Vector2 dirB = Quaternion.Euler(0, 0, -splitAngle / 2f) * sourceDir;

        source.Redirect(dirA);

        GameObject clone = Instantiate(ballPrefab, source.transform.position, Quaternion.identity);
        clone.GetComponent<BallController>().LaunchInDirection(dirB, source.CurrentSpeed);
    }

    private IEnumerator SlowDownBalls()
    {
        foreach (var b in FindObjectsByType<BallController>(FindObjectsSortMode.None))
            b.SetSpeed(b.BaseSpeed * 0.5f);

        yield return new WaitForSeconds(slowDownDuration);

        foreach (var b in FindObjectsByType<BallController>(FindObjectsSortMode.None))
            b.SetSpeed(b.BaseSpeed);

        slowDownRoutine = null;
    }

    private IEnumerator ScalePaddle(float multiplier, float duration)
    {
        Debug.Log($"ScalePaddle running. Original: {originalPaddleScale}, target: {originalPaddleScale * multiplier}");
        paddle.localScale = new Vector3(originalPaddleScale.x * multiplier, originalPaddleScale.y, originalPaddleScale.z);
        Debug.Log($"Paddle scale after set: {paddle.localScale}");
        yield return new WaitForSeconds(duration);
        paddle.localScale = originalPaddleScale;
        paddleScaleRoutine = null;
    }
}