using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civil3DExtractSurfacesFromSolid.Extensions
{
    public static class DocumentExtensions
    {
        public static List<Solid3d> PickSolids3d(this Document document, OpenMode openMode)
        {
            List<Solid3d> solids = new List<Solid3d>();

            PromptSelectionOptions options = new PromptSelectionOptions();
            options.MessageForAdding = "Pick solids";

            Editor editor = document.Editor;
            Database database = document.Database;

            TypedValue[] typedValues = new TypedValue[1];
            typedValues.SetValue(new TypedValue((int)DxfCode.Start, "3DSOLID"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValues);

            PromptSelectionResult promptResult = editor.GetSelection(options, selectionFilter);

            if (promptResult.Status == PromptStatus.OK)
            {
                SelectionSet selectionSet = promptResult.Value;
                // Step through the objects in the selection set
                using (document.LockDocument())
                {
                    using (Transaction transaction = database.TransactionManager.StartTransaction())
                    {
                        foreach (SelectedObject selectedObject in selectionSet)
                        {
                            if (selectedObject != null)
                            {
                                Solid3d solid = transaction.GetObject(selectedObject.ObjectId, openMode, false, true) as Solid3d;
                                solids.Add(solid);
                            }
                        }
                        transaction.Commit();
                    }
                }
            }

            return solids;
        }
    }
}
