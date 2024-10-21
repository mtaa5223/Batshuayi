using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager »ç¿ë
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public int QuestID;
    public bool isQuestStart;
    public QuestData[] questList;

    public Text questNameText;
    public Text questDescriptionText;

    public GameObject Wolf;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        FindWolf();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindWolf();

        if (scene.name == "Chungha")
        {
            StartQuest();
        }
    }

    private void FindWolf()
    {
        if (Wolf == null)
        {
            Wolf = GameObject.Find("Wolf");
        }
    }


    public QuestData GetQuestByID(int id)
    {
        foreach (QuestData quest in questList)
        {
            if (quest.QuestID == id)
            {
                return quest;
            }
        }
        return default;
    }

    public void StartQuest()
    {
        WolfMove(true);
        isQuestStart = true;

        QuestData currentQuest = GetQuestByID(QuestID);

        if (currentQuest.QuestName != null)
        {
            questNameText.text = currentQuest.QuestName;
            questDescriptionText.text = currentQuest.QuestDescription;
        }
    }

    public void EndQuest()
    {
        bool isQuestEnd = true;

        foreach (int questCount in GetQuestByID(QuestID).QuestCounts)
        {
            if (questCount > 0)
            {
                isQuestEnd = false;
            }
        }

        if (isQuestEnd)
        {
            isQuestStart = false;

            questNameText.text = "";
            questDescriptionText.text = "";
            QuestID += 1;
            WolfMove(false);
        }
    }

    public bool QuestDiscount(int questCountNum)
    {
        GetQuestByID(QuestID).QuestCounts[questCountNum]--;
        EndQuest();
        if (GetQuestByID(QuestID - 1).QuestCounts[questCountNum] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void WolfMove(bool isMove)
    {
        if (Wolf != null)
        {
            Wolf.GetComponent<MalbersInput>().enabled = isMove;
        }

    }
}

[System.Serializable]
public struct QuestData
{
    public int QuestID;
    public string QuestName;
    public string QuestDescription;

    public int[] QuestCounts;
}