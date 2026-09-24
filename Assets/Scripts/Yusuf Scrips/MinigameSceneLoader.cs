using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

  
        if (playerController != null) playerController.enabled = false;
        if (playerInteraction != null) playerInteraction.enabled = false;

    
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