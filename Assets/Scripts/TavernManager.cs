using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TavernManager : MonoBehaviour
{
    // This creates a "Singleton" so any script can easily talk to it
    public static TavernManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI penaltyText;
    public RectTransform penaltyTextRect;
    public TextMeshProUGUI timerText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // The panel we just made
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI totalPayText; // The final math text

    [Header("Game Settings")]
    public float shiftLengthSeconds = 120f; // 2 minutes
    public AudioSource cashSound; // The Ka-Ching!

    private float currentEarnings = 0f;
    private float timeLeft;
    private bool shiftActive = true;

    private Coroutine penaltyAnimCoroutine;

    void Awake()
    {
        instance = this;
        timeLeft = shiftLengthSeconds;

        if (penaltyTextRect != null)
        {
            penaltyTextRect.anchoredPosition = new Vector2(-500, penaltyTextRect.anchoredPosition.y);
        }

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
            EndShift();
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
        // 1. Deduct directly from our main Earnings! (Allows it to go negative)
        currentEarnings -= amount;
        UpdateUI();

        // 2. Stop the old animation if it's currently playing, and start a new one
        if (penaltyAnimCoroutine != null) StopCoroutine(penaltyAnimCoroutine);
        penaltyAnimCoroutine = StartCoroutine(SlidePenaltyText(amount));
    }

    IEnumerator SlidePenaltyText(float amount)
    {
        // Set the text to show the exact amount lost
        penaltyText.text = "-$" + amount.ToString("F2");

        float slideSpeed = 5f;
        Vector2 hiddenPos = new Vector2(-500, penaltyTextRect.anchoredPosition.y);
        Vector2 visiblePos = new Vector2(50, penaltyTextRect.anchoredPosition.y); // Adjust 50 to move it further right

        // Slide In
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            penaltyTextRect.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, t);
            yield return null;
        }

        // Wait on screen for 2 seconds so the player sees it
        yield return new WaitForSeconds(2f);

        // Slide Out
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            penaltyTextRect.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, t);
            yield return null;
        }
    }

    void UpdateUI()
    {
        earningsText.text = "Earnings: $" + currentEarnings.ToString("F2");
    }

    void UpdateTimerUI()
    {
        // Math to make the timer look like a real clock (00:00)
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndShift()
    {
        shiftActive = false;

        // --- WIN / LOSE LOGIC ---
        if (currentEarnings <= 0)
        {
            gameOverTitleText.text = "Game Over. You lost!";
            gameOverTitleText.color = Color.white; // Turns the text red!
        }
        else
        {
            gameOverTitleText.text = "Shift Complete!";
            gameOverTitleText.color = Color.white; // Or whatever color you used
        }

        totalPayText.text = "TOTAL PAY: $" + currentEarnings.ToString("F2");

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // The button will trigger this method
    public void ReturnToMainMenu()
    {
        // Unfreeze the game before leaving, or the Main Menu will be frozen too!
        Time.timeScale = 1f;

        // Destroy the MusicManager so it restarts properly on the Main Menu
        if (TavernJukebox.instance != null)
        {
            Destroy(TavernJukebox.instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }
}