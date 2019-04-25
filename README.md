# Unity Performance Testing
Unity test framework(built on NUnit) is not ideal for performance testing as you are unable to run on release players and there is very little ways of extending it due to limited support of NUnit attributes and interfaces.

This was my attempt to hack a test-runner similar to existing test framework with additional quality of life improvements.



## Installation

Add `com.performance-testing` into your project.
Create `Assets/StreamingAssets` folder. 

StreamingAssets folder is used for saving run settings between editor and built players.



## Running

Build unity-test-host, installed dotnet core is required.


Arguments for the host app:

`--editor` - path to Unity editor

`--project` - path to project

`--player` - path where to save player

`--platform` - build target platform

`--filter` - test filter

`--results` - folder where to save results



##### Example:

``` shell
unity-test-host.exe --editor="C:/Program Files/Unity/Hub/Editor/2019.2.0a4/Editor/Unity.exe" --project="C:/Users/User/unity-performance-testing/project/"  --platform="StandaloneWindows64"  --filter="All" --player="C:/Users/User/unity-performance-testing/project/Build/player.exe"  --results="C:/Users/User/results"
```



## Adding tests

Create a test folder and add an assembly definition that references `PerformanceTesting` assembly.

All scripts in the assembly that have `[Test]` attributes will be included in players. 

``` c#
using System.Collections;
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
    }

    [Teardown]
    public IEnumerator Teardown()
    {
        SceneManager.LoadScene("Empty");
        yield return null;
        yield return null;
    }
}

```



In the example a test is depending on scene `shake`, it should be added to build settings otherwise it will not be picked up during build time.



## Notes

This is not a finished repo and I will likely not develop this actively. This was a side project I worked on a couple hours a week for a month.

For now the only tested platform is windows standalone.