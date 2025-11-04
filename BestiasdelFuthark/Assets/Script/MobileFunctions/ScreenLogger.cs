using UnityEngine;
using System.Collections.Generic;

public class ScreenLogger : MonoBehaviour
{
    Queue<string> logs = new Queue<string>();

    void OnEnable() => Application.logMessageReceived += HandleLog;
    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logs.Enqueue(logString);
        if (logs.Count > 15) logs.Dequeue(); // Máximo 15 líneas
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        foreach (var log in logs)
            GUILayout.Label(log);
        GUILayout.EndArea();
    }
}
