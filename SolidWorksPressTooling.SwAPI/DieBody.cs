using SldWorks;
using SolidWorksPressTooling.Data;
using SolidWorksPressTooling.Models.Tooling;
using SolidWorks.Interop.swconst;
using SolidWorksPressTooling.Models.Drawings;
using System.Diagnostics;

namespace SolidWorksPressTooling.SwAPI
{
    public class DieBodySW
    {
        SldWorks.SldWorks swApp = (SldWorks.SldWorks)Activator.CreateInstance(System.Type.GetTypeFromProgID("SldWorks.Application"));
        ModelDoc2 swModel { get; set; }
        PartDoc swPart { get; set; }
        Feature swFeature { get; set; }

        PartDoc swComp { get; set; }

        public DieBody dieSizes { get; set; } = new();
        DieBodyRepository dieRepo = new DieBodyRepository();

        private double _blankDia;
        private double _dieDia;

        public DieBodySW(double blankDia, double dieDia, double dieRad, double punchRad, double drawLength)
        {

            GetDieSizes();
            _blankDia = blankDia;
            _dieDia = dieDia;

            //Connect to SW
            Testing connect = new Testing();
            swApp = connect.ConnectToSW();
        }

        public async void GetDieSizes()
        {
            dieSizes = await dieRepo.GetDieAsync("611");
        }

