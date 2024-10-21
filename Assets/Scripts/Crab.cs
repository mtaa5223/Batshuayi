using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Crab : MonoBehaviour
{
    public Transform player;  // �÷��̾ Ÿ������ ����
    public Rigidbody rigid;
    private NavMeshAgent nav;
    private bool isSkillActive = false;
    Animator anim;

    public GameObject[] particle;

    public GameObject laserPosition;
    private bool followLaser;
    private Vector3 beforeLaserPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 vector = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(vector);  // �÷��̾ �ٶ󺸰� ȸ��

        if (!isSkillActive)
        {
            StartCoroutine(Skill());
        }

        if (player != null && followLaser)
        {
            laserPosition.transform.LookAt(player.position);
        }
        else if (!followLaser)
        {
            laserPosition.transform.LookAt(beforeLaserPosition);
        }
    }

    IEnumerator Skill()
    {
        isSkillActive = true;

        yield return new WaitForSeconds(4f);

        int randomSkill = Random.Range(1, 2);

        switch (randomSkill)
        {
            case 1:
                Attack1();
                anim.SetTrigger("Attack1");
                break;
        }

        yield return new WaitForSeconds(3f);
        isSkillActive = false;
    }

    public void Attack1()
    {
        followLaser = false;
        particle[0].SetActive(true);
        StartCoroutine(ShootParticleToPlayer());
    }

    IEnumerator ShootParticleToPlayer()
    {
        Vector3 direction = (player.position - particle[0].transform.position).normalized;
        float speed = 10f;

        float elapsedTime = 0f;
        float maxDuration = 2f;

        while (elapsedTime < maxDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        particle[0].SetActive(false);

        followLaser = true;
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
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
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
