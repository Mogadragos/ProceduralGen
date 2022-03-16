using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersector
{
    public static bool Intersect(Plane plane, Line line, Vector3 q)
    {
        return Intersect(plane, line.PointA, line.PointB, out q);
    }

    public const float Epsilon = 0.0001f;

    public static bool Intersect(Plane plane, Vector3 pointA, Vector3 pointB, out Vector3 q)
    {
        Vector3 normale = plane.Normale;
        Vector3 ab = pointB - pointA;
        float t = (plane.Distance - Vector3.Dot(normale, pointA)) / Vector3.Dot(normale, ab);

        if (t >= -Epsilon && t <= (1 + Epsilon))
        {
            q = pointA + t * ab;
            return true;
        }
        q = Vector3.zero;
        return false;

    }

    public static float TriArea2D(float x1, float y1, float x2, float y2, float x3, float y3)
    {
        return (x1 - x2) * (y2 - y3) - (x2 - x3) * (y1 - y2);
    }

    public static void Intersect(Plane plane, Triangle triangle, IntersectionResult result)
    {
        result.Clear();
        Vector3 a = triangle.PointA;
        Vector3 b = triangle.PointB;
        Vector3 c = triangle.PointC;

        SideOfPlane sa = plane.SideOf(a);
        SideOfPlane sb = plane.SideOf(b);
        SideOfPlane sc = plane.SideOf(c);

        // tout le monde est du même côté du plan
        if (sa == sb && sb == sc)
        {
            return;
        }

        // cas où un plan est confondu avec un des cotes du triangle
        if ((sa == SideOfPlane.ON && sb == sc) ||
            (sb == SideOfPlane.ON && sa == sc) ||
            (sc == SideOfPlane.ON && sb == sa))
        {
            return;
        }

        // 1 point sur leplan, 2 autres points du même côté
        if ((sa == SideOfPlane.ON && sb != SideOfPlane.ON && sb == sc) ||
            (sb == SideOfPlane.ON && sa != SideOfPlane.ON && sc == sa) ||
            (sc == SideOfPlane.ON && sa != SideOfPlane.ON && sa == sb))
        {
            return;
        }

        // points d'intersection 
        Vector3 qa;
        Vector3 qb;

        //si un point appartient au plan => création de 2 triangles
        if (sa == SideOfPlane.ON)
        {
            if (Intersector.Intersect(plane, b, c, out qa))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(a);

                Triangle ta = new Triangle(a, b, qa);
                Triangle tb = new Triangle(a, qa, c);

                if (triangle.IsNormSet)
                {
                    Vector3 pq = triangle.GenerateNormale(qa);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pa, pb, pq);
                    tb.SetNormal(pa, pq, pc);
                }


                if (sb == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb);
                }
                else if (sb == SideOfPlane.DOWN)
                {
                    result.AddUpperHull(tb).AddLowerHull(ta);
                }
            }
        }

        else if (sb == SideOfPlane.ON)
        {
            if (Intersect(plane, a, c, out qa))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(b);

                Triangle ta = new Triangle(a, b, qa);
                Triangle tb = new Triangle(qa, b, c);

                if (triangle.IsNormSet)
                {
                    Vector3 pq = triangle.GenerateNormale(qa);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pa, pb, pq);
                    tb.SetNormal(pq, pb, pc);
                }

                if (sa == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb);
                }
                else if (sa == SideOfPlane.DOWN)
                {
                    result.AddUpperHull(tb).AddUpperHull(ta);
                }
            }
        }
        else if (sc == SideOfPlane.ON)
        {
            if (Intersect(plane, a,b, out qa))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(c);

                Triangle ta = new Triangle(a, qa, c);
                Triangle tb = new Triangle(qa, b, c);

                if (triangle.IsNormSet)
                {
                    Vector3 pq = triangle.GenerateNormale(qa);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pa, pq, pc);
                    tb.SetNormal(pq, pb, pc);
                }

                if (sa == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb);
                }
                else if (sa == SideOfPlane.DOWN)
                {
                    result.AddUpperHull(tb).AddLowerHull(ta);
                }
            }
        }


        // cas où le triangle est coupé en 2 points d'intersection 
        // création de 1 triangle + 1 trapze => décomposé en 2 triangles
        //cas où A est isolé

        else if ((sa == SideOfPlane.UP || sa == SideOfPlane.DOWN) && sb != sa && sb == sc)
        {
            if (Intersect(plane, a, b, out qa) && Intersect(plane, a, c, out qb))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(qb);

                Triangle ta = new Triangle(a, qa, qb);
                Triangle tb = new Triangle(qa, b, c);
                Triangle tc = new Triangle(qa, qb, c);

                if (triangle.IsNormSet)
                {
                    Vector3 pqa = triangle.GenerateNormale(qa);
                    Vector3 pqb = triangle.GenerateNormale(qb);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pa, pqa, pqb);
                    tb.SetNormal(pqa, pb, pc);
                    tc.SetNormal(pqa, pqb, pc);
                }

                if (sa == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb).AddLowerHull(tc);
                }
                else if (sa == SideOfPlane.DOWN)
                {
                    result.AddLowerHull(ta).AddUpperHull(tb).AddUpperHull(tc);
                }
            }
        }

        // b est isolé
        else if ((sb == SideOfPlane.UP || sb == SideOfPlane.DOWN) && sa != sb && sa == sc)
        {
            if (Intersect(plane, b, a, out qb) && Intersect(plane, b, c, out qa))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(qb);

                Triangle ta = new Triangle(b, qa, qb);
                Triangle tb = new Triangle(qa, a, c);
                Triangle tc = new Triangle(qa, qb, a);

                if (triangle.IsNormSet)
                {
                    Vector3 pqa = triangle.GenerateNormale(qa);
                    Vector3 pqb = triangle.GenerateNormale(qb);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pb, pqa, pqb);
                    tb.SetNormal(pqa, pb, pc);
                    tc.SetNormal(pqa, pqb, pc);
                }

                if (sb == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb).AddLowerHull(tc);
                }

                else if (sb == SideOfPlane.DOWN)
                {
                    result.AddLowerHull(ta).AddUpperHull(tb).AddUpperHull(tc);
                }
            }
        }

        // c isolé

        else if ((sc == SideOfPlane.UP || sc == SideOfPlane.DOWN) && sc != sa && sa == sc)
        {
            if (Intersect(plane, c, a, out qa) && Intersect(plane, c, b, out qb))
            {
                result.AddIntersectionpoint(qa);
                result.AddIntersectionpoint(qb);

                Triangle ta = new Triangle(c, qa, qb);
                Triangle tb = new Triangle(qa, b, a);
                Triangle tc = new Triangle(qa, qb, b);

                if (triangle.IsNormSet)
                {
                    Vector3 pqa = triangle.GenerateNormale(qa);
                    Vector3 pqb = triangle.GenerateNormale(qb);
                    Vector3 pa = triangle.NormA;
                    Vector3 pb = triangle.NormB;
                    Vector3 pc = triangle.NormC;

                    ta.SetNormal(pc, pqa, pqb);
                    tb.SetNormal(pqa, pb, pa);
                    tb.SetNormal(pqa, pqb, pb);
                }

                if (sc == SideOfPlane.UP)
                {
                    result.AddUpperHull(ta).AddLowerHull(tb).AddLowerHull(tc); ;
                }
                else if (sc == SideOfPlane.DOWN)
                {
                    result.AddLowerHull(ta).AddUpperHull(tb).AddUpperHull(tc);
                }
            }
        }

    }


}
