using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerData : MonoBehaviour
{
	#region SerializeField
	[SerializeField] private CameraManager cameraManager;
	#endregion
	#region Private Field
	private static NetworkPlayerData instance;
	public bool IsViewCondition;
	private Player myPlayer;
	#endregion
	#region Public Elements
	public static NetworkPlayerData Instance
	{
		get { return instance; }
	}
	public Player MyPlayer
	{
		get
		{
			return myPlayer;
		}
		private set
		{
			myPlayer = value;
		}
	}
	#endregion
	#region MonoBehaviour Callbacks
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
	#endregion
	#region Public Methods
	// 자기 플레이어 객체 저장하고, 카메라 타겟으로 지정
	public void SetMyPlayer(Player player)
	{
		if (MyPlayer == null & player.photonView.IsMine)
		{
			MyPlayer = player;
			cameraManager.SetTarget(player.transform);
		}
	}
	#endregion
}
