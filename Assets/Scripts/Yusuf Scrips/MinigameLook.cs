using UnityEngine;

public class MinigameLook : MonoBehaviour
{
    [Header("Maussteuerung")]
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Blickbegrenzung")]
    [Tooltip("Wie weit hoch/runter geschaut werden kann")]
    [SerializeField] private float verticalLimit = 60f;

    [Tooltip("Wie weit links/rechts geschaut werden kann - haelt den Blick grob auf die Station gerichtet")]
    [SerializeField] private float horizontalLimit = 70f;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    private void OnEnable()
    {
        // Startrotation zuruecksetzen, falls die Szene neu geladen wird
        verticalRotation = 0f;
        horizontalRotation = 0f;
        transform.localRotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -horizontalLimit, horizontalLimit);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLimit, verticalLimit);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
}