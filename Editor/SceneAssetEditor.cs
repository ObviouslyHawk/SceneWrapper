using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GexagonVR.SceneWrapper
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneAsset))]
    public class SceneAssetEditor : Editor
    {
        SceneAsset[] _sceneAssets;

        private void OnEnable()
        {
            _sceneAssets = targets.Select(x => x as SceneAsset).ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool GUIEnabledState = GUI.enabled;
            GUI.enabled = true;
            if (GUILayout.Button("Create SceneAsset Wrapper"))
            {
                foreach (SceneAsset sceneAsset in _sceneAssets)
                {
                    SceneWrapper asset = CreateInstance<SceneWrapper>();

                    AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var sceneGUID, out long _);
                    ISceneWrapperSceneSetter sceneWrapper = asset;
                    sceneWrapper.SetSceneData(sceneAsset.name, SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(sceneAsset)));
                    sceneWrapper.SetSceneGUID(sceneGUID);
                    asset.name = sceneAsset.name;

                    string filePath = Path.ChangeExtension(AssetDatabase.GetAssetPath(sceneAsset), ".asset");

                    AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(filePath));
                    AssetDatabase.SaveAssets();
                }
            }
            GUI.enabled = GUIEnabledState;
        }
    }
}
