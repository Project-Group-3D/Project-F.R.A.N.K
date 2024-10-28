using UnityEngine;
using System.Collections;

public class FishingBoatInteraction : MonoBehaviour
{
    public GameObject blackScreen; // Référence à un objet qui affichera un écran noir
    public float fadeDuration = 1f; // Durée du fondu
    private bool isPlayerOnBoat = false; // Indique si le joueur est sur le bateau

    void Start()
    {
        blackScreen.SetActive(false); // Assurez-vous que l'écran noir est caché au début
    }

    void Update()
    {
        // Si le joueur est sur le bateau, afficher l'écran noir
        if (isPlayerOnBoat)
        {
            StartCoroutine(FinishGame());
            isPlayerOnBoat = false; // Assurez-vous de ne pas lancer plusieurs fois
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifiez si le joueur entre en collision avec le bateau
        if (other.CompareTag("Player")) // Assurez-vous que votre joueur a le tag "Player"
        {
            isPlayerOnBoat = true; // Indique que le joueur est sur le bateau
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Réinitialisez la variable si le joueur quitte le bateau
        if (other.CompareTag("Player"))
        {
            isPlayerOnBoat = false;
        }
    }

    private IEnumerator FinishGame()
    {
        blackScreen.SetActive(true); // Afficher l'écran noir
        // Optionnel : ajoutez un fondu ici
        yield return new WaitForSeconds(fadeDuration); // Attendre la durée du fondu

        // Terminez le jeu (par exemple, retour au menu ou à l'écran de fin)
        Application.Quit(); // Pour les builds
        // SceneManager.LoadScene("Menu"); // Pour revenir à un menu si vous êtes en mode développement
    }
}
