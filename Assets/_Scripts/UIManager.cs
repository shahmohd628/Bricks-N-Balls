using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private void Awake() => Instance = this;

    public void UpdateScore(int score) => scoreText.text = $"Score: {score}";
    public void UpdateLives(int lives) => livesText.text = $"Lives: {lives}";
    public void ShowWinScreen() => winPanel.SetActive(true);
    public void ShowLoseScreen() => losePanel.SetActive(true);
}