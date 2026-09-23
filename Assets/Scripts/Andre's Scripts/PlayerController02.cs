using UnityEngine;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

public class PlayerController02 : MonoBehaviour
{
    public Camera cam;
    public float sensatibility;
    private float _xRotation;

    // Beim erstmaligen Klicken beim Minispiel wird die Maus festgesetzt am Bildschirm 
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update Methode für das Bewegen des Revolvers mittels Mausbewegung 
    private void Update(){
        Vector2 mousePos = Mouse.current.delta.ReadValue();
        
        // Wohin geschaut wird 
        LookInput(new Vector3(mousePos.x, mousePos.y, 0f));
    }

    // Methode für eine einfache Bewegung in der first Person Sicht 
    void LookInput(Vector3 input){
        float mouseX = input.x;
        float mouseY = input.y;

        _xRotation -= (mouseY * Time.deltaTime * sensatibility);
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime *sensatibility));
    }
}
