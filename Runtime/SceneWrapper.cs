using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GexagonVR.SceneWrapper
{
    [CreateAssetMenu(menuName = "GexagonVR/Scene Wrapper", fileName = "Scene Wrapper")]
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

        public AsyncOperation LoadSceneAsyncByName()
        {
            return SceneManager.LoadSceneAsync(_sceneName);
        }

        public void LoadSceneByBuildIndex()
        {
            SceneManager.LoadScene(_sceneBuildIndex);
        }

        public AsyncOperation LoadSceneAsyncByBuildIndex()
        {
            return SceneManager.LoadSceneAsync(_sceneBuildIndex);
        }
    }

    public interface ISceneWrapperSceneSetter
    {
        void SetSceneData(string sceneName, int sceneIndex);
        void SetSceneGUID(string sceneGUID);
    }
}