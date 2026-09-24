using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;


public class BottleShootingGame : MonoBehaviour
{
    public static BottleShootingGame Instance { get; private set; }

    [Header("Runden-Einstellungen")]
    [Tooltip("Alle Flaschen dieser Runde - im Inspector die Bottle-Objekte reinziehen")]
    [SerializeField] private List<Bottle> bottles = new List<Bottle>();

    [Tooltip("Wie viele Flaschen muessen getroffen werden, um zu gewinnen")]
    [SerializeField] private int bottlesNeededToWin = 30;

    [Tooltip("Zeitlimit fuer die Runde in SEKUNDEN (300 = 5 Minuten)")]
    [SerializeField] private float timeLimit = 300f;

    [Tooltip("Name der Saloon-Szene, die beim Zurueckkehren geladen wird")]
    [SerializeField] private string saloonSceneName = "Saloon";

    [Header("Spieler-Steuerung waehrend des Minispiels")]
    private ShootingInput shootingInput;
    private MinigameLook minigameLook;

    [Header("Events - hier haengen sich UI/RewardHandler ein")]
    public UnityEvent OnGameStarted;
    public UnityEvent<int, int> OnScoreChanged;
    public UnityEvent<float> OnTimeChanged;
    public UnityEvent OnGameWon;
    public UnityEvent OnGameLost;

    [Header("UI: Uhr, Zaehler und Panels")]
    [Tooltip("Text-Element, das die verbleibende Zeit anzeigt (mm:ss)")]
    [SerializeField] private TMP_Text timerText;

    [Tooltip("Text-Element, das die Anzahl getroffener Flaschen anzeigt")]
    [SerializeField] private TMP_Text scoreText;

    [Tooltip("Panel, das beim GEWINNEN eingeblendet wird")]
    [SerializeField] private GameObject winPanel;

    [Tooltip("Panel, das bei Zeitablauf (verloren) eingeblendet wird")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Testen")]
    [Tooltip("Nur fuer isoliertes Testen: startet die Runde automatisch. Vor dem finalen Build ausschalten!")]
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

        minigameLook = FindFirstObjectByType<MinigameLook>();
    }

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

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

    private void UpdateScoreDisplay()
    {
        if (scoreText == null) return;
        scoreText.text = currentHits.ToString();
    }

    public void StartGame()
    {
        currentHits = 0;
        timeRemaining = timeLimit;
        gameIsActive = true;

        ResetAllBottles();

        // Maus sperren und Kamerasteuerung freigeben
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (minigameLook != null)
        {
            minigameLook.EnableLook();
        }

        if (shootingInput != null)
        {
            shootingInput.EnableShooting();
        }

        OnGameStarted?.Invoke();
        OnScoreChanged?.Invoke(currentHits, bottlesNeededToWin);
        OnTimeChanged?.Invoke(timeRemaining);

        UpdateScoreDisplay();
        UpdateTimerDisplay();
    }

    public void OnBottleHit(Bottle bottle)
    {
        if (!gameIsActive) return;

        currentHits++;
        OnScoreChanged?.Invoke(currentHits, bottlesNeededToWin);
        UpdateScoreDisplay();

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

        // Kamerasteuerung sperren, damit sich das Bild nicht mehr bewegt
        if (minigameLook != null)
        {
            minigameLook.DisableLook();
        }

        // Maus freigeben, damit die Buttons anklickbar sind
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (won)
        {
            OnGameWon?.Invoke();

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
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

    
    public void BackToSaloonFromWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        ReturnToSaloon();
    }

   
    private void ReturnToSaloon()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(saloonSceneName);
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