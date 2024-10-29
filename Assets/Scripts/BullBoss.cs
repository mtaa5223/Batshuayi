using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullBoss : MonoBehaviour
{
    public Transform target;
    public Rigidbody rigid;

    private NavMeshAgent nav;
    private bool isSkillActive = false;
    Animator anim;

    public GameObject[] particle;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update�� �� ������ ȣ���
    void Update()
    {
        nav.SetDestination(target.position);
        anim.SetTrigger("run");

        // �ڷ�ƾ�� ���� ������ ���� ���� Skill �ڷ�ƾ�� ������
        if (!isSkillActive)
        {
            StartCoroutine(Skill());
        }
    }

    IEnumerator Skill()
    {
        isSkillActive = true;

        yield return new WaitForSeconds(2f);

        nav.isStopped = true;

        int randomSkill = Random.Range(1, 4);

        // ���õ� ��ų�� ���� �ٸ� �ൿ ����
        switch (randomSkill)
        {
            case 1:
                Debug.Log("Skill 1: Charge Attack");
                anim.SetTrigger("attack1");
                // Charge attack ��ų ���� �ڵ�
                break;
            case 2:
                Debug.Log("Skill 2: Ground Slam");
                anim.SetTrigger("attack2");
                // Ground slam ��ų ���� �ڵ�
                break;
            case 3:
                Debug.Log("Skill 3: Roar");
                anim.SetTrigger("attack3");
                break;
        }

        yield return new WaitForSeconds(2f); // ��ų ��� �� ��� �ð�

        nav.isStopped = false; // �ٽ� ������ ����
        anim.SetBool("run", true);

        isSkillActive = false; // �ڷ�ƾ�� ����Ǹ� �÷��׸� false�� ����
    }
    public void die()
    {
        anim.SetTrigger("die");
        isSkillActive = true;
        if (QuestManager.instance.QuestDiscount(0))
        {
            QuestManager.instance.StartQuest();
        }
        StartCoroutine(DestroyEnemy());
    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    public void Attack1()
    {
        particle[0].SetActive(true);
        StartCoroutine(Paticle1());
    }
    public void Attack2()
    {
        particle[1].SetActive(true);
        StartCoroutine(Paticle2());
    }
    public void Attack3()
    {
        particle[2].SetActive(true);
        StartCoroutine(Paticle3());
    }
    IEnumerator Paticle1()
    {
        yield return new WaitForSeconds(1f);
        particle[0].SetActive(false);
    }
    IEnumerator Paticle2()
    {
        yield return new WaitForSeconds(1f);
        particle[1].SetActive(false);
    }
    IEnumerator Paticle3()
    {
        yield return new WaitForSeconds(1f);
        particle[2].SetActive(false);
    }
    void FixedUpdate()
    {
        FreezeRotation();
    }

    void FreezeRotation()
    {
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}