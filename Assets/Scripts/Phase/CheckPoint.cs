using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Player player;
    [SerializeField]
    private int pointIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out player))
        {
            player.playerlap = RaceManager.Instance.dicPlayer[player.playerlap.playerCode];
            if(player.playerlap.currentPoint == pointIndex - 1)
            {
                RaceManager.Instance.PassedCheckPoint(pointIndex, player.playerlap);
            }
            else if (pointIndex == 0)
            {
                for (int i = 0; i < player.playerlap.checkPoints.Length; i++)
                {
                    if (!player.playerlap.checkPoints[i])
                    {
                        RaceManager.Instance.OnCheckPoint?.Invoke(pointIndex, player.playerlap);
                        return;
                    }
                }
                RaceManager.Instance.OnLastCheckPoint?.Invoke(player.playerlap);
            }       
            else
            {
                Debug.Log("제발 뒤로가지마...");
            }
        }
    }
}
