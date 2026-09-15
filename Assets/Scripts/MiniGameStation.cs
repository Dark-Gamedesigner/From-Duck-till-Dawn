using UnityEngine;

/// <summary>
/// Generische Station fuer alle vier Minispiele. Kommt auf jede Station
/// im Saloon (Bar, Buehne, Flaschentisch, Findespiel-Bereich) - im Inspector
/// wird nur der jeweilige Szenenname eingetragen.
///
/// Ersetzt eigene Stations-Scripts pro Minispiel (z.B. das alte
/// BottleShootingStation.cs) durch eine einzige wiederverwendbare Klasse.
/// </summary>
public class MinigameStation : MonoBehaviour, IInteractable
{
    [Tooltip("Exakter Name der Minispiel-Szene, wie sie in den Build Settings eingetragen ist")]
    [SerializeField] private string minigameSceneName;

    public void Interact()
    {
        if (string.IsNullOrEmpty(minigameSceneName))
        {
            Debug.LogWarning($"MinigameStation auf {gameObject.name}: kein Szenenname eingetragen!");
            return;
        }

        MinigameSceneLoader.Instance.LoadMinigame(minigameSceneName);
    }
}