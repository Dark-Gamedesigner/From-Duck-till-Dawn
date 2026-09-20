using UnityEngine;
using UnityEngine.SceneManagement;

public class ReleaseMinigame : MonoBehaviour
{
    [SerializeField]
    private int price;
  
    // Einfache Methode damit Geld abgezogen wird beim Anklicken des Knopfes, um das Minispiel zu starten
    public void OnClickButton(string value){
        if (price <= MoneySpawner.Instance.StartMoney){
            MoneySpawner.Instance.ToLooseMoney(price);
            SceneManager.LoadScene(value);
        }
    }
}