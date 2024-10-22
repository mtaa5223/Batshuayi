using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Shine2 : MonoBehaviour
{
    public GameObject TimeLine;
    public GameObject Portal;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestManager.instance.QuestID == 8)
        {

            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.questNameText.text = "";
            QuestManager.instance.questDescriptionText.text = "";
            TimeLine.SetActive(true);
        }

    }
    public void signal()
    {
        QuestManager.instance.StartQuest();
    }
}


