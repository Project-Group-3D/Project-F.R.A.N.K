using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        // Remplacez "NomDeVotreScene" par le nom exact de la scène que vous voulez charger
        SceneManager.LoadScene("SampleScene");
    }
}
