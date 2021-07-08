using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class S
{

    public static string[] DiferencesArrays(string[] a1, string[] a2)
    {
        var differences = new List<string>();

        if (a1 == null || a2 == null)
            return differences.ToArray();

        for (int i = 0; i < a1.Length; i++)
        {
            if (!a2.Contains(a1[i])) differences.Add(a1[i]);
        }

        for (int i = 0; i < a2.Length; i++)
        {
            if (!a1.Contains(a2[i]) && !differences.Contains(a2[i])) differences.Add(a2[i]);
        }

        return differences.ToArray();
    }

}
