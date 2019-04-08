using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuSceneControl : MonoBehaviour {

    public void LoadSceneLandscapeLeft(string sceneName) {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        LoadScreenControl.Instance.LoadScene(sceneName);
    }
}