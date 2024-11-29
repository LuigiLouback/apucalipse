using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    // Update is called once per frame
    void Update()
    {
        elapsedTime+=Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime/60);
        int seconds = Mathf.FloorToInt(elapsedTime%60);
        timerText.text=string.Format("{0:00}:{1:00}",minutes,seconds);
    }
}
