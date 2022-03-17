using UnityEngine;

public class IntersectionResult
{
    public bool Success { get; private set; }= false;

    public Triangle[] UpperHull { get; private set; } = new Triangle[2];
    public Triangle[] LowerHull { get; private set; } = new Triangle[2];
    public Vector3[] IntersectionPoints { get; private set; } = new Vector3[2];

    public int UpperCount { get; private set; } = 0;
    public int LowerCount { get; private set; } = 0;
    public int IntersectionPointsCount { get; private set; } = 0;

    public IntersectionResult AddUpperHull(Triangle triangle)
    {
        UpperHull[UpperCount++] = triangle;

        Success = true;
        return this;
    }

    public IntersectionResult AddLowerHull(Triangle triangle)
    {
        LowerHull[LowerCount++] = triangle;

        Success = true;
        return this;
    }

    public void AddIntersectionpoint(Vector3 pt)
    {
        IntersectionPoints[IntersectionPointsCount++] = pt;
    }

    public void Clear()
    {
        Success = false;
        UpperCount = 0;
        LowerCount = 0;
        IntersectionPointsCount = 0;
    }
}