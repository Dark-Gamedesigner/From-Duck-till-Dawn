using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Persistenter, zentraler Loader fuer alle Minispiel-Szenen.
/// Gehoert in die Bootstrap-Szene (die als erstes geladen wird) und
/// ueberlebt Szenenwechsel per DontDestroyOnLoad.
///
/// Jede Station im Saloon ruft LoadMinigame(sceneName) auf, statt eine
/// direkte Referenz auf das Minispiel-Objekt zu brauchen - dadurch
/// funktioniert das System fuer beliebig viele Minispiel-Szenen gleich.
/// </summary>
public class MinigameSceneLoader : MonoBehaviour
{
    public static MinigameSceneLoader Instance { get; private set; }

    [Header("Events - z.B. fuer GameManager/EquipmentManager")]
    public UnityEvent<string, bool> OnMinigameFinished; // Szenenname, gewonnen?

    private string currentMinigameScene;
    private PlayerController playerController;
    private PlayerInteraction playerInteraction;
    private Camera playerCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Wird von einer Station im Saloon aufgerufen (z.B. MinigameStation.cs),
    /// wenn der Spieler mit E ein Minispiel startet.
    /// </summary>
    public void LoadMinigame(string sceneName)
    {
        if (!string.IsNullOrEmpty(currentMinigameScene))
        {
            Debug.LogWarning("Es laeuft bereits ein Minispiel - LoadMinigame wird ignoriert.");
            return;
        }

        currentMinigameScene = sceneName;
        FindAndDisablePlayer();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Wird vom jeweiligen Minispiel selbst aufgerufen (z.B. am Ende von
    /// BottleShootingGame.EndGame()), wenn die Runde vorbei ist.
    /// </summary>
    public void FinishCurrentMinigame(bool won)
    {
        if (string.IsNullOrEmpty(currentMinigameScene))
        {
            Debug.LogWarning("FinishCurrentMinigame aufgerufen, aber kein Minispiel aktiv.");
            return;
        }

        string finishedScene = currentMinigameScene;
        SceneManager.UnloadSceneAsync(finishedScene);
        currentMinigameScene = null;

        EnablePlayer();

        OnMinigameFinished?.Invoke(finishedScene, won);
    }

    private void FindAndDisablePlayer()
    {
        // Wird einmalig gesucht, falls noch nicht zwischengespeichert
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
            playerInteraction = playerController != null
                ? playerController.GetComponent<PlayerInteraction>()
                : null;
            playerCamera = playerController != null
                ? playerController.GetComponentInChildren<Camera>()
                : null;
        }

        // Spielerbewegung und -interaktion waehrend des Minispiels abschalten,
        // damit er sich nicht gleichzeitig im Saloon weiterbewegen kann.
        if (playerController != null) playerController.enabled = false;
        if (playerInteraction != null) playerInteraction.enabled = false;

        // Kamera + AudioListener des Spielers deaktivieren, damit die eigene
        // Kamera der Minispiel-Szene die Bildausgabe uebernimmt (sonst gibt
        // es Unity-Warnungen wegen mehrerer aktiver AudioListener).
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
            AudioListener listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = false;
        }
    }

    private void EnablePlayer()
    {
        if (playerController != null) playerController.enabled = true;
        if (playerInteraction != null) playerInteraction.enabled = true;

        if (playerCamera != null)
        {
            playerCamera.enabled = true;
            AudioListener listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = true;
        }
    }
}