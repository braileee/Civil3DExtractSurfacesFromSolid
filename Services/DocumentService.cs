using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using System.IO;
using AutoCAD = Autodesk.AutoCAD.DatabaseServices;

namespace Civil3DExtractSurfacesFromSolid.Services
{
    public static class DocumentService
    {
        public static Document ActiveDocument
        {
            get
            {
                return Application.DocumentManager.MdiActiveDocument;
            }
        }

        public static string ActiveDocumentFullPath
        {
            get
            {
                return ActiveDocument.Name;
            }
        }

        public static string DocumentNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Application.DocumentManager?.MdiActiveDocument?.Name);
            }
        }

        public static AutoCAD.Database Database
        {
            get
            {
                return Application.DocumentManager.MdiActiveDocument.Database;
            }
        }

        public static Editor Editor
        {
            get
            {
                return Application.DocumentManager.MdiActiveDocument.Editor;
            }
        }

        public static CivilDocument CivilDocument
        {
            get
            {
                return CivilDocument.GetCivilDocument(Database);
            }
        }

        public static AutoCAD.TransactionManager TransactionManager
        {
            get
            {
                return Database.TransactionManager;
            }
        }

        public static DocumentLock LockActiveDocument()
        {
            return ActiveDocument.LockDocument();
        }
    }
}
