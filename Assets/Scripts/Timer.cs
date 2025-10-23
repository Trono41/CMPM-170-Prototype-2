using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI tmp;
    public float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if ((int)timer % 60 < 10)
        {
            tmp.text = (int)timer / 60 + ":0" + (int)timer % 60;
        } else
        {
            tmp.text = (int)timer / 60 + ":" + (int)timer % 60;
        }
    }
}
