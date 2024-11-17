using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public string itemName; // Nom de l'objet
    public Sprite itemImage; // Image de l'objet dans l'inventaire

    // Fonction qui sera appelée pour ramasser l'objet
    public void OnPickUp()
    {
        // Désactive l'objet dans la scène, mais il sera stocké en mémoire
        gameObject.SetActive(false);
    }

    
}

