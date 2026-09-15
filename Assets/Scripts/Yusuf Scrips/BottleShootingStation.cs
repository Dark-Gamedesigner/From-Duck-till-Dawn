using UnityEngine;

/// <summary>
/// Sitzt auf der Station im Saloon (z.B. dem Tisch mit den Flaschen).
/// Verbindet das allgemeine Interaktionssystem (PlayerInteraction.cs, IInteractable)
/// mit dem konkreten Minispiel BottleShootingGame.
/// </summary>
public class BottleShootingStation : MonoBehaviour, IInteractable
{
    [Tooltip("Referenz auf das Minispiel, das gestartet werden soll")]
    [SerializeField] private BottleShootingGame bottleShootingGame;

    /// <summary>
    /// Wird von PlayerInteraction.cs aufgerufen, wenn der Spieler
    /// vor dieser Station steht und E drueckt.
    /// </summary>
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