using UnityEngine;
using UnityEngine.Events;

public class ZielscheibeScript : MonoBehaviour
{
    private float _speed = 1.5f;
    private float _distance = 4.25f;
    private float _start;

    private int _currentHitPoints = 1;

    public static int Targets = 12;
    public static UnityEvent TargetHit = new();
    
    
    // erzeugt beim Start 12 Ziele
    void Start(){
        _start = transform.position.z;
        Targets = 12;
    }

    // Update Methode fuer das Bewegen der Ziele auf einer Achse
    void Update(){
        float newPosZ = _start + Mathf.PingPong(Time.time * _speed, _distance);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosZ);
    }

    // Methode fuer treffen der Ziele 
    public void Hited(int incomingHit = 1 ){
        
        int wouldBeHit = _currentHitPoints - incomingHit;
        Destroy(gameObject);
        
        // setzt einen zufaelligen Wert
        float getGold = Random.value;
        //Debug.Log(getGold);
        
        // wenn der zufaellige Wert bei 0- unter 70 liegt, gibt es 1 Gold 
        if (getGold < 0.7f){
            MoneySpawner.GetMoney.Invoke(1);
            //Debug.Log(MoneySpawner.GetMoney);
        }
        
        // Danach wird die Ziele Minus gerechnet
        Targets--;
        
        // Danach wir das Event Ziel getroffen eingeleitet
        TargetHit.Invoke();
    }
}