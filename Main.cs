using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace Civil3DExtractSurfacesFromSolid
{
    public class Program
    {
        [CommandMethod("PSV", "Civil3DExtractSurfacesFromSolid", CommandFlags.Modal)]
        public static void MainProgram()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            MainWindow form = new MainWindow();
            adoc.LockDocument();
            form.ShowDialog();

            return;
        }
    }
}






