using System;
using System.Reflection;
using System.Text;

public static class Utils
{
    public static string CombineString(object[] objs)
    {
        if (objs == null) return "";
        StringBuilder sb = new StringBuilder();
        foreach (var o in objs)
        {
            sb.Append(o.ToString() + ", ");
        }
        if (objs.Length > 0)
            sb.Remove(sb.Length - 2, 2);

        return sb.ToString();
    }
}
