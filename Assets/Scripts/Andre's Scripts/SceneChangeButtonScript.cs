using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneChangeButtonScript : MonoBehaviour
{
    private float distance = 40f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame){
            ButtonRayCast();
        }
    }

    private void ButtonRayCast(){
        bool hasHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
            distance + Single.Epsilon, LayerMask.GetMask("releaseButton"));
        Debug.DrawLine(transform.position,hit.point, Color.red, 2);
        if (hasHit){
            var ButtonComponent = hit.collider.GetComponent<Button>();
            if (ButtonComponent == null) return;
            ButtonComponent.onClick.Invoke();
        }
    }
}
