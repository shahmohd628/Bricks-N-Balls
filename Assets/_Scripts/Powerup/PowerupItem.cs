using UnityEngine;

public class PowerupItem : MonoBehaviour
{
    public PowerupType type;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private SpriteRenderer shapeRenderer;

    public void Init(PowerupType powerupType, Sprite sprite)
    {
        type = powerupType;
        if (sprite != null) shapeRenderer.sprite = sprite;
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        if (transform.position.y < -6f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            AudioManager.Instance.PlayPowerupCollect();
            PowerupManager.Instance.ActivatePowerup(type);
            Destroy(gameObject);
        }
    }
}