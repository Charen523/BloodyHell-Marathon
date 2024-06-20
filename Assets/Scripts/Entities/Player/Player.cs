using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IGrabbable
{
  [field: SerializeField] public PlayerAnimeData AnimeData { get; private set; }
  public GameObject TargetObject { get; set; }

  public PlayerInput Input { get; private set; }

  private PlayerStateMachine _stateMachine;
  public PlayerLap playerlap = new PlayerLap();
  [HideInInspector] public List<Item> PickedUpItem = new List<Item>();

  protected override void Awake()
  {
    base.Awake();
    AnimeData.Initialize();
    Input = GetComponent<PlayerInput>();

    _stateMachine = new PlayerStateMachine(this);
  }

  private void Start()
  {
    _stateMachine.ChangeState(_stateMachine.IdleState);
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
    return;
  }

  public void OnRelease()
  {
    return;
  }
}
