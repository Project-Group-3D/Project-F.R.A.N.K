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
    public GameObject gemInHand; // Référence à la gemme dans la main

    private string currentItem = ""; // Variable pour suivre l'objet actuellement équipé

    void Start()
    {
        axeInHand.SetActive(false);
        flashlightInHand.SetActive(false);
        keyInHand.SetActive(false);
        gemInHand.SetActive(false);
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
        else if (pickableObject.itemName == "Gem1" || pickableObject.itemName == "Gem2")
        {
            questManager.CompleteQuest("Ramasser la gemme");
            EquipItem("Gem");
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

            if (currentItem == "Axe") axeInHand.SetActive(false);
            else if (currentItem == "Flashlight") flashlightInHand.SetActive(false);
            else if (currentItem == "Key") keyInHand.SetActive(false);
            else if (currentItem == "Gem") gemInHand.SetActive(false);

            item.transform.position = dropPosition;
            item.gameObject.SetActive(true);
            inventory.RemoveItem(slotIndex);
            currentItem = "";
        }
    }

    void EquipItem(string itemName)
    {
        if (currentItem == "Axe") axeInHand.SetActive(false);
        else if (currentItem == "Flashlight") flashlightInHand.SetActive(false);
        else if (currentItem == "Key") keyInHand.SetActive(false);
        else if (currentItem == "Gem") gemInHand.SetActive(false);

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
        else if (itemName == "Gem" && (inventory.itemsInInventory.Exists(item => item.itemName == "Gem1") || inventory.itemsInInventory.Exists(item => item.itemName == "Gem2")))
        {
            gemInHand.SetActive(true); currentItem = "Gem";
        }
    }
}
