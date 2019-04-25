using System;

[Serializable]
public class RunSettings
{
    public RunSettings(string resultsPath)
    {
        this.resultsPath = resultsPath;
    }

    public string resultsPath;
}