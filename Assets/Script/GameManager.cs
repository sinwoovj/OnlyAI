using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public bool IsInitialized { get; private set; }

    public bool IsDevelopingVersion => Application.isEditor || Debug.isDebugBuild;

    [RuntimeInitializeOnLoadMethod]
    static void InitilizedOnLoaded()
    {
        //UIManager.Instance.ClearAllUIs();
        if (Instance.IsDevelopingVersion)
        {
            //UIManager.Instance.ShowUI<TitleUI>();
        }
        else
        {
            //UIManager.Instance.ShowUI<IntroUI>();
        }

        Instance.IsInitialized = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}