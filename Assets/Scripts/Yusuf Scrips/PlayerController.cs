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
    /// kann bei hoher Fallgeschwindigkeit durch duenne oder normalenverkehrte Mesh
    /// Collider hindurchrutschen. Hier wird zusaetzlich per Raycast geprueft, ob die
    /// UNTERKANTE der Spieler-Kapsel im Boden steckt - falls ja, wird der Spieler
    /// exakt auf die Bodenoberflaeche gesetzt.
    ///
    /// Wichtig: transform.position ist NICHT automatisch die Fussposition, sondern
    /// haengt vom "Center" des CharacterControllers ab. Bei Center = (0,0,0) (Unity-
    /// Standard) liegt transform.position in der MITTE der Kapsel - die Fussposition
    /// muss deshalb explizit aus Center und Height berechnet werden.
    /// </summary>
    private void PreventFallingThroughGround()
    {
        // Unterkante der Kapsel in Weltkoordinaten (haengt von Center.y und Height ab)
        float capsuleBottomY = transform.position.y + controller.center.y - (controller.height / 2f);

        Vector3 rayOrigin = new Vector3(transform.position.x, capsuleBottomY + 0.1f, transform.position.z);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 0.1f + groundCheckDistance, groundLayer))
        {
            float penetration = hit.point.y - capsuleBottomY;

            // Positive Penetration = die Kapsel-Unterkante steckt unterhalb der
            // erkannten Bodenoberflaeche -> Spieler wieder hochsetzen.
            if (penetration > 0f)
            {
                Vector3 correctedPosition = transform.position;
                correctedPosition.y += penetration;
                transform.position = correctedPosition;
                velocity.y = 0f;
            }
        }
    }
}