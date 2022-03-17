using System.Collections;
using UnityEngine;

/**
    * Define Extension methods for easy access to splitter functionality
    */
public static class SplitterExtensions
{

    /**
        * SplitElement Return functions and appropriate overrides!
        */
    public static SplitElement Split(this GameObject obj, Plane pl, Material crossSectionMaterial = null)
    {
        return Split(obj, pl, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f), crossSectionMaterial);
    }

    public static SplitElement Split(this GameObject obj, Vector3 position, Vector3 direction, Material crossSectionMaterial = null)
    {
        return Split(obj, position, direction, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f), crossSectionMaterial);
    }

    public static SplitElement Split(this GameObject obj, Vector3 position, Vector3 direction, TextureRegion textureRegion, Material crossSectionMaterial = null)
    {
        Plane cuttingPlane = new Plane();

        Matrix4x4 mat = obj.transform.worldToLocalMatrix;
        Matrix4x4 transpose = mat.transpose;
        Matrix4x4 inv = transpose.inverse;

        Vector3 refUp = inv.MultiplyVector(direction).normalized;
        Vector3 refPt = obj.transform.InverseTransformPoint(position);

        cuttingPlane.Compute(refPt, refUp);

        return Split(obj, cuttingPlane, textureRegion, crossSectionMaterial);
    }

    public static SplitElement Split(this GameObject obj, Plane pl, TextureRegion textureRegion, Material crossSectionMaterial = null)
    {
        return Splitter.Split(obj, pl);
    }

    /**
        * These functions (and overrides) will return the final indtaniated GameObjects types
        */
    public static GameObject[] SplitInstantiate(this GameObject obj, Plane pl)
    {
        return SplitInstantiate(obj, pl, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f));
    }

    public static GameObject[] SplitInstantiate(this GameObject obj, Vector3 position, Vector3 direction)
    {
        return SplitInstantiate(obj, position, direction, null);
    }

    public static GameObject[] SplitInstantiate(this GameObject obj, Vector3 position, Vector3 direction, Material crossSectionMat)
    {
        return SplitInstantiate(obj, position, direction, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f), crossSectionMat);
    }

    public static GameObject[] SplitInstantiate(this GameObject obj, Vector3 position, Vector3 direction, TextureRegion cuttingRegion, Material crossSectionMaterial = null)
    {
        Plane cuttingPlane = new Plane();

        Matrix4x4 mat = obj.transform.worldToLocalMatrix;
        Matrix4x4 transpose = mat.transpose;
        Matrix4x4 inv = transpose.inverse;

        Vector3 refUp = inv.MultiplyVector(direction).normalized;
        Vector3 refPt = obj.transform.InverseTransformPoint(position);

        cuttingPlane.Compute(refPt, refUp);

        return SplitInstantiate(obj, cuttingPlane, cuttingRegion, crossSectionMaterial);
    }

    public static GameObject[] SplitInstantiate(this GameObject obj, Plane pl, TextureRegion cuttingRegion, Material crossSectionMaterial = null)
    {
        SplitElement split = Splitter.Split(obj, pl);

        if (split == null)
        {
            return null;
        }

        GameObject upperHull = split.CreateUpper();
        GameObject lowerHull = split.CreateLower();

        if (upperHull != null && lowerHull != null)
        {
            return new GameObject[] { upperHull, lowerHull };
        }

        // otherwise return only the upper hull
        if (upperHull != null)
        {
            return new GameObject[] { upperHull };
        }

        // otherwise return only the lower hull
        if (lowerHull != null)
        {
            return new GameObject[] { lowerHull };
        }

        // nothing to return, so return nothing!
        return null;
    }
}