        public void CreateDieBody()
        {
            Rename rename = new Rename();
            Potrusions createBodyFeature = new Potrusions();
            Cutouts createCutout = new Cutouts();
            SelectFace selectFace = new SelectFace();

            var templatePath = @"C:\Users\cabuk\Desktop\SACO Metal and Insert.prtdot";
            swModel = swApp.NewDocument(templatePath, 0, 0, 0);

            //create the body dia sketch
            Sketches circleSketches = new Sketches(swModel);
            circleSketches.CreateCircleSketch("Top Plane", "PLANE", ((double)dieSizes.BodyOD / 2) / 1000, 0);

            //create the body potrusion            
            createBodyFeature.CreateFeatureExtrusion(swModel, dieSizes.BodyOAL, dieSizes.MainBodyName);
            swModel.ClearSelection2(true);

            //Rename current bodies and faces created
            rename.RenameFace(swModel, ((double)dieSizes.BodyOD / 1000) / 2, 0.01, 0, 0.1, 0.1, 0.1, dieSizes.MainBodyName);
            rename.RenameFace(swModel, 0, 0, 0, 0, 1, 0, dieSizes.BaseName);
            rename.RenameFace(swModel, 0, (double)dieSizes.BodyOAL / 1000, 0, 0, 1, 0, dieSizes.TopFaceName);

            //Create bottom diameter
            selectFace.SelectFaceByName(dieSizes.BaseName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", ((double)dieSizes.BodyBottomStepDia / 2) / 1000, 0);
            createCutout.CreateFeatureCut(swModel, (double)dieSizes.BodyBottomStepLength, "Bottom Diameter Step", false, 0.01, true, 0);

            //Create the top diameter step
            selectFace.SelectFaceByName(dieSizes.TopFaceName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", ((double)dieSizes.BodyTopStepDia / 2) / 1000, (double)dieSizes.BodyOAL / 1000);
            createCutout.CreateFeatureCut(swModel, (double)dieSizes.BodyTopStepLength, "Top Step", false, 0.01, true, 0);

            //Rename the face where the body and top diameter meet
            var posX = (double)((dieSizes.BodyTopStepDia + ((dieSizes.BodyOD - dieSizes.BodyTopStepDia) / 2)) / 1000) / 2;
            var posY = (double)(dieSizes.BodyOAL - dieSizes.BodyTopStepLength) / 1000;
            rename.RenameFace(swModel, posX, posY, 0, 0, 1, 0, dieSizes.BodyToTopStepFaceName);

            //Create the undercut from the body to top face face
            selectFace.SelectFaceByName(dieSizes.BodyToTopStepFaceName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", (double)((dieSizes.BodyTopStepDia - dieSizes.UCutDepth) / 2) / 1000, 0);
            createCutout.CreateFeatureCut(swModel, (double)dieSizes.UCutHeight, "Body Top Face Undercut", true, 0, true, 0);

            //create the bore for the die insert
            selectFace.SelectFaceByName(dieSizes.TopFaceName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", ((_blankDia + 2) / 2) / 1000, 0);
            createCutout.CreateFeatureCut(swModel, (double)dieSizes.CarbideBoreDepth, "Carbide Insert Bore", false, 0.01, false, 0);
            rename.RenameFace(swModel, 0, (double)(dieSizes.BodyOAL - dieSizes.CarbideBoreDepth) / 1000, 0, 0, 1, 0, "Carbide Bore Base Face");

            //Create bore for ejector body
            selectFace.SelectFaceByName(dieSizes.BaseName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", ((_dieDia + 0.5) / 2) / 1000, 0);
            createCutout.CreateFeatureCut(swModel, 0, "Ejector Body Bore", false, 0.01, false, 1);
            rename.RenameFace(swModel, (double)((_dieDia + 0.5) / 2) / 1000, 0.01, 0, 0.1, 0.1, 0.1, dieSizes.EjectorBodyBore);

            //TODO: need to check the size to see if a head bore is needed
            //create the bore for the ejector head
            selectFace.SelectFaceByName(dieSizes.BaseName, swModel);
            circleSketches.CreateCircleSketch("", "FACES", ((_dieDia + 7) / 2) / 1000, 0);
            createCutout.CreateFeatureCut(swModel, 5, "Ejector Head Bore", false, 0.01, false, 0);
            rename.RenameFace(swModel, (double)((_dieDia + 7) / 2) / 1000, 2 / 1000, 0, 0.1, 0.1, 0.1, dieSizes.EjectorBodyBore);

            //rename the edges for the chamfers
            rename.RenameEdge(swModel, dieSizes.BottomEdge, (double)(dieSizes.BodyBottomStepDia / 2) / 1000, 0, 0, 1);
        }

        public void CreateDrg()
        {
            Drawings drg = new Drawings();

            


            drg.OpenDrawng();

            DrawingDoc swDrawingDoc = default(DrawingDoc);


            swModel = (ModelDoc2)swApp.ActiveDoc;
            swDrawingDoc = (DrawingDoc)swApp.ActiveDoc;

            var oneone = 2000 / 1000;
            var twotwo = 2000 / 1000;
            double one = 2000 / 1000;// ((200.0 / 4.0) / 1000.0);
            double two = 2000 / 1000; //((200.0 / 4.0) / 1000.0);

            swModel.SketchManager.CreateCornerRectangle(-one, -two, 0, one, two, 0);


            var depth = 34.925 / 1000;
            swDrawingDoc.CreateBreakOutSection(depth);

            List<DrawingDimensionPlacement> ListOfDims = new List<DrawingDimensionPlacement>();
            ListOfDims = dieRepo.GetDrawingPlacements();

            
            //loop through the horizontal and vertical dimensions and place them
            foreach(var dim in ListOfDims)
            {
                    drg.DimensionDrawing("", dim.xPositionOne, dim.yPositionsOne, dim.xPositionTwo,
                        dim.yPositionTwo, dim.OperatorChoice, dim.dimX, dim.dimY, dim.precisionSize,
                        dim.DimType, dim.EdgeType);
            }


    }    

    

    //var countRows = dimensionPositions.Length / 9;

    //for (var i = 0; i < countRows; i++)
    //{
    //    drg.DimensionDrawing("", dimensionPositions[i, 0], dimensionPositions[i, 1],
    //        dimensionPositions[i, 2], dimensionPositions[i, 3], dimensionPositions[i, 4], dimensionPositions[i, 5],
    //        dimensionPositions[i, 6], dimensionPositions[i, 7], dimensionPositions[i, 8]); //("", 0, 0, 0, 0, 0, 0, dimensionPositions);
    //}




    public void CreateDrawing()
        {           
            var modelLocation = @"C:\Users\cabuk\Desktop\DieBodyModel.SLDDRW";

            DrawingDoc swDrawingDoc = default(DrawingDoc); // swModel;
            SelectionMgr swSelMgr = default(SelectionMgr);
            Entity swEnt = default(Entity);
            Entity swEnt1 = default(Entity);
            ModelDoc2 swDrawModel = default(ModelDoc2);
            ModelDoc2 swModel = default(ModelDoc2);
            bool bRet = false;
            bool bRet1 = false;
            View swView = default(View);
            double[] vOutline = null;
            double[] pos = null;
            DisplayDimension swDispDim = default(DisplayDimension);
            double nXpos = 0;
            double nYpos = 0;
            SketchManager swKsethMgr = default(SketchManager);
            

            //swModel = (ModelDoc2)swApp.ActiveDoc;
            //swSelMgr = (SelectionMgr)swModel.SelectionManager;
            //swEnt = (Entity)swSelMgr.GetSelectedObject6(1, -1);
            //swEnt1 = (Entity)swSelMgr.GetSelectedObject6(2, -1);

            swDrawingDoc = (DrawingDoc)swApp.OpenDoc6(modelLocation, (int)swDocumentTypes_e.swDocDRAWING, 
                                        (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel,
                                        "", 0, 0);

            //swDrawModel = (ModelDoc2)swDrawingDoc;  
            swView = (View)swDrawingDoc.GetFirstView();
            swView = (View)swView.GetNextView();
         
            //swView.SelectEntity(swEnt, true);
            //swView.SelectEntity(swEnt1, true);

            //this is the location of the dimension in relationship to the outline of the view
            vOutline = (double[])swView.GetOutline();
            pos = (double[])swView.Position;

            var xDimCentrePos = pos[0];
            var yDimCentrePos = pos[1];

            //var xFirstEdge = xDimCentrePos - ((63.50 / 2) / 1000);
            //var yFirstEdge = ((63.50 / 2) / 1000) + yDimCentrePos;

            //var xSecondEdge = xDimCentrePos + ((63.50 / 2) / 1000); 
            //var ySecondEdge = ((63.50 / 2) / 1000) + yDimCentrePos;

            var xFirstEdge = xDimCentrePos - ((69.00 / 2) / 1000);
            var yFirstEdge = yDimCentrePos - ((63.50 / 2) / 1000) ; //ok

            var xSecondEdge = xDimCentrePos + ((63.50 / 2) / 1000);
            var ySecondEdge = yDimCentrePos + ((63.50 / 2) / 1000);//ok


            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.ClearSelection2(true);

            //swModel.Extension.SelectByRay(xFirstEdge, yFirstEdge, 0, 0, 0, -1, 1.3362457669207E-03, (int)swSelectType_e.swSelEDGES, false, 0, 0);
            //swModel.Extension.SelectByRay(xSecondEdge, ySecondEdge, 0, 0, 0, -1, 1.3362457669207E-03, (int)swSelectType_e.swSelEDGES, true, 0, 0);

            swModel.Extension.SelectByRay(xFirstEdge, yFirstEdge, 0, 0, 0, -1, 1.3362457669207E-03, (int)swSelectType_e.swSelEDGES, false, 0, 0);
            swModel.Extension.SelectByRay(xSecondEdge, ySecondEdge, 0, 0, 0, -1, 1.3362457669207E-03, (int)swSelectType_e.swSelEDGES, true, 0, 0);

            
            swDispDim = (DisplayDimension)swModel.AddDimension2(0.1366, 0.2039, 0);
            
            Console.WriteLine(swView.Name);

            //swModel.ClearSelection2(true);

            //SketchSegment skSegment;
            //SelectData selectData = null;
            swDrawingDoc = swApp.ActiveDoc;

            //create section view
            var oneone = 2000 / 1000;
            var twotwo = 2000 / 1000;
            double one = ((200.0 / 4.0) / 1000.0);
            double two = ((200.0 / 4.0) / 1000.0);
            //swModel.SketchManager.CreateCornerRectangle(-oneone, -twotwo, 0, one, two, 0);
            swModel.SketchManager.CreateCornerRectangle(-one, -two, 0, one, two, 0);
            //skSegment.Select4(true, selectData);
            //swModel.EditSketch();
            

            var depth = 34.925 / 1000;
            swDrawingDoc.CreateBreakOutSection(depth);
       
          

            //nXpos = (vOutline[0] + vOutline[2]) / 2.0;
           // nYpos = vOutline[3] + 0.01;

            //swModel.Extension.SelectByRay()
            //swModel.AddDimension2(nXpos, nYpos, 0);

           // swDispDim = (DisplayDimension)swDrawModel.Extension.AddDimension(nXpos, nYpos, 0.0, 
           //                 (int)swSmartDimensionDirection_e.swSmartDimensionDirection_Left);

            if(nXpos != 0)
            {
                return;
            }




            //ModelDoc2 swDrawModel = default(ModelDoc2);
            //swDrawModel = (ModelDoc2)swModel;

            ////DrawingDoc swDrawingDoc = (DrawingDoc)swModel;
            //ModelDocExtension swModelDocExt = swModel.Extension;

            //var status = swDrawingDoc.ActivateView("Drawing View1");
            ////SelectFace selectFace = new SelectFace();
            ////selectFace.SelectFaceByName(dieSizes.BaseName, )
            //SldWorks.View swView = swDrawingDoc.GetFirstView();

            //var bRet = swView.SelectEntity(status, false);
            //double[] vOutline = null;
            //vOutline = (double[])swView.GetOutline();
            //var nXpos = (vOutline[0] + vOutline[2]) / 2.0;
            //var nYpos = vOutline[3] + 0.01;

            //DisplayDimension swDispDim = default(DisplayDimension);
            //swDispDim = (DisplayDimension)swDrawModel.Extension.AddDimension(nXpos, nYpos, 0.0,
            //                    (int)swSmartDimensionDirection_e.swSmartDimensionDirection_Left);
        }
    }
}
