using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Texte pour afficher les quêtes en cours
    public TextMeshProUGUI tutorialText; // Texte pour afficher "Tutoriel :"

    private List<string> tutorialQuests = new List<string>(); // Liste des quêtes de tutoriel
    private List<bool> tutorialQuestStatus = new List<bool>(); // Statut des quêtes de tutoriel
    private List<string> mainQuests = new List<string>(); // Liste des quêtes principales
    private List<bool> mainQuestStatus = new List<bool>(); // Statut des quêtes principales

    void Start()
    {
        // Ajoutez les quêtes de tutoriel
        AddTutorialQuest("Récupérer la hache");
        AddTutorialQuest("Ramasser la lampe torche");

        UpdateQuestUI();
    }

    // Ajouter une quête de tutoriel
    public void AddTutorialQuest(string quest)
    {
        tutorialQuests.Add(quest);
        tutorialQuestStatus.Add(false);
        UpdateQuestUI();
    }

    // Ajouter une quête principale
    public void AddQuest(string quest)
    {
        mainQuests.Add(quest);
        mainQuestStatus.Add(false);
        UpdateQuestUI();
    }

    public bool IsTutorialCompleted()
    {
        return tutorialQuestStatus.TrueForAll(status => status);
    }

    // Marquer une quête comme terminée
    public void CompleteQuest(string quest)
    {
        int index = tutorialQuests.IndexOf(quest);
        if (index != -1)
        {
            tutorialQuestStatus[index] = true;
        }
        else
        {
            index = mainQuests.IndexOf(quest);
            if (index != -1)
            {
                mainQuestStatus[index] = true;
            }
        }
        UpdateQuestUI();
    }

    // Mise à jour de l'affichage des quêtes
    void UpdateQuestUI()
    {
        questText.text = "";
        bool allTutorialCompleted = IsTutorialCompleted();

        if (!allTutorialCompleted)
        {
            tutorialText.gameObject.SetActive(true); // Affiche "Tutoriel :" tant que le tutoriel n'est pas terminé

            // Affiche les quêtes de tutoriel non terminées
            for (int i = 0; i < tutorialQuests.Count; i++)
            {
                if (!tutorialQuestStatus[i])
                {
                    questText.text += tutorialQuests[i] + "\n";
                }
            }
        }
        else
        {
            tutorialText.gameObject.SetActive(false); // Masque "Tutoriel :"
        }

        // Affiche les quêtes principales en cours
        for (int i = 0; i < mainQuests.Count; i++)
        {
            if (!mainQuestStatus[i])
            {
                questText.text += mainQuests[i] + "\n";
            }
        }

        questText.gameObject.SetActive(questText.text.Length > 0); // Masque questText si aucune quête n'est à afficher
    }
   
}
