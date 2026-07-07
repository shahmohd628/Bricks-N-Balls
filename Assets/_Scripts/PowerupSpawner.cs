using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public static PowerupSpawner Instance;
    [SerializeField] private GameObject powerupPrefab; // just ONE prefab now

    private void Awake() => Instance = this;

    public void SpawnRandomPowerup(Vector3 position)
    {
        int count = System.Enum.GetValues(typeof(PowerupType)).Length;
        PowerupType randomType = (PowerupType)Random.Range(0, count);

        GameObject obj = Instantiate(powerupPrefab, position, Quaternion.identity);
        obj.GetComponent<PowerupItem>().Init(randomType);
    }
}