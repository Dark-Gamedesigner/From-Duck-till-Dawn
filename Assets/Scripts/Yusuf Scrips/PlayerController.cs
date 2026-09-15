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
}