using MalbersAnimations.Controller;
using UnityEngine;

public class RespawnMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GetComponent<MRespawner>().player.transform.position;
    }
}
