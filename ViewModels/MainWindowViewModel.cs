using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DatabaseServices;
using Civil3DExtractSurfacesFromSolid.Extensions;
using Civil3DExtractSurfacesFromSolid.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BrepAPI = Autodesk.AutoCAD.BoundaryRepresentation;

namespace Civil3DExtractSurfacesFromSolid.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand SelectSolidsCommand { get; }

        private List<Solid3d> solids = new List<Solid3d>();
        public List<Solid3d> Solids
        {
            get { return solids; }
            set
            {
                solids = value;
                RaisePropertyChanged();
            }
        }

        private string selectSolidsInfo;
        public string SelectSolidsInfo
        {
            get { return selectSolidsInfo; }
            set
            {
                selectSolidsInfo = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand GenerateSurfacesCommand { get; }

        public MainWindowViewModel()
        {
            SelectSolidsCommand = new DelegateCommand(OnSelectSolidsCommand);
            SelectSolidsInfo = "Select";
            GenerateSurfacesCommand = new DelegateCommand(OnGenerateSurfacesCommand);
        }

        private void OnGenerateSurfacesCommand()
        {
            try
            {
                List<BrepAPI.Face> faces = Solids.SelectMany(solid => solid.GetFaces()).ToList();

                using (Transaction transaction = DocumentService.TransactionManager.StartTransaction())
                {
                    foreach (BrepAPI.Face face in faces)
                    {
                        // Check if tin surface can be created
                        if (!face.IsTinSurfaceCanBeCreated(tolerance: Constants.PointCoordinateTolerance))
                        {
                            continue;
                        }

                        // Create surface and add face points
                        ObjectId tinSurfaceId = TinSurface.Create(DocumentService.Database, Guid.NewGuid().ToString());
                        TinSurface tinSurface = transaction.GetObject(tinSurfaceId, OpenMode.ForWrite, false, true) as TinSurface;
                        Point3dCollection facePoints = face.GetSurfaceAsNurb().ControlPoints;
                        tinSurface.AddVertices(facePoints);

                        // Add surface boundary
                        Point3dCollection boundaryPoints = new Point3dCollection(face.GetPoints().ToArray());
                        tinSurface.BoundariesDefinition.AddBoundaries(boundaryPoints, 0.01, Autodesk.Civil.SurfaceBoundaryType.Outer, useNonDestructiveBreakline: true);
                    }

                    transaction.Commit();
                }

                RaiseCloseRequest();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void OnSelectSolidsCommand()
        {
            try
            {
                // Get solids
                Solids = DocumentService.ActiveDocument.PickSolids3d(OpenMode.ForRead);
                SelectSolidsInfo = $"Selected: {solids.Count}";
                GenerateSurfacesCommand.RaiseCanExecuteChanged();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void RaiseCloseRequest()
        {
            WindowCloseHandler?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler WindowCloseHandler;
    }
}