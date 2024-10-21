using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
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


    void Update()
    {
        nav.SetDestination(target.position);


        if (!isSkillActive)
        {
            StartCoroutine(Skill());
        }
    }

    IEnumerator Skill()
    {
        isSkillActive = true;

        yield return new WaitForSeconds(5f);



        int randomSkill = Random.Range(1, 3);


        switch (randomSkill)
        {
            case 1:
                Attack1();
                anim.SetTrigger("Attack1");
                break;
            case 2:
                Attack2();
                anim.SetTrigger("Attack2");
                break;

        }

        yield return new WaitForSeconds(3f);
        isSkillActive = false;
    }
    public void die()
    {

        anim.SetTrigger("die");
        nav.isStopped = true;
        isSkillActive = true;

        if (QuestManager.instance.QuestDiscount(0))
        {
            QuestManager.instance.StartQuest();
        }
        StartCoroutine(DestroyEnemy());

    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2f);
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

    IEnumerator Paticle1()
    {
        yield return new WaitForSeconds(3f);
        particle[0].SetActive(false);

    }
    IEnumerator Paticle2()
    {
        yield return new WaitForSeconds(3f);
        particle[1].SetActive(false);
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