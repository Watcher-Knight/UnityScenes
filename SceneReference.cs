using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct SceneReference
{
    [SerializeField] private string Guid; // String reference: SceneReferenceDrawer

    public bool Load()
    {
        string path = SceneData.GetPath(Guid);
        if (path.Length > 0)
        {
            SceneManager.LoadScene(path);
            return true;
        }
        else
        {
            Debug.LogError("Could not load scene");
            return false;
        }
    }
}