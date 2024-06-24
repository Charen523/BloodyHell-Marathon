using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private Transform target;
	private CinemachineVirtualCamera cinemachine;

	private void Awake()
	{
		cinemachine = GetComponent<CinemachineVirtualCamera>();
	}

	public void SetTarget(Transform t)
	{
		target = t;
		cinemachine.Follow = target;
	}
}
