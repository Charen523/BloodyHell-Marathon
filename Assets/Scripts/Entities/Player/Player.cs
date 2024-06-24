using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IGrabbable
{
  [field: SerializeField] public PlayerAnimeData AnimeData { get; private set; }
  public GameObject TargetObject { get; set; }

  public PlayerInput Input { get; private set; }

  private PlayerStateMachine _stateMachine;
  public PlayerLap playerlap = new PlayerLap();
  public PhotonView PlayerPhotonView { get; private set; }
  [HideInInspector] public List<Item> PickedUpItems = new List<Item>();

  protected override void Awake()
  {
    base.Awake();
    AnimeData.Initialize();
    Input = GetComponent<PlayerInput>();
    PlayerPhotonView = GetComponent<PhotonView>();

    _stateMachine = new PlayerStateMachine(this);
  }

  private void Start()
  {
    Debug.Log("플레이어 start시작");
    _stateMachine.ChangeState(_stateMachine.IdleState);
    if (photonView.IsMine)
    {
      NetworkPlayerData.Instance.SetMyPlayer(this);
	}
  }

  private void Update()
  {
    _stateMachine.HandleInput();
    _stateMachine.Update();
  }
  private void FixedUpdate()
  {
    _stateMachine.PhysicsUpdate();
  }

  public void OnGrab()
  {
    _stateMachine.Player.Rigid.isKinematic = true;
    _stateMachine.ChangeState(_stateMachine.GroggyState);
  }

  public void OnRelease()
  {
    _stateMachine.Player.Rigid.isKinematic = false;
    _stateMachine.ChangeState(_stateMachine.IdleState);
  }
}
