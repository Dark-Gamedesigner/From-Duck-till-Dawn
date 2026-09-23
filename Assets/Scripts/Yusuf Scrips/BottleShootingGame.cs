using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class BottleShootingGame : MonoBehaviour
{
   
    public static BottleShootingGame Instance { get; private set; }

    [Header("Runden-Einstellungen")]
    [Tooltip("Alle Flaschen dieser Runde - im Inspector die Bottle-Objekte reinziehen")]
    [SerializeField] private List<Bottle> bottles = new List<Bottle>();

    [Tooltip("Wie viele Flaschen muessen getroffen werden, um zu gewinnen")]
    [SerializeField] private int bottlesNeededToWin = 5;

    [Tooltip("Zeitlimit fuer die Runde in Sekunden")]
    [SerializeField] private float timeLimit = 20f;

    [Header("Spieler-Steuerung waehrend des Minispiels")]
    [Tooltip("Wird zur Laufzeit automatisch gefunden, da Player in einer anderen Szene liegt")]
    private ShootingInput shootingInput;

    [Header("Events - hier haengen sich UI/RewardHandler ein")]
    public UnityEvent OnGameStarted;
    public UnityEvent<int, int> OnScoreChanged;   // aktuelle Treffer, Ziel-Anzahl
    public UnityEvent<float> OnTimeChanged;       // verbleibende Zeit
    public UnityEvent OnGameWon;
    public UnityEvent OnGameLost;

    [Header("Rueckkehr zum Saloon")]
    [Tooltip("Wartezeit nach GEWONNENER Runde, bevor die Szene entladen wird (Zeit fuer Sieg-UI)")]
    [SerializeField] private float returnDelay = 1.5f;

    [Header("UI: Uhr und Game Over")]
    [Tooltip("Text-Element, das die verbleibende Zeit anzeigt (mm:ss)")]
    [SerializeField] private TMP_Text timerText;

    [Tooltip("Panel, das bei Zeitablauf (verloren) eingeblendet wird")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Testen")]
    [Tooltip("Nur fuer isoliertes Testen dieser Szene: startet die Runde automatisch, ohne ueber Saloon -> Station zu gehen. Vor dem finalen Build wieder ausschalten!")]
    [SerializeField] private bool autoStartForTesting = false;

    private int currentHits = 0;
    private float timeRemaining;
    private bool gameIsActive = false;
    private bool lastResultWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Mehr als eine BottleShootingGame-Instanz in der Szene!");
            return;
        }
        Instance = this;

        shootingInput = FindFirstObjectByType<ShootingInput>();
        if (shootingInput == null)
        {
            Debug.LogWarning("BottleShootingGame: Kein ShootingInput in der Szene gefunden!");
        }
    }

    private void Start()
    {
        if (autoStartForTesting)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (!gameIsActive) return;

        timeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(timeRemaining);
        UpdateTimerDisplay();

        if (timeRemaining <= 0f)
        {
            EndGame(won: false);
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float displayTime = Mathf.Max(timeRemaining, 0f);
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartGame()
    {
        currentHits = 0;
        timeRemaining = timeLimit;
        gameIsActive = true;

        ResetAllBottles();

        if (shootingInput != null)
        {
            shootingInput.EnableShooting();
        }

        OnGameStarted?.Invoke();
        OnScoreChanged?.Invoke(currentHits, bottlesNeededToWin);
        OnTimeChanged?.Invoke(timeRemaining);
    }

    public void OnBottleHit(Bottle bottle)
    {
        if (!gameIsActive) return;

        currentHits++;
        OnScoreChanged?.Invoke(currentHits, bottlesNeededToWin);

        if (currentHits >= bottlesNeededToWin)
        {
            EndGame(won: true);
        }
    }

    private void EndGame(bool won)
    {
        gameIsActive = false;
        lastResultWon = won;

        if (shootingInput != null)
        {
            shootingInput.DisableShooting();
        }

        if (won)
        {
            OnGameWon?.Invoke();
            Invoke(nameof(ReturnToSaloon), returnDelay);
        }
        else
        {
            OnGameLost?.Invoke();

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }

    public void RetryFromGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        StartGame();
    }

    public void BackToSaloonFromGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        ReturnToSaloon();
    }

    private void ReturnToSaloon()
    {
        if (MinigameSceneLoader.Instance != null)
        {
            MinigameSceneLoader.Instance.FinishCurrentMinigame(lastResultWon);
        }
        else
        {
            Debug.Log("Testlauf beendet (kein MinigameSceneLoader vorhanden - normal beim isolierten Testen).");
        }
    }

    private void ResetAllBottles()
    {
        foreach (Bottle bottle in bottles)
        {
            if (bottle != null)
            {
                bottle.ResetBottle();
            }
        }
    }
}