using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PerformanceTesting;
using UnityEngine.SceneManagement;

public class Examples
{
    [Test]
    public IEnumerator Shake_TimeTo_Render10Frames()
    {
        SceneManager.LoadScene("Shake");
        yield return null;
        yield return null;

        yield return Measure.Frametime();
        yield return null;
        yield return null;
    }

    [Teardown]
    public IEnumerator Teardown()
    {
        SceneManager.LoadScene("Empty");
        yield return null;
        yield return null;
    }
}
