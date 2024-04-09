using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : ScriptableObject
{
    [SerializeField] private List<ScenePair> Scenes;

    private static SceneData b_Instance;
    private static SceneData Instance
    {
        get
        {
            if (b_Instance == null)
                b_Instance = Resources.Load<SceneData>("SceneData") ?? throw new MissingReferenceException("Could not load SceneData");
            return b_Instance;
        }
    }
    public static void SetScenes(IEnumerable<ScenePair> scenes) => Instance.Scenes = new(scenes);
    public static string[] GetAllPaths()
    {
        List<string> paths = new List<string>();
        for (int i = 0; true; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Length > 0) paths.Add(path);
            else break;
        }
        return paths.ToArray();
    }

    public static string GetPath(string guid) => Instance.Scenes.FirstOrDefault(s => s.Guid == guid).Path;
}

[Serializable]
public struct ScenePair
{
    public string Path;
    public string Guid;

    public ScenePair(string path, string guid)
    {
        Path = path;
        Guid = guid;
    }
}