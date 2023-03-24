using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Djb2
{

    public static int GetHashCode(string str)
    {

        uint hash = 5381;
        uint i = 0;

        for (i = 0; i < str.Length; i++)
        {
            hash = ((hash << 5) + hash) + ((byte)str[(int)i]);
        }

        if (hash >= 2147483648)
        {
            hash -= 2147483648;
        }

        return (int)hash;

    }

}