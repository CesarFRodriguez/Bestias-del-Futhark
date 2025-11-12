using UnityEditor;
using UnityEngine;
using System.Linq;

public class QuickBuild
{
    [MenuItem("Builds/Build PC (Fast)")]
    public static void BuildPC()
    {
        // 🔹 Obtiene todas las escenas activas del Build Settings
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("❌ No hay escenas marcadas en Build Settings.");
            return;
        }

        // 🔹 Asegura que la escena principal (índice 0) esté al inicio
        // (ya lo está si está configurada así en Build Settings)
        Debug.Log($"🎮 Escena principal: {System.IO.Path.GetFileNameWithoutExtension(scenes[0])}");
        Debug.Log($"Total escenas incluidas: {scenes.Length}");

        string path = "Builds/PC/NetworkTest.exe";

        BuildPlayerOptions opt = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.StandaloneWindows64,
            locationPathName = path,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(opt);
        Debug.Log("✅ PC Build ready for network test!");
    }
}
