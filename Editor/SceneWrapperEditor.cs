using UnityEditor;
using UnityEngine.SceneManagement;

namespace GexagonVR.SceneWrapper
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneWrapper))]
    public class SceneWrapperEditor : Editor
    {
        private SerializedProperty _gUIProp;
        private SceneWrapper _sceneWrapper;

        void OnEnable()
        {
            _gUIProp = serializedObject.FindProperty("_sceneGUID");
            _sceneWrapper = (SceneWrapper)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            string guid = _gUIProp.stringValue;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guid));
            sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false);
            if (sceneAsset != null)
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var newGUID, out long _))
                {
                    _gUIProp.stringValue = newGUID;

                    int sceneIndex = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(sceneAsset));
                    if (_sceneWrapper.SceneName != sceneAsset.name || _sceneWrapper.SceneBuildIndex != sceneIndex)
                    {
                        ((ISceneWrapperSceneSetter)_sceneWrapper).SetSceneData(sceneAsset.name, sceneIndex);
                    }
                }
            }
            else
            {
                _gUIProp.stringValue = "";
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
