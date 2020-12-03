using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private static List<Vector3> points;

    void Awake()
    {
        points = new List<Vector3>();
    }

    public static void AddPoint(Vector3 point, AudioClip sound) {
        if (!points.Contains(point))
        {
            points.Add(point);
        }
    }

    public static Vector3 GetLastCheckpoint()
    {
        return points[points.Count-1];
    }

}
