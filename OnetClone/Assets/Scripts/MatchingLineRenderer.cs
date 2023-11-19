using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingLineRenderer : MonoBehaviour
{
    [SerializeField] float jitterAmount;
    private LineRenderer lineRenderer;
    public Vector3 startPoint;
    public Vector3 endPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Destroy(gameObject,0.3f);
        bool isHorizontal = startPoint.x != endPoint.x;
        float distance = isHorizontal?Mathf.Abs(startPoint.x - endPoint.x):Mathf.Abs(startPoint.y - endPoint.y);
        int posCount = Random.Range((int)distance*5,(int)distance*8);
        lineRenderer.positionCount = posCount;

        
        if(isHorizontal && endPoint.x > startPoint.x)
        {
            for (int i = 1; i < posCount -1; i++)
            {
                lineRenderer.SetPosition(i,new Vector3(startPoint.x + (distance / posCount) * i,startPoint.y + 1 * Random.Range(-jitterAmount,jitterAmount),0));
            }
        }
        else if(isHorizontal && endPoint.x < startPoint.x)
        {
            for (int i = posCount - 1; i > 0; i--)
            {
                lineRenderer.SetPosition(i,new Vector3(startPoint.x - (distance / posCount) * i,startPoint.y + 1 * Random.Range(-jitterAmount,jitterAmount),0));
            }
        }
        else if(isHorizontal ==false && endPoint.y > startPoint.y)
        {
            for (int i = 1; i < posCount -1; i++)
            {
                lineRenderer.SetPosition(i,new Vector3(startPoint.x + 1 * Random.Range(-jitterAmount,jitterAmount),startPoint.y + (distance / posCount) * i,0));
            }
        }
        else if(isHorizontal==false && endPoint.y < startPoint.y)
        {
            for (int i = posCount -1; i > 0; i--)
            {
                lineRenderer.SetPosition(i,new Vector3(startPoint.x + 1 * Random.Range(-jitterAmount,jitterAmount),startPoint.y - (distance / posCount) * i,0));
            }
        }
        lineRenderer.SetPosition(0,startPoint);
        lineRenderer.SetPosition(posCount-1,endPoint);
    }
}
