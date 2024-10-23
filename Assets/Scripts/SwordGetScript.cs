using System;
using UnityEngine;

public class SwordGetScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemys;
    [SerializeField] private GameObject[] Sowrds;
    public GameObject TimeLine;

    public void GetItem()
    {
        if (QuestManager.instance.QuestID == 3)
        {
            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();
            foreach (GameObject sword in Sowrds)
            {
                sword.SetActive(false);
            }

            foreach (GameObject enemy in Enemys)
            {
                enemy.SetActive(true);
            }
        }
        if (QuestManager.instance.QuestID == 9)
        {

            foreach (GameObject sword in Sowrds)
            {
                sword.SetActive(false);
            }
            return;

            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();
            TimeLine.SetActive(true);

        }
    }
}
