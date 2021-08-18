using UnityEditor;

[FilePath("Assets/SceneSelectorTool/Editor/SceneSelectorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
public class SceneSelectorSettings : ScriptableSingleton<SceneSelectorSettings>
{
    public bool ReturnOnExit;
    public string PreviousScenePath;
    
    void OnDestroy()
    {
        Save(true);
    }
}