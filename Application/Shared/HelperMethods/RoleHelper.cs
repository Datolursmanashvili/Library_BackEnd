using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Application.HelperMethods;

public static class RoleHelper
{
    public static bool CheckRoleName(string roleName)
    {
        char[] allLetters = new char[63];
        var listStr = new List<string>();

        for (int i = 0; i < 26; i++)
        {
            allLetters[i] = (char)('A' + i);
            allLetters[i + 26] = (char)('a' + i);
        }
        allLetters[52] = ' ';
        for (int i = 0; i < 10; i++)
        {
            allLetters[i + 53] = (char)('0' + i);
        }

        foreach (char symbol in allLetters)
        {
            foreach (char name in roleName)
            {
                if (symbol == name)
                {
                    listStr.Add(symbol.ToString());
                }
            }
        }

        if (listStr.IsNull() || listStr.Count() < roleName.Length)
            return true;

        return false;
    }

}
