using UnityEngine;

public class BrickGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int columns = 6;
    [SerializeField] private int rows = 7;
    [SerializeField] private float sideMargin = 0.3f;
    [SerializeField] private float topMargin = 1.5f;
    [SerializeField] private float spacingY = 0.55f;

    private void Start() => SpawnGrid();

    private void SpawnGrid()
    {
        float halfW = ScreenBounds.HalfWidth - sideMargin;
        float spacingX = (halfW * 2f) / columns;
        float startX = -halfW + spacingX / 2f;
        float startY = ScreenBounds.HalfHeight - topMargin;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = new Vector2(startX + col * spacingX, startY - row * spacingY);
                Instantiate(brickPrefab, pos, Quaternion.identity, transform);
            }
        }
        GameManager.Instance.SetTotalBricks(columns * rows);
    }
}