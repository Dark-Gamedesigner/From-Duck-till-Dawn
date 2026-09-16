using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MoneySpawner : MonoBehaviour
{
    public static UnityEvent<int> GetMoney = new();
    public static UnityEvent<int> looseMoney = new();
    public static UnityEvent<int> ChangeMoney = new();

    public static MoneySpawner Instance{ private set; get; }

    public int startMoney{ private set; get; } = 1;
    public int releasemoney{ private set; get; }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        looseMoney.AddListener(ToLooseMoney);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame){
            GetMoney.Invoke(1);
        }
    }

    private void ToGetMoney(int amount){
        startMoney += amount;
        ChangeMoney.Invoke(startMoney);
        //looseMoney.Invoke(startMoney);
    }

    public void ToLooseMoney(int amount){
        startMoney -= amount;
        ChangeMoney.Invoke(startMoney);
    }
}
