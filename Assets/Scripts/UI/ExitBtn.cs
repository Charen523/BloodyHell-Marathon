using UnityEngine;

public class ExitBtn : MonoBehaviour
{
    public void ApplicationExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}