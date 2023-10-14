using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrepAPI = Autodesk.AutoCAD.BoundaryRepresentation;

namespace Civil3DExtractSurfacesFromSolid.Extensions
{
    public static class BrepFaceExtensions
    {
        public static List<Point3d> GetPoints(this BrepAPI.Face face)
        {
            List<BrepAPI.Vertex> vertices = face.Loops.SelectMany(loop => loop.Vertices).ToList();
            List<Point3d> points = vertices.ConvertAll(vertice => new Point3d(vertice.Point.X, vertice.Point.Y, vertice.Point.Z));
            return points;
        }

        public static List<BrepAPI.Edge> GetEdges(this BrepAPI.Face face)
        {
            List<BrepAPI.Edge> edges = face.Loops.SelectMany(loop => loop.Edges).ToList();
            return edges;
        }

        public static bool IsTinSurfaceCanBeCreated(this BrepAPI.Face face, int tolerance)
        {
            List<Point3d> facePoints = face.GetSurfaceAsNurb().ControlPoints.ToList();
            List<Point2d> facePoints2d = facePoints.Select(point => new Point2d(Math.Round(point.X, tolerance), Math.Round(point.Y, tolerance))).ToList();

            List<Point2d> distinctedPoints = facePoints2d.GroupBy(point => new { point.X, point.Y }).Select(group => group.First()).ToList();

            if (distinctedPoints.Count < 3)
            {
                return false;
            }

            return true;
        }
    }
}
