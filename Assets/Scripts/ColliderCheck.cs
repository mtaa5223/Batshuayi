using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ColliderCheck : MonoBehaviour
{
    public GameObject cutScene;
    public string moveScene;
    public TextMeshProUGUI loadingText;
    public RectTransform portalEffect;

    private void Start()
    {

    }

    public void LoadAnotherScene()
    {
        portalEffect.gameObject.SetActive(true);
        portalEffect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic).onComplete = () =>
        {
            loadingText.gameObject.SetActive(true);
            StartCoroutine(LoadSceneCoroutine());
        };
    }

    IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(moveScene);
        while (!operation.isDone)
        {
            loadingText.text = string.Format("Loading... ({0:#00}%)", operation.progress * 100);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Animal") || other.CompareTag("Player")) && QuestManager.instance.isQuestStart)
        {
            if (moveScene != "")
            {
                QuestManager.instance.QuestDiscount(0);
                Debug.Log($"QuestID = {QuestManager.instance.QuestID}");
                LoadAnotherScene();
            }
            switch (QuestManager.instance.QuestID)
            {
                case 0:
                case 2:
                    if (other.CompareTag("Animal") && QuestManager.instance.isQuestStart == true)
                    {
                        cutScene.SetActive(true);
                        QuestManager.instance.QuestDiscount(0);
                    }
                    break;
                case 5:
                case 7:
                case 9:
                case 11:
                    if (other.CompareTag("Player") && QuestManager.instance.isQuestStart == true)
                    {
                        cutScene.SetActive(true);
                        QuestManager.instance.QuestDiscount(0);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
