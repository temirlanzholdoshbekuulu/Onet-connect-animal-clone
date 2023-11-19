using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] float duration = 0.03f;
    [SerializeField] AnimationCurve curve;

    void Start()
    {
        SelectObjects.current.onTilesMatch +=StartShaking;
    }

    void StartShaking()
    {
        StartCoroutine(Shaking());
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime+=Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}

