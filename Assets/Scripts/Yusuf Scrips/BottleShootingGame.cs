using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Zentrale Logik fuer das Minispiel "Flaschenabschiessen".
/// Verwaltet Start, Ablauf, Zeitlimit, Trefferzaehler und Erfolg/Misserfolg.
/// Andere Systeme (UI, GameManager/RewardHandler) haengen sich ueber die
/// UnityEvents unten ein, statt dass dieses Script sie direkt kennen muss.
/// </summary>
public class BottleShootingGame : MonoBehaviour
{
    // Singleton-Zugriff, damit Bottle.cs sich einfach zurückmelden kann,
    // ohne dass jede Flasche eine eigene Referenz im Inspector braucht.
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
    [Tooltip("Wartezeit nach Spielende, bevor die Szene entladen wird (Zeit fuer Sieg/Niederlage-UI)")]
    [SerializeField] private float returnDelay = 1.5f;

    private int currentHits = 0;
    private float timeRemaining;
    private bool gameIsActive = false;
    private bool lastResultWon = false;

    private void Awake()
    {
        // Einfache Singleton-Absicherung
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Mehr als eine BottleShootingGame-Instanz in der Szene!");
            return;
        }
        Instance = this;

        // ShootingInput liegt auf dem Player-Objekt in der Saloon-Szene, die
        // additiv geladen im Hintergrund bleibt - deshalb hier zur Laufzeit
        // suchen statt per Inspector zu verknuepfen (Cross-Scene-Referenzen
        // funktionieren im Inspector nicht zuverlaessig).
        shootingInput = FindFirstObjectByType<ShootingInput>();
        if (shootingInput == null)
        {
            Debug.LogWarning("BottleShootingGame: Kein ShootingInput in der Szene gefunden!");
        }
    }

    private void Update()
    {
        if (!gameIsActive) return;

        timeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(timeRemaining);

        if (timeRemaining <= 0f)
        {
            EndGame(won: false);
        }
    }

    /// <summary>
    /// Wird von der Station (z.B. BottleShootingStation.cs) aufgerufen,
    /// wenn der Spieler mit "E" das Minispiel startet.
    /// </summary>
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

    /// <summary>
    /// Wird von Bottle.cs aufgerufen, sobald eine Flasche getroffen wurde.
    /// </summary>
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
        }
        else
        {
            OnGameLost?.Invoke();
        }

        // Kurze Pause, damit der Spieler das Ergebnis (z.B. "Gewonnen!"-UI) noch
        // sieht, bevor die Szene entladen wird und er zurueck im Saloon landet.
        Invoke(nameof(ReturnToSaloon), returnDelay);
    }

    private void ReturnToSaloon()
    {
        MinigameSceneLoader.Instance.FinishCurrentMinigame(lastResultWon);
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