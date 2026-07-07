using UnityEngine;

public class BrickGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int columns = 8;
    [SerializeField] private int rows = 5;
    [SerializeField] private float spacingX = 1.1f;
    [SerializeField] private float spacingY = 0.5f;
    [SerializeField] private Vector2 startPosition = new Vector2(-3.85f, 4f);

    private void Start() => SpawnGrid();

    private void SpawnGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = startPosition + new Vector2(col * spacingX, -row * spacingY);
                Instantiate(brickPrefab, pos, Quaternion.identity, transform);
            }
        }
        GameManager.Instance.SetTotalBricks(columns * rows);
    }
}