using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    // einfache Methode zum Wechseln einer Szene
    public void OnClickButton(string value){
        SceneManager.LoadScene(value);
    }
}
