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
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI totalPayText;

    [Header("Game Settings")]
    public float shiftLengthSeconds = 60f;
    public AudioSource cashSound;

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
        if (cashSound != null) cashSound.Play();
        UpdateUI();
    }

    public void AddPenalty(float amount)
    {
        // Deduct directly from our main Earnings
        currentEarnings -= amount;
        UpdateUI();

        // Stop the old animation if it's currently playing, and start a new one
        if (penaltyAnimCoroutine != null) StopCoroutine(penaltyAnimCoroutine);
        penaltyAnimCoroutine = StartCoroutine(SlidePenaltyText(amount));
    }

    IEnumerator SlidePenaltyText(float amount)
    {
        // Set the text to show the exact amount lost
        penaltyText.text = "-$" + amount.ToString("F2");

        float slideSpeed = 5f;
        Vector2 hiddenPos = new Vector2(-500, penaltyTextRect.anchoredPosition.y);
        Vector2 visiblePos = new Vector2(288, penaltyTextRect.anchoredPosition.y);

        // Slide In
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            penaltyTextRect.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, t);
            yield return null;
        }

        // Wait on screen for 2 seconds
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
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndShift()
    {
        shiftActive = false;

        if (currentEarnings <= 0)
        {
            gameOverTitleText.text = "Game Over. You lost!";
            gameOverTitleText.color = Color.black;
        }
        else
        {
            gameOverTitleText.text = "Shift Complete!";
            gameOverTitleText.color = Color.black;
        }

        totalPayText.text = "TOTAL PAY: $" + currentEarnings.ToString("F2");

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        // Unfreeze the game before leaving
        Time.timeScale = 1f;

        // Destroy the MusicManager so it restarts properly on the Main Menu
        if (TavernJukebox.instance != null)
        {
            Destroy(TavernJukebox.instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }
}