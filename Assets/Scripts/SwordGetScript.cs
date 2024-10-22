using UnityEngine;

public class SwordGetScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemys;
    public GameObject Portal;
    public void GetItem()
    {
        if (QuestManager.instance.QuestID == 3)
        {
            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();

            foreach (GameObject enemy in Enemys)
            {
                enemy.SetActive(true);
            }
        }
        if (QuestManager.instance.QuestID == 10)
        {
            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();

            Portal.SetActive(true);
        }
    }
}
