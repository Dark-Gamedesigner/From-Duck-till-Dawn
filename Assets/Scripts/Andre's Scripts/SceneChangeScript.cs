using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public void OnClickButton(string value){
        SceneManager.LoadScene(value);
    }
}
