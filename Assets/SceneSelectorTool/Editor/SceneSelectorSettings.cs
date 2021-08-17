using System;
using UnityEditor;
using UnityEngine;

[FilePath("Assets/SceneSelectorTool/Editor/SceneSelectorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
[CreateAssetMenu]
public class SceneSelectorSettings : ScriptableSingleton<SceneSelectorSettings>
{
    public bool PlayOnOpen;

    void OnDisable()
    {
        Save(true);
    }
}