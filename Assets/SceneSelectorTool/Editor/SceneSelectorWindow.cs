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

        var returnOnExitToggle = new Toggle("Return on Exit");
        returnOnExitToggle.SetValueWithoutNotify(SceneSelectorSettings.instance.ReturnOnExit);
        returnOnExitToggle.RegisterValueChangedCallback(evt =>
        {
            SceneSelectorSettings.instance.ReturnOnExit = evt.newValue;
        });
        rootVisualElement.Add(returnOnExitToggle);
        foreach (var sceneGuid in sceneGuids)
        {
            rootVisualElement.Add(CreateSceneButton(sceneGuid));
        }
    }

    static void ReturnToPreviousScene(PlayModeStateChange change)
    {
        if (SceneSelectorSettings.instance.ReturnOnExit && change == PlayModeStateChange.EnteredEditMode)
        {
            EditorSceneManager.OpenScene(SceneSelectorSettings.instance.PreviousScenePath, OpenSceneMode.Single);
        }
    }

    VisualElement CreateSceneButton(string sceneGuid)
    {
        var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
        var buttonGroup = new VisualElement();
        buttonGroup.style.flexDirection = FlexDirection.Row;
        buttonGroup.style.marginLeft = 3;

        var sceneAsset = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));
        var label = new Label($"{sceneAsset.name}");
        label.style.width = 150;
        buttonGroup.Add(label);

        var openButton = new Button(() => { EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single); });
        openButton.text = "Open";
        buttonGroup.Add(openButton);

        var playButton = new Button(() =>
        {
            SceneSelectorSettings.instance.PreviousScenePath = SceneManager.GetActiveScene().path;
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        });
        playButton.text = "Play";
        buttonGroup.Add(playButton);
        return buttonGroup;
    }
}