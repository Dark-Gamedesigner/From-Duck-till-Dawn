using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
   
    private UnityEvent _gamewin = new ();
    
    // Start setzt am Anfang diese Komponente sowie ein DontDestroy, wenn neu scene geladen oder geswitcht wird
    // und fuegt ein Listener hinzu fur ONTargetHit. 
    void Start(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
            ZielscheibeScript.TargetHit.AddListener(OnTargetHit);
            
        } 
        // Wenn schon eine Instance existiert zerstöre die neue Instanz
        else if (Instance != this){
            Destroy(gameObject);
        }
    }

    // Methode für das Game Win des Minispiels
    void OnTargetHit(){
        // wenn die Instanz nicht null ist 
        if (Instance != null){
            // sowie wenn aus dem ZielscheibenScript die Ziele null und aus selben Script die Ziele kleiner oder gleich 0 sind ...
            // ... wird der Win des Games eingeleitet und die Methode OnGameWin wird aufgerufen. 
            if (ZielscheibeScript.Targets <= 0){       
                        _gamewin.Invoke();
                        OnGameWin();
            }
        }
    }
    
    // Methode zum zurueck Switchen in den Saloon
    public void OnGameWin(){
        SceneManager.LoadScene("Scenes/Saloon");
    }
}