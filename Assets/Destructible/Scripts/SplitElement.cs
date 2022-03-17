using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitElement
{
    // Start is called before the first frame update
    Mesh upper_mesh;
    Mesh lower_mesh;

    public SplitElement(Mesh upper, Mesh lower)
    {
        upper_mesh = upper;
        lower_mesh = lower;
    }


    public GameObject CreateUpper()
    {
        return CreateEmptyObject("upper", upper_mesh);
    }

    public GameObject CreateLower()
    {
        return CreateEmptyObject("lower", lower_mesh);
    }

    private GameObject CreateEmptyObject(string name, Mesh mesh)
    {
        if (mesh == null)
        {
            return null;
        }

        GameObject go = new GameObject(name);
        go.AddComponent<MeshRenderer>();
        MeshFilter filter = go.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        return go;
    }
}
