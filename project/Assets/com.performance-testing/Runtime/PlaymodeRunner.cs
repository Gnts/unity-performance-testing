using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PerformanceTesting
{
    public class PlaymodeRunner : MonoBehaviour
    {
        private static PlaymodeRunner Instance;

        public IEnumerator Start()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            DontDestroyOnLoad(gameObject);
            yield return null;
            yield return null;

            yield return TestRun.Execute();
            CleanAfterTest();
        }

        public void CleanAfterTest()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}