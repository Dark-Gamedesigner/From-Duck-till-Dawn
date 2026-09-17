using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ReleaseMinigame : MonoBehaviour
{
    //public Button startMinigame;
    public int price;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (MoneySpawner.Instance.releasemoney <= MoneySpawner.Instance.startMoney ){
            
        }
        else{
            MoneySpawner.Instance.ToLooseMoney(1);
            if (MoneySpawner.Instance.releasemoney >= MoneySpawner.Instance.startMoney){
                
                startMinigame = ;
            }
        }*/
    }
    
    public void OnClickButton(string value){
        //if (MoneySpawner.Instance.releasemoney < MoneySpawner.Instance.startMoney) return;
        
        if (price <= MoneySpawner.Instance.startMoney){
            MoneySpawner.Instance.ToLooseMoney(price);
            SceneManager.LoadScene(value);
        }
        else{
            return;
        }

        
    }
}
