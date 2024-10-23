using System;
using UnityEngine;

public class SwordGetScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemys;
    [SerializeField] private GameObject[] Sowrds;
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

        QuestManager.instance.StartQuest();

    }
}
