using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public void ResetSzene(string ZieleSchiessen){
        SceneManager.LoadScene(ZieleSchiessen);
    }
}
