using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SideOfPlane
{
    UP,
    DOWN,
    ON
}
public struct Plane
{
    public Vector3 Normale { get; private set; }
    public float Distance { get; private set; }


    public Plane(Vector3 pos, Vector3 norm)
    {
        Normale = norm;
        Distance = Vector3.Dot(norm, pos);

    }

    public Plane(Vector3 norm, float dot)
    {
        Normale=norm;
        Distance = dot;
    }

    public Plane(Vector3 ptA, Vector3 ptB, Vector3 ptC)
    {
        Normale = Vector3.Normalize(Vector3.Cross(ptB - ptA, ptC - ptA));
        Distance = -Vector3.Dot(Normale, ptA);
    }

    public void Compute(Vector3 pos, Vector3 norm)
    {
        Normale = norm;
        Distance= Vector3.Dot(norm, pos);
    }

    public void Compute(Transform trans)
    {
        Compute(trans.position, trans.up);

        // this is for editor debugging only!
    }

    public void Compute(GameObject obj)
    {
        Compute(obj.transform);
    }

    public SideOfPlane SideOf(Vector3 pt)
    {
        float res = Vector3.Dot(Normale, pt) - Distance;
        if (res > Intersector.Epsilon)
        {
            return SideOfPlane.UP;
        }

        if (res < -Intersector.Epsilon)
        {
            return SideOfPlane.DOWN;
        }

        return SideOfPlane.ON;
    }

}
