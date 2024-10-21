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
        // SignalReceiver ������Ʈ ��������
        if (!TryGetComponent<SignalReceiver>(out signalReceiver))
        {
            Debug.LogError("SignalReceiver ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        UnityEvent ev1 = new UnityEvent();
        UnityEvent ev2 = new UnityEvent();
        
        ev1.AddListener(() => {
            Debug.Log("ù ��° ����Ʈ ����");
            QuestManager.instance.StartQuest();
        });

        ev2.AddListener(() => {
            Debug.Log("�� ��° ����Ʈ ����");
            QuestManager.instance.QuestDiscount(0);
            QuestManager.instance.StartQuest();
        });

        signalReceiver.ChangeReactionAtIndex(1, ev1);
        signalReceiver.ChangeReactionAtIndex(2, ev2);
    }

    void Update()
    {
        // �ʿ� �� ���� ��� ���� ����
    }
}
