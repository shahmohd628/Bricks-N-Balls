using System.Collections;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform paddle;
    [SerializeField] private float powerupDuration = 6f;

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
                paddleScaleRoutine = StartCoroutine(ScalePaddle(1.5f));
                break;
            case PowerupType.PaddleShrink:
                if (paddleScaleRoutine != null) StopCoroutine(paddleScaleRoutine);
                paddleScaleRoutine = StartCoroutine(ScalePaddle(0.6f));
                break;
        }
    }

    private void SpawnExtraBall()
    {
        BallController[] existing = FindObjectsByType<BallController>(FindObjectsSortMode.None);
        if (existing.Length == 0) return;

        GameObject clone = Instantiate(ballPrefab, existing[0].transform.position, Quaternion.identity);
        clone.GetComponent<BallController>().Launch();
    }

    private IEnumerator SlowDownBalls()
    {
        foreach (var b in FindObjectsByType<BallController>(FindObjectsSortMode.None))
            b.SetSpeed(b.CurrentSpeed * 0.5f);

        yield return new WaitForSeconds(powerupDuration);

        foreach (var b in FindObjectsByType<BallController>(FindObjectsSortMode.None))
            b.SetSpeed(b.CurrentSpeed * 2f);
    }

    private IEnumerator ScalePaddle(float multiplier)
    {
        paddle.localScale = new Vector3(originalPaddleScale.x * multiplier, originalPaddleScale.y, originalPaddleScale.z);
        yield return new WaitForSeconds(powerupDuration);
        paddle.localScale = originalPaddleScale;
    }
}