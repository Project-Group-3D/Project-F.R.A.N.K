using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Image> slots; // Référence aux images des slots de l'inventaire (ItemImage)
    public List<PickableObject> itemsInInventory; // Liste des objets stockés

    // Fonction pour ajouter un objet à l'inventaire
    public bool AddItem(PickableObject item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].sprite == null) // Si le slot est vide
            {
                slots[i].sprite = item.itemImage; // Met l'image de l'objet dans le slot
                itemsInInventory.Add(item); // Ajoute l'objet à la liste
                return true;
            }
        }
        Debug.Log("Inventaire plein");
        return false; // Si aucun slot disponible
    }

    // Fonction pour retirer un objet de l'inventaire
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < itemsInInventory.Count)
        {
            PickableObject item = itemsInInventory[slotIndex];
            slots[slotIndex].sprite = null; // Vide le slot
            itemsInInventory.RemoveAt(slotIndex); // Retire l'objet de la liste
        }
    }

    // Fonction pour déposer l'objet dans la scène
    public void DropItem(int slotIndex, Vector3 dropPosition)
    {
        if (slotIndex < itemsInInventory.Count)
        {
            PickableObject item = itemsInInventory[slotIndex];
            item.transform.position = dropPosition; // Replace l'objet dans la scène
            item.gameObject.SetActive(true); // Réactive l'objet
            RemoveItem(slotIndex); // Enlève l'objet de l'inventaire
        }
    }
}
