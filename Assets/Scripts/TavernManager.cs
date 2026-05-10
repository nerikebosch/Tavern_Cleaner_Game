using UnityEngine;
using TMPro;

public class TavernManager : MonoBehaviour
{
    // This creates a "Singleton" so any script can easily talk to it
    public static TavernManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI penaltyText;
    public TextMeshProUGUI timerText;

    [Header("Game Settings")]
    public float shiftLengthSeconds = 120f; // 2 minutes
    public AudioSource cashSound; // The Ka-Ching!

    private float currentEarnings = 0f;
    private float currentPenalties = 0f;
    private float timeLeft;
    private bool shiftActive = true;

    void Awake()
    {
        instance = this;
        timeLeft = shiftLengthSeconds;
        UpdateUI();
    }

    void Update()
    {
        if (!shiftActive) return;

        // Count down the timer
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            shiftActive = false;
            // TODO: Later we will pop up the "Shift Over" screen here!
        }

        UpdateTimerUI();
    }

    public void AddMoney(float amount)
    {
        currentEarnings += amount;
        if (cashSound != null) cashSound.Play(); // Ka-Ching!
        UpdateUI();
    }

    public void AddPenalty(float amount)
    {
        currentPenalties += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        earningsText.text = "Earnings: $" + currentEarnings.ToString("F2");
        penaltyText.text = "Damages: -$" + currentPenalties.ToString("F2");
    }

    void UpdateTimerUI()
    {
        // Math to make the timer look like a real clock (00:00)
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}