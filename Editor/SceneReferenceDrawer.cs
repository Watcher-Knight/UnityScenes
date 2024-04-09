using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty guidProperty = property.FindPropertyRelative("Guid");
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenesEditor.GetPathFromGuid(guidProperty.stringValue ?? ""));

        SceneAsset newScene = EditorGUILayout.ObjectField(label, oldScene, typeof(SceneAsset), false) as SceneAsset;
        if (newScene != oldScene)
        {
            string path = AssetDatabase.GetAssetPath(newScene);
            if (SceneUtility.GetBuildIndexByScenePath(path) > -1)
                guidProperty.stringValue = ScenesEditor.GetGuidFromPath(path);
            else Debug.LogWarning($"\"{path}\" is not a valid scene.");
        }
    }
}