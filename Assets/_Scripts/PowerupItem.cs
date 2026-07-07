using UnityEngine;
using TMPro;

public class PowerupItem : MonoBehaviour
{
    public PowerupType type;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private SpriteRenderer shapeRenderer;
    [SerializeField] private TMP_Text label;

    public void Init(PowerupType powerupType)
    {
        type = powerupType;
        switch (type)
        {
            case PowerupType.DoubleBall:
                shapeRenderer.color = new Color(0.23f, 0.63f, 1f);   // blue
                label.text = "2X";
                break;
            case PowerupType.SlowDown:
                shapeRenderer.color = new Color(0.69f, 0.42f, 1f);   // purple
                label.text = "↓";
                break;
            case PowerupType.PaddleGrow:
                shapeRenderer.color = new Color(0.24f, 0.81f, 0.43f); // green
                label.text = "+";
                break;
            case PowerupType.PaddleShrink:
                shapeRenderer.color = new Color(1f, 0.36f, 0.36f);   // red
                label.text = "-";
                break;
        }
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        if (transform.position.y < -6f) Destroy(gameObject); // missed, clean up
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