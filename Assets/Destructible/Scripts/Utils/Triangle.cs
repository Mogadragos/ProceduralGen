using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Triangle 
{
    public Vector3 PointA { get; private set; }
    public Vector3 PointB { get; private set; }
    public Vector3 PointC { get; private set; }

    public Vector3 NormA { get; private set; }
    public Vector3 NormB { get; private set; }
    public Vector3 NormC { get; private set; }

    public bool IsNormSet { get; private set; }

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        PointA = a;
        PointB = b;
        PointC = c;

        NormA = Vector3.zero;
        NormB = Vector3.zero;
        NormC = Vector3.zero;
        IsNormSet = false;
    }

    public void SetNormal(Vector3 normA, Vector3 normB, Vector3 normC)
    {
        NormA = normA;
        NormB = normB;
        NormC = normC;
    }

    public Vector3 GenerateNormale(Vector3 point)
    {
        if (!IsNormSet)
        {
            return Vector3.zero;
        }

        Vector3 weights = Barycentric(point);
        return (weights.x * NormA) + (weights.y * NormB) + (weights.z * NormC);

    }

    public Vector3 Barycentric(Vector3 p)
    {
        Vector3 a = PointA;
        Vector3 b = PointB;
        Vector3 c = PointC;

        Vector3 m = Vector3.Cross(b - a, c - a);

        float nu;
        float nv;
        float ood;

        float x = Mathf.Abs(m.x);
        float y = Mathf.Abs(m.y);
        float z = Mathf.Abs(m.z);

        // compute areas of plane with largest projections
        if (x >= y && x >= z)
        {
            // area of PBC in yz plane
            nu = Intersector.TriArea2D(p.y, p.z, b.y, b.z, c.y, c.z);
            // area of PCA in yz plane
            nv = Intersector.TriArea2D(p.y, p.z, c.y, c.z, a.y, a.z);
            // 1/2*area of ABC in yz plane
            ood = 1.0f / m.x;
        }
        else if (y >= x && y >= z)
        {
            // project in xz plane
            nu = Intersector.TriArea2D(p.x, p.z, b.x, b.z, c.x, c.z);
            nv = Intersector.TriArea2D(p.x, p.z, c.x, c.z, a.x, a.z);
            ood = 1.0f / -m.y;
        }
        else
        {
            // project in xy plane
            nu = Intersector.TriArea2D(p.x, p.y, b.x, b.y, c.x, c.y);
            nv = Intersector.TriArea2D(p.x, p.y, c.x, c.y, a.x, a.y);
            ood = 1.0f / m.z;
        }

        float u = nu * ood;
        float v = nv * ood;
        float w = 1.0f - u - v;

        return new Vector3(u, v, w);
    }

    public bool Split(Plane pl, IntersectionResult result)
    {
        Intersector.Intersect(pl, this, result);

        return result.Success;
    }
}

