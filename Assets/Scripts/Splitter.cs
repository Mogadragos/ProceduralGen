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
        if (mesh == null)
        {
            return null;
        }

        Vector3[] verts = mesh.vertices;
        Vector2[] uv = mesh.uv;
        Vector3[] norm = mesh.normals;
        Vector4[] tan = mesh.tangents;

        int submeshCount = mesh.subMeshCount;

        SlicedSubMesh[] slices = new SlicedSubMesh[submeshCount];
        List<Vector3> crossHull = new List<Vector3>();
        IntersectionResult res = new IntersectionResult();

        bool genUV = verts.Length == uv.Length;
        bool genNorm = verts.Length == norm.Length;
        bool genTan = verts.Length == tan.Length;


        for (int submesh = 0; submesh < submeshCount; submesh++)
        {
            int[] indices = mesh.GetTriangles(submesh);
            int indicesCount = indices.Length;

            SlicedSubMesh sliced = new SlicedSubMesh();

            for (int index = 0; index < indicesCount; index += 3)
            {
                int i0 = indices[index];
                int i1 = indices[index + 1];
                int i2 = indices[index + 2];

                Triangle triangle = new Triangle(verts[i0], verts[i1], verts[i2]);

                if (genNorm)
                {
                    triangle.SetNormal(norm[0], norm[i1], norm[i2]);
                }

                if (triangle.Split(plane, res))
                {
                    int upperCount = res.UpperCount;
                    int lowerCount = res.LowerCount;
                    int intersecCount = res.IntersectionPointsCount;

                    foreach (var item in res.UpperHull)
                    {
                        sliced.UpperHull.Add(item);
                    }
                    foreach (var item in res.LowerHull)
                    {
                        sliced.LowerHull.Add(item);
                    }
                    foreach (var item in res.IntersectionPoints)
                    {
                        crossHull.Add(item);
                    }

                }
                else
                {
                    SideOfPlane sa = plane.SideOf(verts[i0]);
                    SideOfPlane sb = plane.SideOf(verts[i1]);
                    SideOfPlane sc = plane.SideOf(verts[i2]);

                    SideOfPlane side = SideOfPlane.ON;

                    if (sa != SideOfPlane.ON)
                    {
                        side = sa;
                    }
                    if (sb != SideOfPlane.ON)
                    {
                        side = sb;
                    }
                    if (sc != SideOfPlane.ON)
                    {
                        side = sc;
                    }

                    if (side == SideOfPlane.UP || side == SideOfPlane.ON)
                    {
                        sliced.UpperHull.Add(triangle);
                    }
                    else
                    {
                        sliced.LowerHull.Add(triangle);
                    }
                }
            }

            slices[submesh] = sliced;
        }

        foreach (var slice in slices)
        {
            if (slice != null && slice.IsValid)
            {
                return CreateFrom(slices, CreateFrom(crossHull, plane.Normale), crossindex);
            }
        }

        return null;
    }

    private static List<Triangle> CreateFrom(List<Vector3> crossHull, Vector3 normale)
    {
        List<Triangle> triangles;
        if (Triangulator.MonotoneChain(crossHull, normale, out triangles);
    }

    private static SplitElement CreateFrom(SlicedSubMesh[] meshes, List<Triangle> cross, int crossSectionIndex)
    {
        l
    }
}
