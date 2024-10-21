using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderCheck : MonoBehaviour
{
    public GameObject cutScene;
    public string moveScene;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Animal") || other.CompareTag("Player")) && QuestManager.instance.isQuestStart)
        {
            if (moveScene != "")
            {
                QuestManager.instance.QuestDiscount(0);
                Debug.Log($"QuestID = {QuestManager.instance.QuestID}");
                SceneManager.LoadScene(moveScene);
                return;
            }
            switch (QuestManager.instance.QuestID)
            {
                case 0:
                case 2:
                    if (other.CompareTag("Animal") && QuestManager.instance.isQuestStart == true)
                    {
                        cutScene.SetActive(true);
                        QuestManager.instance.QuestDiscount(0);
                    }
                    break;
                case 5:
                case 7:
                case 9:
                    if (other.CompareTag("Player") && QuestManager.instance.isQuestStart == true)
                    {
                        cutScene.SetActive(true);
                        QuestManager.instance.QuestDiscount(0);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
  