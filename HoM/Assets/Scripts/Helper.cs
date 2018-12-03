using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {

	public static float Longevity(float now, float delay, float max)
    {
        return Mathf.Min(Mathf.Max(0.0f, now - delay) / max, 1.0f);
    }


    /// <summary>
    /// (Lifted from the internet)
    /// Gets the EULER angle between two points on a 2-dimensional plane.
    /// Used mainly for determining the facing direction of actors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Direction(Vector3 a, Vector3 b)
    {
        return Helper.Roundabout(180 - (Mathf.Atan2(b.z - a.z, b.x - a.x) * Mathf.Rad2Deg) -90, 360);
    }
    // *** ... Because Unity just can't answer a simple question.


    public static float Roundabout(float val, float max)
    {
        return (val % max + max) % max;
    }
}
