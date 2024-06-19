using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public bool isPlaying = false;
    public bool isFirst = false;
    public List<Player> players = new List<Player>();
    [SerializeField]
    private int waitTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalIn(Player player)
    {

    }

    private IEnumerator CountDown()
    {
        for (int i = waitTime; i > 0; i--)
        {
            yield return new WaitForSeconds(i);
        }
    }
}
