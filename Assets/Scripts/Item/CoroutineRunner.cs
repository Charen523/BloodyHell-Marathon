using System.Collections;
using UnityEngine;

public class CoroutineRunner : Singleton<CoroutineRunner>
{
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
