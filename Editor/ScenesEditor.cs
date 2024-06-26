using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class ScenesEditor
{
    public static string BuildSettingPath = Application.dataPath.Replace("/Assets", "/ProjectSettings/EditorBuildSettings.asset");
    private const string RelativeDataPath = "/Scenes/Resources/SceneData.asset";

    public static string GetPathFromGuid(string guid)
    {
        if (guid.Length == 0) return "";
        string[] lines = File.ReadAllLines(BuildSettingPath);
        int index = 0;
        foreach (string line in lines)
        {
            if (line.Contains($"guid: {guid}")) return lines[index - 1].Replace("    path: ", "");
            index++;
        }
        return "";
    }
    public static string GetGuidFromPath(string path)
    {
        string[] lines = File.ReadAllLines(BuildSettingPath);
        int index = 0;
        foreach (string line in lines)
        {
            if (line.Contains(path)) return lines[index + 1].Replace("    guid: ", "");
            index++;
        }
        return "";
    }

    [InitializeOnEnterPlayMode]
    public static void SetSceneData()
    {
        if (!File.Exists(Application.dataPath + RelativeDataPath))
            ScriptableObject.CreateInstance<SceneData>().CreateAsset("Assets" + RelativeDataPath);

        ScenePair[] scenes = SceneData.GetAllPaths().Select(p => new ScenePair(p, GetGuidFromPath(p))).ToArray();
        SceneData.SetScenes(scenes);
    }

    public static void CreateAsset<T>(this T obj, string path) where T : UnityEngine.Object
    {
        if (!Regex.IsMatch(path, @".asset$")) throw new ArgumentException("Path must end with .asset");
        string[] directories = path.Split("/").SkipLast(1).ToArray();
        string currentDirectory = Application.dataPath;
        for (int i = 1; i < directories.Length; i++)
        {
            currentDirectory += "/" + directories[i];
            if (!Directory.Exists(currentDirectory)) Directory.CreateDirectory(currentDirectory);
        }
        AssetDatabase.CreateAsset(obj, path);
    }
}

public class ScenePreBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        ScenesEditor.SetSceneData();
    }
}