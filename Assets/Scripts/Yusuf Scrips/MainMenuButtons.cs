using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuButtons : MonoBehaviour
{
    [Tooltip("Exakter Szenenname, der beim Drücken von PLAY geladen wird")]
    [SerializeField] private string gameSceneName = "Saloon";

    [Tooltip("Das Options-Panel, das ein-/ausgeblendet wird")]
    [SerializeField] private GameObject optionsPanel;

    [Tooltip("Alles, was zum normalen Hauptmenü gehoert (Logo, Sign, Buttons, Hintergrund) - wird ausgeblendet solange Options offen ist")]
    [SerializeField] private GameObject mainMenuGroup;

   
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    
    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }

        if (mainMenuGroup != null)
        {
            mainMenuGroup.SetActive(false);
        }
    }

  
    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if (mainMenuGroup != null)
        {
            mainMenuGroup.SetActive(true);
        }
    }

   
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Nur damit man das Verhalten auch im Editor testweise sieht (Log statt Beenden)
        Debug.Log("QuitGame() aufgerufen - im Editor wird das Spiel nicht wirklich beendet.");
#endif
    }
}