using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSelectorWindow : EditorWindow
{
    [MenuItem("Tools/Scene Selector %#o")]
    static void OpenWindow()
    {
        GetWindow<SceneSelectorWindow>();
    }

    [InitializeOnLoadMethodAttribute]
    static void RegisterCallbacks()
    {
        EditorApplication.playModeStateChanged += ReturnToPreviousScene;
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
        var returnOnExitToggle = new Toggle("Return on Exit");
        returnOnExitToggle.SetValueWithoutNotify(SceneSelectorSettings.instance.ReturnOnExit);
        returnOnExitToggle.RegisterValueChangedCallback(evt =>
        {
            SceneSelectorSettings.instance.ReturnOnExit = evt.newValue;
        });
        rootVisualElement.Add(playOnOpenToggle);
        rootVisualElement.Add(returnOnExitToggle);
        foreach (var sceneGuid in sceneGuids)
        {
            rootVisualElement.Add(CreateSceneButton(sceneGuid));
        }
    }

    static void ReturnToPreviousScene(PlayModeStateChange change)
    {
        if (SceneSelectorSettings.instance.PlayOnOpen && change == PlayModeStateChange.EnteredEditMode)
        {
            EditorSceneManager.OpenScene(SceneSelectorSettings.instance.PreviousScenePath, OpenSceneMode.Single);
        }
    }

    Button CreateSceneButton(string sceneGuid)
    {
        var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
        var button = new Button(async () =>
        {
            SceneSelectorSettings.instance.PreviousScenePath = SceneManager.GetActiveScene().path;

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            if (SceneSelectorSettings.instance.PlayOnOpen)
            {
                EditorApplication.EnterPlaymode();
            }
        });
        var sceneAsset = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
        button.text = $"{sceneAsset.name}";
        return button;
    }
}