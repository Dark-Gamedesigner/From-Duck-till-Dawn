using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Maussteuerung")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float lookXLimit = 80f; // wie weit man nach oben/unten schauen kann

    [Header("Boden-Erkennung (verhindert Durchfallen)")]
    [Tooltip("Zusaetzlicher Raycast nach unten, damit der Spieler nicht durch duenne Box Collider faellt")]
    [SerializeField] private LayerMask groundLayer = ~0; // standardmaessig alle Layer
    [SerializeField] private float groundCheckDistance = 0.3f;

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Maus in der Mitte fixieren
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        PreventFallingThroughGround();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Spieler horizontal drehen (links/rechts)
        transform.Rotate(Vector3.up * mouseX);

        // Kamera vertikal kippen (hoch/runter), begrenzt damit man sich nicht überschlägt
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");      // W/S

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Einfache Schwerkraft, damit der Spieler am Boden bleibt
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Zusaetzliche Absicherung gegen "Tunneling": Der eingebaute CharacterController
    /// kann bei hoher Fallgeschwindigkeit durch duenne Box Collider hindurchrutschen,
    /// weil er die Kollision nur zwischen den einzelnen Bewegungsschritten prueft.
    /// Hier wird zusaetzlich per Raycast nach unten geprueft, ob unter dem Spieler
    /// Boden ist - falls er tiefer steht als der erkannte Boden, wird er wieder
    /// daraufgesetzt und die Fallgeschwindigkeit zurueckgesetzt.
    /// </summary>
    private void PreventFallingThroughGround()
    {
        // Startpunkt etwas oberhalb der Fuesse, damit der Ray nicht direkt im
        // Collider des Spielers selbst startet.
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, controller.height / 2f + groundCheckDistance, groundLayer))
        {
            float distanceToGround = transform.position.y - hit.point.y;

            // Wenn der Spieler unterhalb der eigentlichen Standflaeche "steckt"
            // (durchgefallen), wird er zurueck auf die Oberflaeche gesetzt.
            if (distanceToGround < 0f)
            {
                Vector3 correctedPosition = transform.position;
                correctedPosition.y = hit.point.y;
                transform.position = correctedPosition;
                velocity.y = 0f;
            }
        }
    }
}