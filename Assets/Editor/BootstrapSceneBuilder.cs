using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[InitializeOnLoad]
public static class BootstrapSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/Ranch.unity";

    static BootstrapSceneBuilder()
    {
        EditorApplication.delayCall += EnsureBootstrapScene;
    }

    [MenuItem("Chicken Ranch/Create or Repair Bootstrap Scene")]
    private static void EnsureBootstrapScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.studio.happyhenranch");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        EnsureUniversalRenderPipeline();

        if (!File.Exists(ScenePath))
        {
            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("Chicken Ranch Game").AddComponent<RanchGameController>();
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), ScenePath);
        }

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == ScenePath && scene.enabled)
            {
                return;
            }
        }

        EditorBuildSettingsScene[] updated = new EditorBuildSettingsScene[scenes.Length + 1];
        scenes.CopyTo(updated, 0);
        updated[scenes.Length] = new EditorBuildSettingsScene(ScenePath, true);
        EditorBuildSettings.scenes = updated;
    }

    private static void EnsureUniversalRenderPipeline()
    {
        if (GraphicsSettings.renderPipelineAsset != null)
        {
            return;
        }

        Directory.CreateDirectory("Assets/Settings");
        UniversalRendererData rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
        AssetDatabase.CreateAsset(rendererData, "Assets/Settings/RanchRenderer.asset");
        UniversalRenderPipelineAsset pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
        AssetDatabase.CreateAsset(pipelineAsset, "Assets/Settings/RanchURP.asset");
        GraphicsSettings.renderPipelineAsset = pipelineAsset;
        QualitySettings.renderPipeline = pipelineAsset;
        AssetDatabase.SaveAssets();
    }
}
