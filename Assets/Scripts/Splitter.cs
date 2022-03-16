using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter
{
    // Start is called before the first frame update
    internal class SlicedSubMesh
    {
        public List<Triangle> UpperHull { get; private set; } = new List<Triangle>();
        public List<Triangle> LowerHull { get; private set; } = new List<Triangle>();

        public bool HasNormale { get => UpperHull.Count > 0 ? UpperHull[0].IsNormSet : LowerHull.Count > 0 ? LowerHull[0].IsNormSet : false; }
        public bool IsValid { get => UpperHull.Count > 0 && LowerHull.Count > 0; }
    }

    public static SplitElement Split(GameObject obj, Plane plane)
    {
        MeshFilter filter = obj.GetComponent<MeshFilter>();

        if (filter == null)
        {
            return null;
        }

        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

        if (renderer == null)
        {
            return null;
        }

        Material[] materials = renderer.sharedMaterials;
        Mesh mesh = filter.sharedMesh;
        if (mesh == null)
        {
            return null;
        }

        int submeshCount = mesh.subMeshCount;
        if (materials.Length != submeshCount)
        {
            return null;
        }

        int crossindex = materials.Length;


        return Split(mesh, plane, crossindex);

    }

    private static SplitElement Split(Mesh mesh, Plane plane, int crossindex)
    {
        throw new NotImplementedException();
    }
}
