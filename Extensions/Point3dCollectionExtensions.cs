using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civil3DExtractSurfacesFromSolid.Extensions
{
    public static class Point3dCollectionExtensions
    {
        public static List<Point3d> ToList(this Point3dCollection point3DCollection)
        {
            List<Point3d> points = new List<Point3d>();

            foreach (Point3d point in point3DCollection)
            {
                points.Add(point);
            }

            return points;
        }
    }
}
