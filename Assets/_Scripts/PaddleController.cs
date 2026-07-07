using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float minX = -4f;
    [SerializeField] private float maxX = 4f;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            MovePaddleToScreenX(touch.position.x);
        }
        else if (Input.GetMouseButton(0)) // lets you test with a mouse in-editor
        {
            MovePaddleToScreenX(Input.mousePosition.x);
        }
    }

    private void MovePaddleToScreenX(float screenX)
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenX, 0, 0));
        float clampedX = Mathf.Clamp(worldPoint.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}