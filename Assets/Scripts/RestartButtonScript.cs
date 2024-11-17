using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour
{
    void Start()
{
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
}

public void RestartGame()
{
    // Débloquer le curseur avant de charger la scène pour éviter sa disparition
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Charger la scène
    SceneManager.LoadScene("MainMenu");
}



}
