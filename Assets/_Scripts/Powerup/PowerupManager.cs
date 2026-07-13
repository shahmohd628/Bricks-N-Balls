using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private PaddleScaler paddleScaler;

    [Header("Durations (seconds) — DoubleBall has none, it's permanent")]
    [SerializeField] private float paddleGrowDuration = 4f;
    [SerializeField] private float paddleShrinkDuration = 4f;
    [SerializeField] private float slowDownDuration = 4f;

    private Coroutine slowDownRoutine;

    private void Awake()
    {
        Instance = this;
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
                paddleScaler.GrowFor(paddleGrowDuration);
                break;
            case PowerupType.PaddleShrink:
                paddleScaler.ShrinkFor(paddleShrinkDuration);
                break;
        }
    }

    private void SpawnExtraBall()
    {
        BallController[] existing = FindObjectsByType<BallController>(FindObjectsSortMode.None);

        List<BallController> toClone = new List<BallController>();
        foreach (var b in existing)
            if (b.IsLaunched) toClone.Add(b);

        if (toClone.Count == 0) return;

        // Iterating a snapshot list, not the live scene, so freshly spawned
        // clones this same call don't get caught and re-doubled.
        foreach (var source in toClone)
        {
            Vector2 sourceDir = source.GetComponent<Rigidbody2D>().linearVelocity.normalized;
            float splitAngle = Random.Range(25f, 50f);
            Vector2 dirA = Quaternion.Euler(0, 0, splitAngle / 2f) * sourceDir;
            Vector2 dirB = Quaternion.Euler(0, 0, -splitAngle / 2f) * sourceDir;

            source.Redirect(dirA);

            GameObject clone = Instantiate(ballPrefab, source.transform.position, Quaternion.identity);
            clone.GetComponent<BallController>().LaunchInDirection(dirB, source.CurrentSpeed);
        }
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
}