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
        // SignalReceiver ������Ʈ ��������
        if (!TryGetComponent<SignalReceiver>(out signalReceiver))
        {
            return;
        }

        UnityEvent ev1 = new UnityEvent();

        ev1.AddListener(() =>
        {
            Debug.Log("ù ����Ʈ ����");
            QuestManager.instance.StartQuest();
        });

        signalReceiver.ChangeReactionAtIndex(1, ev1);
    }
    void Update()
    {
        // �ʿ� �� ���� ��� ���� ����
    }
}
