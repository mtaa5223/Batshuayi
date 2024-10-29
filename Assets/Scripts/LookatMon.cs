using UnityEngine;

public class LookatMon : MonoBehaviour
{
    public Transform target;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
