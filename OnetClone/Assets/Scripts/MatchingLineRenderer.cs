﻿using System.Collections;
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

		Vector3 direction = (endPoint - startPoint).normalized;

		lineRenderer.positionCount = 2;

		lineRenderer.SetPosition(0, startPoint);
		lineRenderer.SetPosition(1, endPoint);
	}
}
