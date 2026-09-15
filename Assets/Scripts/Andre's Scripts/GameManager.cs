using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Scene switchScene;
    public static GameManager Instance;
    
    private int targets = 12;
    [SerializeField] private GameObject Crosshair;
   
    private UnityEvent Gamewin = new ();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
            ZielscheibeScript.TargetHit.AddListener(OnTargetHit);
        } 
        else if (Instance != this){
            Destroy(gameObject); return;
        }
    }

    public static void SetCrosshair(bool newState){
        Instance.Crosshair.SetActive(newState);
    }

    void OnTargetHit(){
        if (ZielscheibeScript.targets <= 0){
            Gamewin.Invoke();
            OnGameWin();
        }
        
    }

    private void Update(){
      
    }
    public void OnGameWin(){
        SceneManager.LoadScene("Scenes/Saloon");
        SetCrosshair(false);
    }
}
