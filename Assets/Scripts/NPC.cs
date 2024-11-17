using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool hasInteracted = false; // Variable pour garder une trace de l'interaction

    // Méthode pour interagir avec le NPC
    public void Interact()
    {
        if (!hasInteracted)
        {
            hasInteracted = true; // Marquer le NPC comme interagi
            Debug.Log("Vous interagissez avec " + gameObject.name);
            // Toute autre logique d'interaction, par exemple, démarrer un dialogue
        }
    }

    // Méthode pour vérifier si le NPC a déjà été interagi
    public bool HasInteracted()
    {
        return hasInteracted;
    }
    
}
