using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace GexagonVR.SceneWrapper
{
    public class SceneWrapperPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 69;

        public void OnPreprocessBuild(BuildReport report)
        {
            SetSceneWrapperData();
        }

        private static void SetSceneWrapperData()
        {
            foreach (var sceneWrapperGUID in AssetDatabase.FindAssets($"t:{typeof(SceneWrapper).Name}"))
            {
                SceneWrapper sceneWrapperInstance = AssetDatabase.LoadAssetAtPath<SceneWrapper>(AssetDatabase.GUIDToAssetPath(sceneWrapperGUID));
                if (sceneWrapperInstance == null)
                {
                    continue;
                }

                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(sceneWrapperInstance.SceneGUID));
                if (sceneAsset == null)
                {
                    continue;
                }

                int sceneIndex = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(sceneAsset));
                if (sceneWrapperInstance.SceneName != sceneAsset.name || sceneWrapperInstance.SceneBuildIndex != sceneIndex)
                {
                    ((ISceneWrapperSceneSetter)sceneWrapperInstance).SetSceneData(sceneAsset.name, sceneIndex);
                }
            }
        }
    }

}
