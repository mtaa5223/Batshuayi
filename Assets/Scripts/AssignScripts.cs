using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class AssignScripts : MonoBehaviour
{
    private SignalReceiver signalReceiver;
    void Start()
    {
        // SignalReceiver 컴포넌트 가져오기
        if (!TryGetComponent<SignalReceiver>(out signalReceiver))
        {
            Debug.LogError("SignalReceiver 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        UnityEvent ev1 = new UnityEvent();
        UnityEvent ev2 = new UnityEvent();
        
        ev1.AddListener(() => {
            Debug.Log("첫 번째 퀘스트 시작");
            QuestManager.instance.StartQuest();
        });

        ev2.AddListener(() => {
            Debug.Log("두 번째 퀘스트 시작");
            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();
        });

        signalReceiver.ChangeReactionAtIndex(1, ev1);
        signalReceiver.ChangeReactionAtIndex(2, ev2);
    }

    void Update()
    {
        // 필요 시 이후 기능 구현 가능
    }
}
