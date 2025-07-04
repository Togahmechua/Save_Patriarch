using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<Collider, Rope> rope = new Dictionary<Collider, Rope>();

    public static Rope GetRope(Collider collider)
    {
        if (!rope.ContainsKey(collider))
        {
            rope.Add(collider, collider.GetComponent<Rope>());
        }

        return rope[collider];
    }
}
