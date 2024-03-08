using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PathRenderer : MonoBehaviour
{
	[SerializeField] private GameObject pointDotPrefab;
	private LineRenderer lineRenderer;
	public Vector3 startPoint;
	public Vector3 endPoint;

	void Start()
	{
		RenderLine(startPoint,endPoint);
	}
	void RenderLine(Vector3 startPoint,Vector3 endPoint)
	{
		lineRenderer = GetComponent<LineRenderer>();
		Destroy(gameObject, 0.3f);

		Vector3 direction = (endPoint - startPoint).normalized;

		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, startPoint);
		lineRenderer.SetPosition(1, endPoint);

		Instantiate(pointDotPrefab,startPoint + new Vector3(0,0,-1),Quaternion.identity,gameObject.transform);		
		Instantiate(pointDotPrefab,endPoint + new Vector3(0,0,-1),Quaternion.identity,gameObject.transform);		
	}
}
