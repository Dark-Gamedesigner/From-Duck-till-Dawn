using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButtonScript : MonoBehaviour
{
    private float _distance = 40f;
   
    // Update, wenn E Taste gedrueckt wird der Knopf mittels Raycast gedrueckt
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            ButtonRayCast();
        }
    }
    
    // Methode fuer den Raycast sodass er den Knopf erreicht
    private void ButtonRayCast(){
        bool hasHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
            _distance + Single.Epsilon, LayerMask.GetMask("releaseButton"));
        //Debug.DrawLine(transform.position,hit.point, Color.red, 2);
        
        // wenn der Raycast etwas getroffen hat mit der Komponente Button ...
        if (hasHit){
            var buttonComponent = hit.collider.GetComponent<Button>();
            
            // sie null ist passiert nichts
            if (buttonComponent == null) return;
            
            // ... wird die onclick aufgerufen 
            buttonComponent.onClick.Invoke();
        }
    }
}