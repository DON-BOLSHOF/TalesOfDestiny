﻿using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static void SnuffleArray<T>(T[] array)
        {
            var n = array.Length;

            for (var i = 0; i < n; i++)
            {
                Swap(array, i, i + Random.Range(0, n - i));
            }
        }

        public static void Swap<T>(T[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    }
}