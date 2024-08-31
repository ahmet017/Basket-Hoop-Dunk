using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static List<T> GetClone<T>(this List<T> list)
    {
        return list.GetRange(0, list.Count);
    }

    public static T FirstElement<T>(this IList<T> list)
    {
        return list[0];
    }

    public static T LastElement<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }

    public static string ToCommaSeperatedString<T>(this IList<T> list)
    {
        string s = "";
        string prefix = "";
        foreach (var i in list)
        {
            s += prefix;
            s += i.ToString();
            prefix = ",";
        }
        return s;
    }

    public static List<T> ParseCommaSeperatedString<T>(string s)
    {
        List<T> list = new List<T>();

        string[] strings = s.Split(",");
        foreach (var i in strings)
        {
            list.Add((T)Convert.ChangeType(i, typeof(T)));
        }
        return list;
    }
}
