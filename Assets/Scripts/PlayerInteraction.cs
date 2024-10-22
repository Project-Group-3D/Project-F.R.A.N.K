using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si tu utilises TextMeshPro

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // Portée de l'interaction
    public LayerMask interactableLayer; // Couche pour les objets interactifs
    public TextMeshProUGUI actionText; // Le texte d'affichage
    
    private Camera playerCamera;
    private PickableObject currentObject;

    void Start()
    {
        playerCamera = Camera.main;
        actionText.gameObject.SetActive(false); // Masquer le texte au début
    }

    void Update()
    {
        // Lancer un rayon depuis la caméra pour détecter un objet à portée
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange, interactableLayer))
        {
            PickableObject pickableObject = hit.collider.GetComponent<PickableObject>();
            if (pickableObject != null)
            {
                currentObject = pickableObject;
                ShowActionText();
                
                // Détecter les actions du joueur (touche "E" pour prendre, "R" pour déposer)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Ajouter l'objet à l'inventaire (fonction à définir dans ton script d'inventaire)
                    AddToInventory(pickableObject);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    // Déposer l'objet
                    DropItem(pickableObject);
                }
            }
        }
        else
        {
            HideActionText(); // Masquer le texte si aucun objet n'est proche
            currentObject = null;
        }
    }

    void ShowActionText()
    {
        actionText.text = "E to take it, R to drop it";
        actionText.gameObject.SetActive(true); // Afficher le texte
    }

    void HideActionText()
    {
        actionText.gameObject.SetActive(false); // Masquer le texte
    }

    void AddToInventory(PickableObject pickableObject)
    {
        // Logique pour ajouter l'objet à l'inventaire
        Debug.Log("Objet ajouté à l'inventaire : " + pickableObject.name);
        HideActionText();
    }

    void DropItem(PickableObject pickableObject)
    {
        // Logique pour déposer l'objet
        Debug.Log("Objet déposé : " + pickableObject.name);
        HideActionText();
    }
}
