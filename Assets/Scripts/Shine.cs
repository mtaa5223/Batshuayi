using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Shine : MonoBehaviour
{
    public GameObject TimeLine;
    public Text ShineID;
    public int count;
    public GameObject Portal;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestManager.instance.QuestID == 8)
        {
            count++;
            QuestManager.instance.QuestDiscount(0);
            ShineID.text = "������� : " + count;   
        }
        if (count == 1)
        {
            QuestManager.instance.questNameText.text = "";
            QuestManager.instance.questDescriptionText.text = "";
            TimeLine.SetActive(true);
        }
    }
}
