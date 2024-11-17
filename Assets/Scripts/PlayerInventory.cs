
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory; // Référence à l'inventaire
    public Camera playerCamera; // Référence à la caméra du FirstPersonController
    public float interactionRange = 3f; // Distance maximale pour interagir avec un objet
    public QuestManager questManager; // Référence au QuestManager pour gérer les quêtes

    public GameObject axeInHand; // Référence à la hache dans la main
    public GameObject flashlightInHand; // Référence à la lampe torche dans la main
    public GameObject keyInHand; // Référence à la clé dans la main
    public GameObject gem1InHand; // Référence à Gem1 dans la main
    public GameObject gem2InHand; // Référence à Gem2 dans la main

    private string currentItem = ""; // Variable pour suivre l'objet actuellement équipé

    void Start()
    {
        axeInHand.SetActive(false);
        flashlightInHand.SetActive(false);
        keyInHand.SetActive(false);
        gem1InHand.SetActive(false);
        gem2InHand.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
            {
                PickableObject pickable = hit.transform.GetComponent<PickableObject>();
                if (pickable != null)
                {
                    if (inventory.AddItem(pickable))
                    {
                        AddToInventory(pickable);
                        pickable.OnPickUp();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            DropCurrentItem(dropPosition);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipInventoryItem(0); // Équipe l'item de la première case
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipInventoryItem(1); // Équipe l'item de la deuxième case
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipInventoryItem(2); // Équipe l'item de la troisième case
        }
    }

    void AddToInventory(PickableObject pickableObject)
    {
        if (pickableObject.itemName == "Axe")
        {
            questManager.CompleteQuest("Récupérer la hache");
            EquipItem("Axe");
        }
        else if (pickableObject.itemName == "Flashlight")
        {
            questManager.CompleteQuest("Ramasser la lampe torche");
            EquipItem("Flashlight");
        }
        else if (pickableObject.itemName == "Key")
        {
            questManager.CompleteQuest("Trouver la clé");
            EquipItem("Key");
        }
        else if (pickableObject.itemName == "Gem1")
        {
            questManager.CompleteQuest("Trouver la gem");
            EquipItem("Gem1");
        }
        else if (pickableObject.itemName == "Gem2")
        {
            questManager.CompleteQuest("Trouver la deuxième gem");
            EquipItem("Gem2");
        }
    }

    void EquipInventoryItem(int slotIndex)
    {
        if (slotIndex < inventory.itemsInInventory.Count)
        {
            PickableObject item = inventory.itemsInInventory[slotIndex];
            EquipItem(item.itemName);
        }
    }

    public void DropCurrentItem(Vector3 dropPosition)
    {
        int slotIndex = inventory.itemsInInventory.FindIndex(item => item.itemName == currentItem);

        if (slotIndex != -1)
        {
            PickableObject item = inventory.itemsInInventory[slotIndex];

            // Désactiver l'objet actuellement équipé
            DeactivateCurrentItem();

            // Déposer l'objet
            item.transform.position = dropPosition;
            item.gameObject.SetActive(true);
            inventory.RemoveItem(slotIndex);

            // Réinitialiser l'objet actuellement équipé
            currentItem = "";
        }
    }

    void EquipItem(string itemName)
    {
        DeactivateCurrentItem();

        if (itemName == "Axe" && inventory.itemsInInventory.Exists(item => item.itemName == "Axe"))
        {
            axeInHand.SetActive(true); currentItem = "Axe";
        }
        else if (itemName == "Flashlight" && inventory.itemsInInventory.Exists(item => item.itemName == "Flashlight"))
        {
            flashlightInHand.SetActive(true); currentItem = "Flashlight";
        }
        else if (itemName == "Key" && inventory.itemsInInventory.Exists(item => item.itemName == "Key"))
        {
            keyInHand.SetActive(true); currentItem = "Key";
        }
        else if (itemName == "Gem1" && inventory.itemsInInventory.Exists(item => item.itemName == "Gem1"))
        {
            gem1InHand.SetActive(true); currentItem = "Gem1";
        }
        else if (itemName == "Gem2" && inventory.itemsInInventory.Exists(item => item.itemName == "Gem2"))
        {
            gem2InHand.SetActive(true); currentItem = "Gem2";
        }
    }

    void DeactivateCurrentItem()
    {

        if (currentItem == "Axe") axeInHand.SetActive(false);
        else if (currentItem == "Flashlight") flashlightInHand.SetActive(false);
        else if (currentItem == "Key") keyInHand.SetActive(false);
        else if (currentItem == "Gem1") gem1InHand.SetActive(false);
        else if (currentItem == "Gem2") gem2InHand.SetActive(false);
    }
}
=======
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory; // Référence à l'inventaire
    public Camera playerCamera; // Référence à la caméra du FirstPersonController
    public float interactionRange = 3f; // Distance maximale pour interagir avec un objet
    public QuestManager questManager; // Référence au QuestManager pour gérer les quêtes

    public GameObject axeInHand; // Référence à la hache dans la main
    public GameObject flashlightInHand; // Référence à la lampe torche dans la main

    private string currentItem = ""; // Variable pour suivre l'objet actuellement équipé

    void Start()
    {
        // Assure-toi que la hache et la lampe torche sont invisibles au début
        axeInHand.SetActive(false);
        flashlightInHand.SetActive(false);
    }

    void Update()
    {
        // Ramasser l'objet avec la touche 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            // Lancer un rayon à partir de la caméra dans la direction où elle regarde
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
            {
                PickableObject pickable = hit.transform.GetComponent<PickableObject>();
                if (pickable != null)
                {
                    // Appelle la fonction pour ajouter l'objet à l'inventaire
                    if (inventory.AddItem(pickable))
                    {
                        AddToInventory(pickable); // Ajouter l'objet à l'inventaire et vérifier les quêtes
                        pickable.OnPickUp(); // Ramasse l'objet
                    }
                }
            }
        }

        // Déposer l'objet avec la touche 'R'
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            DropItem(0, dropPosition); // Dépose l'objet devant la caméra
        }

        // Switch entre hache et lampe torche avec les touches 1 et 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem("Axe"); // Équipe la hache
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem("Flashlight"); // Équipe la lampe torche
        }
    }

    // Nouvelle fonction pour ajouter l'objet et compléter les quêtes si nécessaire
    void AddToInventory(PickableObject pickableObject)
    {
        // Vérifier le nom de l'objet ramassé et compléter la quête si nécessaire
        if (pickableObject.itemName == "Axe")
        {
            questManager.CompleteQuest("Récupérer la hache");
            EquipItem("Axe"); // Équipe automatiquement la hache
        }
        else if (pickableObject.itemName == "Flashlight")
        {
            questManager.CompleteQuest("Ramasser la lampe torche");
            EquipItem("Flashlight"); // Équipe automatiquement la lampe torche
        }
        else if (pickableObject.itemName == "Gem1")
        {
            questManager.CompleteQuest("Trouver la gem");
            EquipItem("Gem1"); // Équipe automatiquement la lampe torche
        }
        else if (pickableObject.itemName == "Key")
        {
            questManager.CompleteQuest("Trouver la clés");
            EquipItem("Key"); // Équipe automatiquement la lampe torche
        }
        else if (pickableObject.itemName == "Gem2")
        {
            questManager.CompleteQuest("Trouver la deuxième gem");
            EquipItem("Gem2"); // Équipe automatiquement la lampe torche
        }
    }

    // Nouvelle fonction pour déposer un objet
    public void DropItem(int slotIndex, Vector3 dropPosition)
    {
        if (slotIndex < inventory.itemsInInventory.Count)
        {
            PickableObject item = inventory.itemsInInventory[slotIndex];

            // Désactiver l'objet équipé si déposé
            if (item.itemName == "Axe")
            {
                axeInHand.SetActive(false);
            }
            else if (item.itemName == "Flashlight")
            {
                flashlightInHand.SetActive(false);
            }

            // Déposer l'objet
            item.transform.position = dropPosition; // Replace l'objet dans la scène
            item.gameObject.SetActive(true); // Réactive l'objet dans la scène
            inventory.RemoveItem(slotIndex); // Enlève l'objet de l'inventaire
        }
    }

    // Fonction pour équiper un objet (hache ou lampe torche)
    void EquipItem(string itemName)
    {
        // Désactiver l'objet actuellement équipé
        if (currentItem == "Axe")
        {
            axeInHand.SetActive(false);
        }
        else if (currentItem == "Flashlight")
        {
            flashlightInHand.SetActive(false);
        }

        // Activer le nouvel objet en fonction de l'itemName
        if (itemName == "Axe")
        {
            if (inventory.itemsInInventory.Exists(item => item.itemName == "Axe")) // Vérifie si l'item est dans l'inventaire
            {
                axeInHand.SetActive(true);
                currentItem = "Axe"; // Met à jour l'item actuellement équipé
            }
        }
        else if (itemName == "Flashlight")
        {
            if (inventory.itemsInInventory.Exists(item => item.itemName == "Flashlight")) // Vérifie si l'item est dans l'inventaire
            {
                flashlightInHand.SetActive(true);
                currentItem = "Flashlight"; // Met à jour l'item actuellement équipé
            }
        }
    }


    void DeactivateCurrentItem()
    {

        if (currentItem == "Axe") axeInHand.SetActive(false);
        else if (currentItem == "Flashlight") flashlightInHand.SetActive(false);
        else if (currentItem == "Key") keyInHand.SetActive(false);
        else if (currentItem == "Gem1") gem1InHand.SetActive(false);
        else if (currentItem == "Gem2") gem2InHand.SetActive(false);
    }


