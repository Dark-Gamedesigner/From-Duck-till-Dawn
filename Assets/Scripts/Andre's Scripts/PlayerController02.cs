using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController02 : MonoBehaviour
{
    public Camera cam;
    public float sensatibility;
    private float xRotation;

    private void Update(){
        Vector2 mousePos = Mouse.current.delta.ReadValue();
        LookInput(new Vector3(mousePos.x, mousePos.y, 0f));
    }

    void LookInput(Vector3 input){
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime * sensatibility);
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime *sensatibility));
    }
}
