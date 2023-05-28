using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension 
{
    public static void ChangeIndice<T>(this T[,] array, int x, int y, T newItem)
    {
        array[x, y] = newItem;
    }
}
