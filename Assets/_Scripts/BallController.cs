using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseSpeed = 8f;

    [Header("Paddle")]
    [SerializeField] private float maxBounceAngle = 70f;

    [Header("Safety")]
    [SerializeField] private float minY = 0.25f;
    [SerializeField] private float randomBounce = 2f;

    Rigidbody2D rb;
    float currentSpeed;

    public float CurrentSpeed => currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentSpeed = baseSpeed;
        Launch();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude < 0.01f)
        {
            Launch();
            return;
        }

        rb.linearVelocity =
            rb.linearVelocity.normalized * currentSpeed;
    }

    public void Launch()
    {
        float angle = Random.Range(-25f, 25f);

        Vector2 dir =
            Quaternion.Euler(0,0,angle) * Vector2.up;

        rb.linearVelocity =
            dir.normalized * currentSpeed;
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
        rb.linearVelocity =
            rb.linearVelocity.normalized * currentSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        if (collision.collider.CompareTag("Paddle"))
        {
            PaddleBounce(collision.collider);

            if(AudioManager.Instance!=null)
                AudioManager.Instance.PlayPaddleBounce();

            return;
        }

        Vector2 dir =
            Vector2.Reflect(
                rb.linearVelocity.normalized,
                contact.normal);

        if (collision.collider.CompareTag("Brick"))
        {
            dir =
                Quaternion.Euler(
                    0,
                    0,
                    Random.Range(-randomBounce, randomBounce))
                * dir;
        }

        if (Mathf.Abs(dir.y) < minY)
        {
            dir.y = Mathf.Sign(dir.y == 0 ? 1 : dir.y) * minY;
            dir.Normalize();
        }

        rb.position += contact.normal * 0.02f;

        rb.linearVelocity =
            dir.normalized * currentSpeed;
    }

    void PaddleBounce(Collider2D paddle)
    {
        float half =
            paddle.bounds.extents.x;

        float offset =
            (transform.position.x -
             paddle.transform.position.x) / half;

        offset = Mathf.Clamp(offset,-1f,1f);

        float angle =
            offset * maxBounceAngle;

        Vector2 dir =
            Quaternion.Euler(0,0,angle) *
            Vector2.up;

        if (Mathf.Abs(dir.y) < minY)
        {
            dir.y = minY;
            dir.Normalize();
        }

        rb.linearVelocity =
            dir.normalized * currentSpeed;
    }
}