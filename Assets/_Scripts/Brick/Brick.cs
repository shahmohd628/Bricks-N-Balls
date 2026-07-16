using UnityEngine;

public enum BrickType { Normal, Strong, Steel, Explosive, Heal }

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickType brickType = BrickType.Normal;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float explosionRadius = 1.2f;
    [SerializeField] private GameObject explosionVfxPrefab;
    [SerializeField] private int brickScore = 5;

    [Header("Crack-stage children")]
    [SerializeField] private GameObject[] crackStages;

    [Header("Powerup drop chance")]
    [SerializeField] [Range(0f, 1f)] private float minDropChance = 0f;
    [SerializeField] [Range(0f, 1f)] private float maxDropChance = 1f;

    private int hitPoints;
    private int maxHitPoints;
    private float powerupDropChance;
    private bool isDestroyed;

    private void Awake()
    {
        powerupDropChance = Random.Range(minDropChance, maxDropChance);
        SetMaxHitPointsForType();
        hitPoints = maxHitPoints;

        foreach (var stage in crackStages)
            if (stage != null) stage.SetActive(false);
    }

    private void SetMaxHitPointsForType()
    {
        switch (brickType)
        {
            case BrickType.Normal: maxHitPoints = 1; break;
            case BrickType.Strong: maxHitPoints = 2; break;
            case BrickType.Steel: maxHitPoints = 3; break;
            case BrickType.Explosive: maxHitPoints = 3; break;
            case BrickType.Heal: maxHitPoints = 1; break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDestroyed) return;
        if (!collision.collider.CompareTag("Ball")) return;

        hitPoints--;

        if (hitPoints <= 0)
        {
            BreakBrick();
        }
        else
        {
            AudioManager.Instance.PlayBrickHit(); // only plays on a hit that DOESN'T break the brick
            UpdateCrackVisual();
        }
    }

    private void UpdateCrackVisual()
    {
        int hitsTaken = maxHitPoints - hitPoints;
        int stageIndex = hitsTaken - 1; // 1st hit -> stage 0, 2nd hit -> stage 1, etc.

        for (int i = 0; i < crackStages.Length; i++)
        {
            if (crackStages[i] == null) continue;
            crackStages[i].SetActive(i == stageIndex);
        }
    }

    private void BreakBrick()
    {
        AudioManager.Instance.PlayBrickBreak();
        if (isDestroyed) return;
        isDestroyed = true;

        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);


        if (brickType == BrickType.Explosive)
            TriggerExplosion();

        if (brickType == BrickType.Heal)
        {
            AudioManager.Instance.PlayPowerupCollect();
            GameManager.Instance.AddLife();
        }

        if (Random.value <= powerupDropChance)
            PowerupSpawner.Instance.SpawnRandomPowerup(transform.position);

        GameManager.Instance.AddScore(brickScore);
        GameManager.Instance.BrickDestroyed();
        Destroy(gameObject);
    }

    private void TriggerExplosion()
    {
        if (explosionVfxPrefab != null)
            Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Brick neighbor = hit.GetComponent<Brick>();
            if (neighbor != null)
                neighbor.DestroyByExplosion();
        }
    }

    // Bypasses normal hit-counting — used when caught in a neighboring explosion.
    public void DestroyByExplosion()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        GameManager.Instance.AddScore(brickScore);
        GameManager.Instance.BrickDestroyed();
        Destroy(gameObject);
    }
}