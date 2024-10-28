using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class Ep3Assign : MonoBehaviour
{
    private SignalReceiver signalReceiver;
    void Start()
    {
        // SignalReceiver 컴포넌트 가져오기
        if (!TryGetComponent<SignalReceiver>(out signalReceiver))
        {
            return;
        }

        UnityEvent ev1 = new UnityEvent();

        ev1.AddListener(() =>
        {
            Debug.Log("첫 퀘스트 시작");
            QuestManager.instance.StartQuest();
        });

        signalReceiver.ChangeReactionAtIndex(1, ev1);
    }
    void Update()
    {
        // 필요 시 이후 기능 구현 가능
    }
}
