using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private Transform cameraTransform; // die First-Person-Kamera
    [SerializeField] private GameObject interactionPromptUI; // z.B. "E zum Spielen" Text/Panel

    [Header("Einstellungen")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer; // Layer für Stationen/Objekte

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                ShowPrompt(true);
                return;
            }
        }

        currentInteractable = null;
        ShowPrompt(false);
    }

    private void ShowPrompt(bool show)
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(show);
        }
    }
}

// Interface, das jede interagierbare Station (Bar, Bühne, etc.) implementiert
public interface IInteractable
{
    void Interact();
}