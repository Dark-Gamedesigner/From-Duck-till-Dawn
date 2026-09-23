using UnityEngine;

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