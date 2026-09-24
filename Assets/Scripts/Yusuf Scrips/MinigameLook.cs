using UnityEngine;

/// <summary>
/// Kommt NUR auf die Kamera innerhalb einer Minispiel-Szene.
/// Erlaubt dem Spieler, sich per Maus umzuschauen, OHNE sich bewegen zu koennen.
/// Kann per DisableLook()/EnableLook() gesperrt werden, z.B. waehrend ein
/// Win- oder Game-Over-Panel angezeigt wird.
/// </summary>
public class MinigameLook : MonoBehaviour
{
    [Header("Maussteuerung")]
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Blickbegrenzung")]
    [Tooltip("Wie weit hoch/runter geschaut werden kann")]
    [SerializeField] private float verticalLimit = 60f;

    [Tooltip("Wie weit links/rechts geschaut werden kann")]
    [SerializeField] private float horizontalLimit = 70f;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private bool lookEnabled = true;

    private void OnEnable()
    {
        verticalRotation = 0f;
        horizontalRotation = 0f;
        transform.localRotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!lookEnabled) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -horizontalLimit, horizontalLimit);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLimit, verticalLimit);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

   
    public void DisableLook()
    {
        lookEnabled = false;
    }

    public void EnableLook()
    {
        lookEnabled = true;
    }
}