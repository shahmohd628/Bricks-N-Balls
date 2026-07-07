using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private GameObject destroyEffect; // optional particle prefab
    [SerializeField] [Range(0f, 1f)] private float powerupDropChance = 0.2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Ball")) return;

        hitPoints--;
        if (hitPoints <= 0) BreakBrick();
    }

    private void BreakBrick()
    {
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