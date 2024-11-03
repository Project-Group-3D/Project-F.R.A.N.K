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
            questManager.CompleteQuest("Ramasser Gem1");
            EquipItem("Gem1");
        }
        else if (pickableObject.itemName == "Gem2")
        {
            questManager.CompleteQuest("Ramasser Gem2");
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
