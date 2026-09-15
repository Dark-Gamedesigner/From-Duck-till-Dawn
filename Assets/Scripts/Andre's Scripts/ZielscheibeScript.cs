using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ZielscheibeScript : MonoBehaviour
{
    private float speed = 1.5f;
    private float distance = 4.25f;
    private float start;

    private int _currentHitPoints = 1;

    public static int targets = 12;
    public static UnityEvent TargetHit = new();

    private static UnityEvent GameWinned = new();

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        start = transform.position.z;
    }

    // Update is called once per frame
    void Update(){
        float newPosZ = start + Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosZ);
    }

    public void Hited(int incomingHit = 1 ){
        int wouldBeHit = _currentHitPoints - incomingHit;
        Destroy(gameObject);
        if (Random.value < 0.7f){
            MoneySpawner.GetMoney.Invoke(1);
        }
        targets--;
        TargetHit.Invoke();
        
    }

    
}
