using TMPro;
using UnityEngine;

public class MoneyUiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        MoneySpawner.ChangeMoney.AddListener(UpdateGetMoney);
        UpdateGetMoney(MoneySpawner.Instance.startMoney);
    }

    private void UpdateGetMoney(int newGetMoney){
        GetComponent<TextMeshProUGUI>().text = newGetMoney.ToString();
    }
}
