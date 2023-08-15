using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GexagonVR.SceneWrapper
{
    [InitializeOnLoad]
    public class SceneWrapperPostProcessor : AssetPostprocessor
    {
        static SceneWrapperPostProcessor()
        {
            EditorBuildSettings.sceneListChanged += SceneListChanged;
        }

        private static void SceneListChanged()
        {
            string[] sceneWrapperGUIDs = AssetDatabase.FindAssets($"t:{typeof(SceneWrapper).Name}");
            SceneWrapper[] sceneWrapperInstances = new SceneWrapper[sceneWrapperGUIDs.Length];

            for (int i = 0; i < sceneWrapperGUIDs.Length; i++)
            {
                sceneWrapperInstances[i] = AssetDatabase.LoadAssetAtPath<SceneWrapper>(AssetDatabase.GUIDToAssetPath(sceneWrapperGUIDs[i]));
            }

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath(EditorBuildSettings.scenes[i].path, typeof(SceneAsset)) as SceneAsset;
                SceneWrapper sceneWrapperInstance = sceneWrapperInstances.FirstOrDefault(x => x.SceneGUID == EditorBuildSettings.scenes[i].guid.ToString());

                if (sceneWrapperInstance == null)
                {
                    continue;
                }

                if (SceneUtility.GetBuildIndexByScenePath(EditorBuildSettings.scenes[i].path) == sceneWrapperInstance.SceneBuildIndex)
                {
                    continue;
                }

                ((ISceneWrapperSceneSetter)sceneWrapperInstance).SetSceneData(sceneAsset.name, SceneUtility.GetBuildIndexByScenePath(EditorBuildSettings.scenes[i].path));
            }
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0; i < movedAssets.Length; i++)
            {
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath(movedAssets[i], typeof(SceneAsset)) as SceneAsset;

                if (!sceneAsset)
                {
                    return;
                }

                SetSceneWrapperData(sceneAsset);
            }
        }

        private static void SetSceneWrapperData(SceneAsset sceneAsset)
        {
            bool isSceneNameUpdated = false;

            foreach (string sceneWrapperGUID in AssetDatabase.FindAssets($"t:{typeof(SceneWrapper).Name}"))
            {
                SceneWrapper sceneWrapperInstance = AssetDatabase.LoadAssetAtPath<SceneWrapper>(AssetDatabase.GUIDToAssetPath(sceneWrapperGUID));

                if (sceneWrapperInstance == null)
                {
                    continue;
                }

                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out string changedSceneGUID, out long _);
                if (sceneWrapperInstance.SceneGUID == changedSceneGUID)
                {
                    ((ISceneWrapperSceneSetter)sceneWrapperInstance).SetSceneData(sceneAsset.name, sceneWrapperInstance.SceneBuildIndex);
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(sceneWrapperInstance), sceneAsset.name);
                    isSceneNameUpdated = true;
                    break;
                }
            }

            if (isSceneNameUpdated)
            {
                EditorApplication.update += DelayedRefreshOnSceneNameChanged;
            }
        }

        private static void DelayedRefreshOnSceneNameChanged()
        {
            AssetDatabase.Refresh();
            EditorApplication.update -= DelayedRefreshOnSceneNameChanged;
        }
    }
}
