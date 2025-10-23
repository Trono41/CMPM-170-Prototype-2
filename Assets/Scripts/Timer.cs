using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI tmp;
    float timer = 60f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        tmp.text = "0:" + (int)timer;
    }
}
