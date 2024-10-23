using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int count;

    public GameObject TimeLine;

    public Text questText;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        questText.text = "에너지 : " + count;
        if (QuestManager.instance.QuestID == 10)
        {
            TimeLine.SetActive(true);
        }
    }
}
