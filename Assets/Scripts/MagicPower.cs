using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MagicPower : MonoBehaviour
{
    public GameObject TimeLine;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.count++;
            QuestManager.instance.QuestDiscount(0);
            Destroy(gameObject);
        }
    }
}
