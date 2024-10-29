using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SwordGetScript : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemys;
    [SerializeField] private GameObject[] Sowrds;
    public void GetItem()
    {
        if (QuestManager.instance.QuestID == 3)
        {
            QuestManager.instance.QuestDiscount(0);
            StartCoroutine(StartQuest());

            foreach (GameObject enemy in Enemys)
            {
                enemy.SetActive(true);
            }
        }

        QuestManager.instance.StartQuest();

    }
    IEnumerator StartQuest()
    {
        yield return new WaitForSeconds(2f);
        QuestManager.instance.StartQuest();
    }
}
