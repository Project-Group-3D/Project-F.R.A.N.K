using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory; // Référence à l'inventaire
    public Camera playerCamera; // Référence à la caméra du FirstPersonController
    public float interactionRange = 3f; // Distance maximale pour interagir avec un objet

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
                    if (inventory.AddItem(pickable))
                    {
                        pickable.OnPickUp(); // Ramasse l'objet
                    }
                }
            }
        }

        // Déposer l'objet avec la touche 'Q'
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            inventory.DropItem(0, dropPosition); // Dépose l'objet devant la caméra
        }
    }
}
