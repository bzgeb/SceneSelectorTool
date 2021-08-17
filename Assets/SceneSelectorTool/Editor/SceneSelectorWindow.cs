using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

public class SceneSelectorWindow : EditorWindow
{
    [MenuItem("Tools/Scene Selector %#o")]
    static void OpenWindow()
    {
        GetWindow<SceneSelectorWindow>();
    }

    void CreateGUI()
    {
        var sceneGuids = AssetDatabase.FindAssets("t:Scene");
        var playOnOpenToggle = new Toggle("Play on Open");
        playOnOpenToggle.SetValueWithoutNotify(SceneSelectorSettings.instance.PlayOnOpen);
        playOnOpenToggle.RegisterValueChangedCallback(evt =>
        {
            SceneSelectorSettings.instance.PlayOnOpen = evt.newValue;
        });
        rootVisualElement.Add(playOnOpenToggle);
        foreach (var sceneGuid in sceneGuids)
        {
            rootVisualElement.Add(CreateSceneButton(sceneGuid));
        }
    }

    Button CreateSceneButton(string sceneGuid)
    {
        var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
        var button = new Button(() =>
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.isPlaying = SceneSelectorSettings.instance.PlayOnOpen;
        });
        var sceneAsset = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
        button.text = $"{sceneAsset.name}";
        return button;
    }
}