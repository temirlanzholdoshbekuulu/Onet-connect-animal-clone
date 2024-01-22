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
		Destroy(gameObject, 0.3f);

		bool isHorizontal = startPoint.x != endPoint.x;
		float distance = isHorizontal ? Mathf.Abs(startPoint.x - endPoint.x) : Mathf.Abs(startPoint.y - endPoint.y);
		int posCount = Random.Range((int)distance * 5, (int)distance * 8);
		lineRenderer.positionCount = posCount;

		Vector3 direction = (endPoint - startPoint).normalized;
		float step = distance / posCount;

		for (int i = 1; i < posCount - 1; i++)
		{
			Vector3 jitter = new Vector3(Random.Range(-jitterAmount, jitterAmount), Random.Range(-jitterAmount, jitterAmount), 0);
			Vector3 position = startPoint + direction * step * i + jitter;
			lineRenderer.SetPosition(i, position);
		}

		lineRenderer.SetPosition(0, startPoint);
		lineRenderer.SetPosition(posCount - 1, endPoint);
	}
}
