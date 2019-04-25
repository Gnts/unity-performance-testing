using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PerformanceTesting;
using UnityEditor;
using UnityEngine;

public static class RunnerCLI
{
    public static void ExecuteTests()
    {
        string playerPath = GetArg("-ptPlayerPath");
        string filter = GetArg("-ptFilter");
        string ptPlatform = GetArg("-ptPlatform");
        string ptResults = GetArg("-ptResultsPath");
        BuildTarget platform = BuildTarget.NoTarget;
        Debug.Log("[PerformanceTest] player path: " + playerPath);
        Debug.Log("[PerformanceTest] filter: " + filter);
        Debug.Log("[PerformanceTest] platform: " + ptPlatform);
        Debug.Log("[PerformanceTest] results: " + ptResults);

        if (ptPlatform != null)
            platform = (BuildTarget)Enum.Parse(typeof(BuildTarget), GetArg("-ptPlatform"), true);

        if (ptResults != null)
            UpdateSettings(new RunSettings(ptResults));
        else
            UpdateSettings();


        if (platform == BuildTarget.StandaloneWindows || platform == BuildTarget.StandaloneWindows64)
            EditmodeRunner.BuildPlayerWithTests(playerPath, platform);
        else if (platform != BuildTarget.NoTarget)
            Debug.LogErrorFormat("[PerformanceTest] Platform {0} not supported", platform);
        else
            EditmodeRunner.RunPlaymodeTests();

        EditorApplication.Exit(0);
    }
    
    private static void UpdateSettings()
    {
        var settings = new RunSettings(Application.persistentDataPath + "/PerformanceRunSettings.json");
        var json = EditorJsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/PerformanceRunSettings.json", json);
    }

    private static void UpdateSettings(RunSettings settings)
    {
        var json = EditorJsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/PerformanceRunSettings.json", json);
    }

    private static string GetArg(string name)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals(name, StringComparison.InvariantCultureIgnoreCase) && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }

        return null;
    }
}