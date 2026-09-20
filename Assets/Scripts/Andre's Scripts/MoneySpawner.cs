using UnityEngine;
using UnityEngine.Events;

public class MoneySpawner : MonoBehaviour
{
    public static UnityEvent<int> GetMoney = new();
    public static UnityEvent<int> LooseMoney = new();
    public static UnityEvent<int> ChangeMoney = new();

    public static MoneySpawner Instance{ private set; get; }

    public int StartMoney{ private set; get; } = 1;
    
    // Beim Start wird ein DontDestroy beim Laden eingerichtet. Außerdem, wenn die Instanz schon existiert wird eine neue zerstört
    // Danach wird ein Listener für das bekommen und verlieren von Geld eingerichtet.
    void Start()
    {
        DontDestroyOnLoad(this);
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(this);
            return;
        }
        GetMoney.AddListener(ToGetMoney);
        LooseMoney.AddListener(ToLooseMoney);
    }
    
    // Methode fuer das Bekommen von Gold 
    private void ToGetMoney(int amount){
        StartMoney += amount;
        ChangeMoney.Invoke(StartMoney);
    }

    // Methode fuer das verlieren von Gold
    public void ToLooseMoney(int x){
        StartMoney -= x;
        ChangeMoney.Invoke(StartMoney);
    }
}