using System;
using System.Collections.Generic;
using UnityEngine;

internal class Triangulator
{
    internal struct Mapped2D
    {
        private readonly Vector3 original;
        private readonly Vector2 mapped;

        public Mapped2D(Vector3 newOriginal, Vector3 u, Vector3 v)
        {
            this.original = newOriginal;
            this.mapped = new Vector2(Vector3.Dot(newOriginal, u), Vector3.Dot(newOriginal, v));
        }

        public Vector2 mappedValue
        {
            get { return this.mapped; }
        }

        public Vector3 originalValue
        {
            get { return this.original; }
        }
    }

    internal static bool MonotoneChain(List<Vector3> crossHull, Vector3 normale, out List<Triangle> triangles)
    {
        return MonotoneChain(crossHull, normale, out triangles, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f));
    }

    // implémentation de l'algo d'Andrew qui permet de calculer l'enveloppe convexe
    public static bool MonotoneChain(List<Vector3> vertices, Vector3 normal, out List<Triangle> tri, TextureRegion texRegion)
    {
        //il faut plusde 3 points pour trianguler qq chose
        if (vertices.Count < 3)
        {
            tri = null;
            return false;
        }

        // passage de 3D à 2D
        Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.up));
        if (Vector3.zero == u)
        {
            u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
        }
        Vector3 v = Vector3.Cross(u, normal);

        //les points en version 2D
        Mapped2D[] mapped = new Mapped2D[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertice = vertices[i];

            Mapped2D mappedVertice = new Mapped2D(vertice, u, v);
            mapped[i] = mappedVertice;

        }

        //1ere étape de l'algo d'Anddrew : organiser par X croissant et si égalité  départager par le Y
        Array.Sort(mapped, (a, b) =>
        {
            Vector2 t = a.mappedValue;
            Vector2 p = b.mappedValue;

            return (t.x < p.x || (p.x == t.x && t.y < p.y) ? -1 : 1);
        });

        Mapped2D[] hulls = new Mapped2D[vertices.Count + 1];

        int k = 0;
        // lower hull
        for (int i = 0; i < vertices.Count; i++)
        {
            while (k >= 2)
            {
                Vector2 mA = hulls[k - 2].mappedValue;
                Vector2 mB = hulls[k - 1].mappedValue;
                Vector2 mC = mapped[i].mappedValue;

                if (Intersector.TriArea2D(mA.x, mA.y, mB.x, mB.y, mC.x, mC.y) > 0.0f)
                {
                    break;
                }

                k--;
            }
            hulls[k++] = mapped[i];
        }

        //upper hull
        for (int i = vertices.Count - 2, t = k + 1; i >= 0; i--)
        {
            while (k >= t)
            {
                Vector2 mA = hulls[k - 2].mappedValue;
                Vector2 mB = hulls[k - 1].mappedValue;
                Vector2 mC = mapped[i].mappedValue;

                if (Intersector.TriArea2D(mA.x, mA.y, mB.x, mB.y, mC.x, mC.y) > 0.0f)
                {
                    break;
                }

                k--;
            }

            hulls[k++] = mapped[i];
        }

        int totVertice = k - 1;
        int totTriangles = (totVertice - 2) * 3;

        if (totVertice < 3)
        {
            tri = null;
            return false;
        }

        tri = new List<Triangle>(totVertice - 2);

        int index = 1;
        // g"neration des nouvelles vertices
        for (int i = 0; i < totTriangles; i += 3)
        {
            Mapped2D A = hulls[0];
            Mapped2D B = hulls[index];
            Mapped2D C = hulls[index + 1];

            Triangle triangle= new Triangle(A.originalValue, B.originalValue, C.originalValue);
            triangle.SetNormal(normal, normal, normal);

            tri.Add(triangle);
            index++;
        }

        return true;
    }
}