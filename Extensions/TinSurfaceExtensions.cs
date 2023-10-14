using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrepAPI = Autodesk.AutoCAD.BoundaryRepresentation;

namespace Civil3DExtractSurfacesFromSolid.Extensions
{
    public static class TinSurfaceExtensions
    {
        public static void AddBrepEdges(this TinSurface tinSurface, List<BrepAPI.Edge> edges)
        {
            Point3d[] startVertices = edges.Select(edge => edge.Vertex1.Point).ToArray();
            Point3d[] endvertices = edges.Select(edge => edge.Vertex1.Point).ToArray();

            tinSurface.AddVertices(new Point3dCollection(startVertices));
            tinSurface.AddVertices(new Point3dCollection(endvertices));
        }

        public static void AddBrepFace(this TinSurface tinSurface, BrepAPI.Face face)
        {
            List<BrepAPI.Edge> edges = face.GetEdges();
            tinSurface.AddBrepEdges(edges);
        }
    }
}
