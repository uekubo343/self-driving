using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// 学生のBattleBrainをセットアップする. 
/// - 学生の名前のフォルダを作成する.
/// - 内部に`YourNameBattleBrain.cs`を作成する.
/// </summary>
public class BattleBrainSetUp: EditorWindow
{
    string studentName = "Fujita";
    string templatePath = "Assets/Scripts/Editor/TemplateBattleBrain.txt";

    [MenuItem("Tools/Set Up BattleBrain")]
    public static void ShowWindow()
    {
        GetWindow<BattleBrainSetUp>("Set Up BattleBrain");
    }

    void OnGUI()
    {
        GUILayout.Label("Set Up BattleBrain", EditorStyles.boldLabel);

        studentName = EditorGUILayout.TextField("Student Name", studentName);

        if (GUILayout.Button("Create"))
        {
            if (string.IsNullOrEmpty(studentName))
            {
                Debug.LogError("Student name cannot be empty!");
                return;
            }

            if (!File.Exists(templatePath))
            {
                Debug.LogError($"Template file not found at: {templatePath}");
                return;
            }

            CreateFolderAndScript(studentName, templatePath);
            Close();
        }
    }

    static void CreateFolderAndScript(string studentName, string templatePath)
    {
        // Define paths
        string folderPath = Path.Combine("Assets", studentName);
        string scriptPath = Path.Combine(folderPath, $"{studentName}BattleBrain.cs");

        // Create the folder
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", studentName);
        }

        // Load the template content from file
        string templateContent = File.ReadAllText(templatePath);

        // Replace placeholders with the student's name
        string finalContent = templateContent.Replace("{StudentName}", studentName);

        // Create the script file
        File.WriteAllText(scriptPath, finalContent);

        // Refresh the AssetDatabase to show the new files in the Unity Editor
        AssetDatabase.Refresh();

        Debug.Log($"Folder and script created for {studentName}: {scriptPath}");
    }
}
