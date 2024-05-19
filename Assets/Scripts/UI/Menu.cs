using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject CreditsPanel;
   // public GameObject SettingsPanel;

    void Start() {
        AudioManager.instance.PlayMusic("menu");
    }

    public void QuitGame() => Application.Quit();

    public void ShowCredits() => CreditsPanel.SetActive(true);

    public void HideCredits() => CreditsPanel.SetActive(false);
    
    //public void ShowSettings() => SettingsPanel.SetActive(true);

    //public void HideSettings() => // SettingsPanel.SetActive(false);

    public void GoToMenu() => SCManager.instance.LoadScene("Menu");
}
