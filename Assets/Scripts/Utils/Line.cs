using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{

    public Vector3 PointA { get; private set; }
    public Vector3 PointB { get; private set; }

    public float Size { get => Vector3.Distance(PointA, PointB); }

    public Line(Vector3 a, Vector3 b)
    {
        PointB = b;
        PointA = a;
    }
}
