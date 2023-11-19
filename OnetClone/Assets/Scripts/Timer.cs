using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image image;
    private int roundMinutes = 10;

    void Start()
    {
        roundMinutes *= 60;
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount -=1f / roundMinutes * Time.deltaTime;
        
    }

}
