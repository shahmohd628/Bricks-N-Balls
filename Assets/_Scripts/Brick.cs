using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private GameObject destroyEffect;

    [Header("Each brick rolls its own drop chance in this range at spawn")]
    [SerializeField] [Range(0f, 1f)] private float minDropChance = 0f;
    [SerializeField] [Range(0f, 1f)] private float maxDropChance = 1f;

    private float powerupDropChance;
    private bool isDestroyed; // guards against being processed twice in one frame

    private void Awake()
    {
        powerupDropChance = Random.Range(minDropChance, maxDropChance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDestroyed) return;
        if (!collision.collider.CompareTag("Ball")) return;

        hitPoints--;
        if (hitPoints <= 0) BreakBrick();
    }

    private void BreakBrick()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        AudioManager.Instance.PlayBrickBreak();

        if (Random.value <= powerupDropChance)
            PowerupSpawner.Instance.SpawnRandomPowerup(transform.position);

        GameManager.Instance.AddScore(10);
        GameManager.Instance.BrickDestroyed();
        Destroy(gameObject);
    }
}