using UnityEngine;

[System.Serializable]
public class PowerupVisual
{
    public PowerupType type;
    public Sprite sprite;
}

public class PowerupSpawner : MonoBehaviour
{
    public static PowerupSpawner Instance;

    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private PowerupVisual[] visuals;

    private void Awake() => Instance = this;

    public void SpawnRandomPowerup(Vector3 position)
    {
        int count = System.Enum.GetValues(typeof(PowerupType)).Length;
        PowerupType randomType = (PowerupType)Random.Range(0, count);

        GameObject obj = Instantiate(powerupPrefab, position, Quaternion.identity);
        obj.GetComponent<PowerupItem>().Init(randomType, GetSpriteFor(randomType));
    }

    private Sprite GetSpriteFor(PowerupType type)
    {
        foreach (var v in visuals)
            if (v.type == type) return v.sprite;

        Debug.LogWarning($"No sprite assigned for {type} in PowerupSpawner.");
        return null;
    }
}