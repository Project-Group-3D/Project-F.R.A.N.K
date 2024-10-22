using System.Collections.Generic;
using UnityEngine;
using TMPro; // Si tu utilises TextMeshPro

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Le texte pour afficher les quêtes
    private List<string> quests = new List<string>(); // Liste des quêtes
    private List<bool> questStatus = new List<bool>(); // Statut des quêtes (complétées ou non)

    void Start()
    {
        // Ajouter les quêtes initiales
        AddQuest("Récupérer la hache");
        AddQuest("Ramasser la lampe torche");

        UpdateQuestUI(); // Mettre à jour l'interface utilisateur
    }

    // Ajouter une quête
    public void AddQuest(string quest)
    {
        quests.Add(quest);
        questStatus.Add(false); // Par défaut, la quête n'est pas encore complétée
        UpdateQuestUI();
    }

    // Marquer une quête comme terminée
    public void CompleteQuest(string quest)
    {
        int index = quests.IndexOf(quest);
        if (index != -1)
        {
            questStatus[index] = true;
            UpdateQuestUI();
        }
    }

    // Mettre à jour le texte des quêtes
    void UpdateQuestUI()
    {
        questText.text = "";
        bool allCompleted = true;

        for (int i = 0; i < quests.Count; i++)
        {
            if (questStatus[i])
            {
                // Si la quête est terminée, on la barre
                questText.text += "<s>" + quests[i] + "</s>\n";
            }
            else
            {
                // Si la quête n'est pas encore terminée, on l'affiche normalement
                questText.text += quests[i] + "\n";
                allCompleted = false; // Si une quête n'est pas complétée, on garde le panneau visible
            }
        }

        // Si toutes les quêtes sont terminées, on masque le panneau
        if (allCompleted)
        {
            questText.gameObject.SetActive(false); // Masquer le texte
        }
    }
}
