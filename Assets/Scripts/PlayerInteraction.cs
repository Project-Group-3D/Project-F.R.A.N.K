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
    private QuestManager questManager; // Référence au QuestManager
    public Inventory inventory; // Référence à l'inventaire



    void Start()
    {
        playerCamera = Camera.main;
        actionText.gameObject.SetActive(false); // Masquer le texte au début
        questManager = FindObjectOfType<QuestManager>(); // Trouver le QuestManager
        inventory = FindObjectOfType<Inventory>(); // Trouver l'Inventory
    }

    void Update()
    {
        // Lancer un rayon depuis la caméra pour détecter un objet à portée
    RaycastHit hit;
    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange, interactableLayer))
    {
        PickableObject pickableObject = hit.collider.GetComponent<PickableObject>();
        NPC npc = hit.collider.GetComponent<NPC>(); // Assurez-vous d'avoir un script pour gérer l'NPC

        if (pickableObject != null)
        {
            currentObject = pickableObject;
            ShowActionText("E pour prendre, R pour déposer");
            
            // Actions pour l'objet ramassable
            if (Input.GetKeyDown(KeyCode.E))
            {
                AddToInventory(pickableObject);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                DropItem(pickableObject);
            }
        }
        else if (npc != null && questManager.IsTutorialCompleted() && !npc.HasInteracted())
        {
            ShowActionText("N pour intéragir");

            // Action pour parler à l'NPC
            if (Input.GetKeyDown(KeyCode.N))
            {
                npc.Interact(); // Appel à la méthode Interact de l'objet NPC
                StartQuest("Trouver la gem");
                StartQuest("Ramener la gem"); // Ajouter la quête "Ramasser gem"
            }
        }
        else if (npc != null && questManager.IsTutorialCompleted() && inventory.itemsInInventory.Exists(item => item.itemName == "Gem1"))
{
    ShowActionText("N pour intéragir");
    if (Input.GetKeyDown(KeyCode.N))
    {
        // Compléter la quête
        questManager.CompleteQuest("Ramener la gem");
        
        // Retirer l'objet "Gem1" de l'inventaire
        int indexToRemove = inventory.itemsInInventory.FindIndex(item => item.itemName == "Gem1");
        if (indexToRemove != -1)
        {
            inventory.RemoveItem(indexToRemove); // Supprimer l'objet de l'inventaire
        }

        npc.Interact(); // Appel à la méthode Interact de l'objet NPC
        StartQuest("Trouver la clés");
        StartQuest("Ramener la clés");
    }
}

        else if (npc != null && questManager.IsTutorialCompleted() && inventory.itemsInInventory.Exists(item => item.itemName == "Key"))
{
    ShowActionText("N pour intéragir");
    if (Input.GetKeyDown(KeyCode.N))
    {
        // Compléter la quête
        questManager.CompleteQuest("Ramener la clés");
        
        // Retirer l'objet "Gem1" de l'inventaire
        int indexToRemove = inventory.itemsInInventory.FindIndex(item => item.itemName == "Key");
        if (indexToRemove != -1)
        {
            inventory.RemoveItem(indexToRemove); // Supprimer l'objet de l'inventaire
        }

        npc.Interact(); // Appel à la méthode Interact de l'objet NPC
        StartQuest("Trouver la deuxième gem");
        StartQuest("Ramener la deuxième gem");
    }
}

        else if (npc != null && questManager.IsTutorialCompleted() && inventory.itemsInInventory.Exists(item => item.itemName == "Gem2"))
{
    ShowActionText("N pour intéragir");
    if (Input.GetKeyDown(KeyCode.N))
    {
        // Compléter la quête
        questManager.CompleteQuest("Ramener la deuxième gem");
        
        // Retirer l'objet "Gem1" de l'inventaire
        int indexToRemove = inventory.itemsInInventory.FindIndex(item => item.itemName == "Gem2");
        if (indexToRemove != -1)
        {
            inventory.RemoveItem(indexToRemove); // Supprimer l'objet de l'inventaire
        }

        npc.Interact(); // Appel à la méthode Interact de l'objet NPC
        StartQuest("Echappe toi !");
        
    }
}

    }
    else
    {
        HideActionText(); // Masquer le texte si aucun objet n'est proche
        currentObject = null;
    }
    }

    void ShowActionText(string text)
    {
        actionText.text = text;
        actionText.gameObject.SetActive(true); // Afficher le texte
    }

    void HideActionText()
    {
        actionText.gameObject.SetActive(false); // Masquer le texte
    }

    void AddToInventory(PickableObject pickableObject)
    {
        Debug.Log("Objet ajouté à l'inventaire : " + pickableObject.name);
        HideActionText();
    }

    void DropItem(PickableObject pickableObject)
    {
        Debug.Log("Objet déposé : " + pickableObject.name);
        HideActionText();
    }

    void StartQuest(string questName)
{
    QuestManager questManager = FindObjectOfType<QuestManager>();
    if (questManager != null)
    {
        questManager.AddQuest(questName);
        Debug.Log("Quête ajoutée : " + questName); // Debug log
        HideActionText();
    }
}

}
