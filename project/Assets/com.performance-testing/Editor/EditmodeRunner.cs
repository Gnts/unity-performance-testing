using System;
using System.Collections;
using System.Collections.Generic;
using PerformanceTesting;
using UnityEditor;
using UnityEditor.SceneManagement;
using Debug = UnityEngine.Debug;
using PlayerSettings = UnityEditor.PlayerSettings;

namespace PerformanceTesting
{
    public class EditmodeRunner
    {
        [MenuItem("Tests/Execute")]
        public static void RunPlaymodeTests()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SetActiveScene(scene);
            CreatePlaymodeRunner();
            EditorApplication.isPlaying = true;
        }

        public static PlaymodeRunner CreatePlaymodeRunner()
        {
            var obj = new UnityEngine.GameObject("TestRunner");
            var component = obj.AddComponent<PlaymodeRunner>();
            return component;
        }

        public static IEnumerator ExecutePrebuildSetup(List<Type> testTypes)
        {
            foreach (var type in testTypes)
            {
                var setupMethods = type.GetPrebuildMethods();
                if (setupMethods.Length > 0)
                {
                    var instance = Activator.CreateInstance(type);
                    foreach (var methodInfo in setupMethods)
                    {
                        if (methodInfo.ReturnType == typeof(IEnumerator))
                            yield return (IEnumerator)methodInfo.Invoke(instance, null);
                        else
                            methodInfo.Invoke(instance, null);
                    }
                }

                var setupTypeNames = type.GetPrebuildTypeNames();
                if (setupTypeNames.Length > 0)
                {
                    foreach (var testType in testTypes)
                    {
                        foreach (var setupTypeName in setupTypeNames)
                        {
                            if (testType.FullName.Contains(setupTypeName))
                            {
                                var instance = Activator.CreateInstance(testType);
                                var setupMethodInfo = testType.GetMethod("Setup");
                                yield return instance.Invoke(setupMethodInfo);
                                continue;
                            }
                        }
                    }
                }

                var setupInterface = type.GetInterface("IPrebuildSetup");
                if (setupInterface != null)
                {
                    var instance = Activator.CreateInstance(type);
                    var mi = type.GetMethod("Setup");
                    mi.Invoke(instance, null);
                }
            }
        }

        public static void RunStandaloneTests()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            var levels = EditorBuildSettings.scenes;
            var report = BuildPipeline.BuildPlayer(levels, target+"Build", target, BuildOptions.None);
            UnityEngine.Debug.Log(report.summary);
        }

        public static void BuildPlayerWithTests(string playerPath, BuildTarget platform)
        {
            var levels = EditorBuildSettings.scenes;
            var report = BuildPipeline.BuildPlayer(levels, playerPath, platform, BuildOptions.None);
            UnityEngine.Debug.Log(report.summary);
        }
    }
}