using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float maxBounceAngleFromPaddle = 60f;
    [SerializeField] private float paddleJitterDegrees = 4f;
    [SerializeField] private float wallJitterDegrees = 6f;
    [SerializeField] private float minBounceComponent = 0.25f;
    [SerializeField] private float attachOffsetY = 0.45f;

    private Rigidbody2D rb;
    private float currentSpeed;
    private bool isLaunched;
    private Transform attachedPaddle;

    public bool IsLaunched => isLaunched;
    public float CurrentSpeed => currentSpeed;
    public float BaseSpeed => baseSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (!isLaunched && attachedPaddle != null)
        {
            transform.position = new Vector3(
                attachedPaddle.position.x,
                attachedPaddle.position.y + attachOffsetY,
                transform.position.z);
            return;
        }

        if (!isLaunched) return;

        // Self-checked bottom loss — works at any orthographic size/aspect ratio.
        if (transform.position.y < -ScreenBounds.HalfHeight - 1f)
        {
            GameManager.Instance.LoseBall(gameObject);
            return;
        }

        if (rb.linearVelocity.magnitude < 0.5f)
        {
            Vector2 nudge = Random.insideUnitCircle.normalized;
            if (nudge.y < 0.3f) nudge.y = 0.3f;
            rb.linearVelocity = nudge.normalized * currentSpeed;
        }
    }

    public void AttachToPaddle(Transform paddle)
    {
        attachedPaddle = paddle;
        isLaunched = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        currentSpeed = baseSpeed;
    }

    public void Launch()
    {
        float angle = Random.Range(-25f, 25f);
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
        LaunchInDirection(dir, currentSpeed);
    }

    public void LaunchInDirection(Vector2 direction, float speed)
    {
        attachedPaddle = null;
        isLaunched = true;
        currentSpeed = speed;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = direction.normalized * currentSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        if (isLaunched)
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
    }

    // Used by the 2x-ball split so both balls visibly separate.
    public void Redirect(Vector2 newDirection)
    {
        rb.linearVelocity = newDirection.normalized * currentSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLaunched) return;

        if (collision.collider.CompareTag("Paddle"))
        {
            HandlePaddleBounce(collision.collider);
            AudioManager.Instance.PlayPaddleBounce();
            return;
        }

        WallBounceSurface wall = collision.collider.GetComponent<WallBounceSurface>();
        if (wall != null)
        {
            HandleWallBounce(wall.orientation);
            return;
        }

        HandleMirrorBounce(collision);
    }

    private void HandlePaddleBounce(Collider2D paddle)
    {
        float paddleHalfWidth = paddle.bounds.extents.x;
        float offset = (transform.position.x - paddle.transform.position.x) / paddleHalfWidth;
        offset = Mathf.Clamp(offset, -1f, 1f);

        float angle = offset * maxBounceAngleFromPaddle;
        angle += Random.Range(-paddleJitterDegrees, paddleJitterDegrees);

        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
        rb.linearVelocity = dir.normalized * currentSpeed;
    }

    private void HandleWallBounce(WallOrientation orientation)
    {
        Vector2 v = rb.linearVelocity.sqrMagnitude > 0.01f
            ? rb.linearVelocity.normalized
            : Vector2.up;

        if (orientation == WallOrientation.Vertical)
            v.x = -v.x;
        else
            v.y = -v.y;

        float jitter = Random.Range(-wallJitterDegrees, wallJitterDegrees);
        v = (Quaternion.Euler(0, 0, jitter) * v).normalized;

        if (orientation == WallOrientation.Vertical && Mathf.Abs(v.x) < minBounceComponent)
        {
            float pushSign = transform.position.x > 0f ? -1f : 1f;
            v.x = pushSign * minBounceComponent;
        }
        else if (orientation == WallOrientation.Horizontal && Mathf.Abs(v.y) < minBounceComponent)
        {
            v.y = -minBounceComponent;
        }

        rb.linearVelocity = v.normalized * currentSpeed;
    }

    private void HandleMirrorBounce(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 incoming = rb.linearVelocity.sqrMagnitude > 0.01f
            ? rb.linearVelocity.normalized
            : -normal;

        Vector2 reflectDir = Vector2.Reflect(incoming, normal).normalized;
        rb.linearVelocity = reflectDir * currentSpeed;
    }
}