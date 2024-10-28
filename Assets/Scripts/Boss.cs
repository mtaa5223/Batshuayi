using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform target;
    public Rigidbody rigid;

    private NavMeshAgent nav;
    private bool isSkillActive = false;
    Animator anim;

    public GameObject[] particle;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update�� �� ������ ȣ���
    void Update()
    {
        anim.SetTrigger("run");

        if (!isSkillActive)
        {
            StartCoroutine(Skill());
        }

    }

    IEnumerator Skill()
    {
        isSkillActive = true;

        yield return new WaitForSeconds(2f);


        int randomSkill = Random.Range(1, 4);

        switch (randomSkill)
        {
            case 1:
                Debug.Log("Skill 1: Charge Attack");
                anim.SetTrigger("Attack1");
                break;
            case 2:
                Debug.Log("Skill 2: Ground Slam");
                anim.SetTrigger("Attack2");
                break;
            case 3:
                Debug.Log("Skill 3: Roar");
                anim.SetTrigger("Attack3");
                break;
        }

        yield return new WaitForSeconds(2f);


        anim.SetBool("run", true);

        isSkillActive = false;
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
        yield return new WaitForSeconds(6f);
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
        yield return new WaitForSeconds(4f);
        particle[0].SetActive(false);
    }
    IEnumerator Paticle2()
    {
        yield return new WaitForSeconds(4f);
        particle[1].SetActive(false);
    }
    IEnumerator Paticle3()
    {
        yield return new WaitForSeconds(4f);
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