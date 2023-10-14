using Autodesk.Aec.PropertyData.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using Civil3DExtractSurfacesFromSolid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrepAPI = Autodesk.AutoCAD.BoundaryRepresentation;

namespace Civil3DExtractSurfacesFromSolid.Extensions
{
    public static class Solid3dExtensions
    {
        public static List<BrepAPI.Face> GetFaces(this Solid3d solid)
        {
            if(solid == null || solid.MassProperties.Volume == 0)
            {
                return new List<BrepAPI.Face>();
            }

            List<BrepAPI.Face> faces = new List<BrepAPI.Face>();

            using (Transaction transaction = DocumentService.TransactionManager.StartTransaction())
            {
                using (var brep = new BrepAPI.Brep(solid))
                {
                    if (brep == null)
                    {
                        return null;
                    }

                    foreach (BrepAPI.Face face in brep.Faces)
                    {
                        if (face == null)
                        {
                            continue;
                        }

                        faces.Add(face);
                    }
                }

                transaction.Commit();
            }

            return faces;
        }
    }
}
