using TMPro;
using UnityEngine;

public class MoneyUiController : MonoBehaviour
{
    // Beim Start wird Dont Destroy eingerichtet. Außerdem ein Listener fuer das Updaten des Goldes und die Methode ...
    // ... für die Anzeige wird aufgerufen
    void Start()
    {
        DontDestroyOnLoad(this);
        MoneySpawner.ChangeMoney.AddListener(UpdateGetMoney);
        UpdateGetMoney(MoneySpawner.Instance.StartMoney);
    }

    // Methoden für das Anzeigen des aktuellen Goldes (Zahl) als Text
    private void UpdateGetMoney(int newGetMoney){
        GetComponent<TextMeshProUGUI>().text = newGetMoney.ToString();
    }
}