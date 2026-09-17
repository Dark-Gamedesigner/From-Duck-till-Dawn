using UnityEngine;

public class BottleShootingStation : MonoBehaviour, IInteractable
{
    [Tooltip("Referenz auf das Minispiel, das gestartet werden soll")]
    [SerializeField] private BottleShootingGame bottleShootingGame;


    public void Interact()
    {
        if (bottleShootingGame == null)
        {
            Debug.LogWarning("BottleShootingStation: Keine BottleShootingGame-Referenz gesetzt!");
            return;
        }

        bottleShootingGame.StartGame();
    }
}