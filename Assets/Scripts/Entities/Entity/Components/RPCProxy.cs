using Photon.Pun;
using UnityEngine;

public class RPCProxy : MonoBehaviourPunCallbacks
{
    StateMachine _stateMachine;
    public void Init(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    [PunRPC]
    public void CallFlipSprite(bool isFaceRight)
    {
        Debug.Log(_stateMachine);
        PlayerMovementHandler handler = (_stateMachine.GetState() as PlayerState).MovementHandler;
        if (handler == null) return;

        handler.FlipSprite(isFaceRight);
    }
}
