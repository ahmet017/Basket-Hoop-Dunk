using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class IntegerExtensions
{

    /// <summary>
    /// Milliseconds to time format
    /// </summary>
    /// <param name="time">Time in milliseconds</param>
    /// <returns></returns>
    public static string ToTimeFormat(this int time)
    {

        // less than an hour
        if (time < 3600 * 1000)
        {
            return TimeSpan.FromMilliseconds(time).ToString(@"mm\:ss");
        }
        else 
        {
            return TimeSpan.FromMilliseconds(time).ToString(@"hh\:mm\:ss");
        }

    }
}
