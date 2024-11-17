using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void start(){
        Time.timeScale = 0; // Arrête le jeu
        Cursor.lockState = CursorLockMode.None; // Si le curseur est caché, affiche-le
        Cursor.visible = true; // Rendre le curseur visible
    }
}
