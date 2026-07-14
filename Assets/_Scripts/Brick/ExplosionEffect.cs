using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.9f; // set to your longest particle system's duration

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();

        Destroy(gameObject, lifetime);
    }
}