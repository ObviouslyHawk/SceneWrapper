using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class SceneWrapper : ScriptableObject, ISceneWrapperSceneSetter
{
    [SerializeField] private string _sceneGUID;
    [SerializeField] private string _sceneName;
    [SerializeField] private int _sceneBuildIndex;

    public string SceneGUID => _sceneGUID;
    public string SceneName => _sceneName;
    public int SceneBuildIndex => _sceneBuildIndex;

    void ISceneWrapperSceneSetter.SetSceneData(string sceneName, int sceneIndex)
    {
        _sceneName = sceneName;
        _sceneBuildIndex = sceneIndex;
    }

    public void SetSceneGUID(string sceneGUID)
    {
        _sceneGUID = sceneGUID;
    }

    public void LoadSceneByName()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadSceneAsyncByName()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadSceneByBuildIndex()
    {
        SceneManager.LoadScene(_sceneBuildIndex);
    }

    public void LoadSceneAsyncByBuildIndex()
    {
        SceneManager.LoadScene(_sceneBuildIndex);
    }
}

public interface ISceneWrapperSceneSetter
{
    void SetSceneData(string sceneName, int sceneIndex);
    void SetSceneGUID(string sceneGUID);
}
