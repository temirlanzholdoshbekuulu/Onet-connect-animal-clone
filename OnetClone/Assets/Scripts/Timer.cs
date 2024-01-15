using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image image;
    private int minutesInRound = 10;

    void Start()
    {
        minutesInRound *= 60;
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount -=1f / minutesInRound * Time.deltaTime;
        
    }

}
