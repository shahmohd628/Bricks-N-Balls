using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("BottomTrigger: GameManager.Instance is null — is a GameManager object in the scene?");
            return;
        }

        GameManager.Instance.LoseBall(other.gameObject);
    }
}