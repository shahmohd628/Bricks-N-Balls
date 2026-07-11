using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float edgeMargin = 0.1f;
    private Camera cam;
    private float halfPaddleWidth;
    private bool wasPressedLastFrame;

    private void Start()
    {
        cam = Camera.main;
        halfPaddleWidth = GetComponent<Collider2D>().bounds.extents.x;
    }

    private void Update()
    {
        bool isPressedThisFrame = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            MovePaddleToScreenX(touch.position.x);
            isPressedThisFrame = touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
        }
        else if (Input.GetMouseButton(0))
        {
            MovePaddleToScreenX(Input.mousePosition.x);
            isPressedThisFrame = true;
        }

        if (wasPressedLastFrame && !isPressedThisFrame)
            GameManager.Instance.TryLaunchPendingBall();

        wasPressedLastFrame = isPressedThisFrame;
    }

    private void MovePaddleToScreenX(float screenX)
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenX, 0, 0));
        float limit = ScreenBounds.HalfWidth - halfPaddleWidth - edgeMargin;
        float clampedX = Mathf.Clamp(worldPoint.x, -limit, limit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}