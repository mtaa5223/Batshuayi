using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject gameOverUI;
    private float respawnTime = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverUI.activeSelf)
        {
            respawnTime -= Time.deltaTime;
            text.text = string.Format("{0}초 후 부활합니다", Mathf.FloorToInt(respawnTime));
            if (respawnTime <= 0)
            {
                gameOverUI.SetActive(false);
            }
        }
    }

    public void ShowGameOver()
    {
        respawnTime = 5;
        gameOverUI.SetActive(true);
    }
}
