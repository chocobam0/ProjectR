using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public string SceneName;
    public void OnStartButtonClicked()
    {
        // GameScene으로 씬 전환
        SceneManager.LoadScene(SceneName);
    }
}