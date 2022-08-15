using SolidWorksPressTooling.Models.Tooling;
using SolidWorks.Interop.swconst;
using SldWorks;
using SolidWorksPressTooling.Data;

namespace SolidWorksPressTooling.SwAPI
{
    public class Drawings
    {
        SldWorks.SldWorks swApp = (SldWorks.SldWorks)Activator.CreateInstance(System.Type.GetTypeFromProgID("SldWorks.Application"));
        ModelDoc2 swModel { get; set; }
        PartDoc swPart { get; set; }
        Feature swFeature { get; set; }
        PartDoc swComp { get; set; }

        public Drawings()
        {
            Testing connect = new Testing();
            swApp = connect.ConnectToSW();
        }

        public void OpenDrawng()
        {
            DrawingDoc swDrawingDoc = default(DrawingDoc);

            var modelLocation = @"C:\Users\cabuk\Desktop\DieBodyModel.SLDDRW";

            swDrawingDoc = (DrawingDoc)swApp.OpenDoc6(modelLocation, (int)swDocumentTypes_e.swDocDRAWING,
                                        (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel,
                                        "", 0, 0);
        }



        //string modelLocation, double xPositionOne,
        //            double yPositionOne, double xPositionTwo, double yPositionTwo,
        //            double operatorChoice, double dimX, double dimY,
        //            double precisionSize, double dimType, double edgeType

        public void DimensionDrawing(string modelLocation, double xPositionOne,
                    double yPositionOne, double xPositionTwo, double yPositionTwo, 
                    string operatorChoice, double dimX, double dimY, 
                    double precisionSize, string dimType, string edgeType)
        {
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

            swDrawingDoc = (DrawingDoc)swApp.ActiveDoc;

            swView = (View)swDrawingDoc.GetFirstView();
            swView = (View)swView.GetNextView();

            Console.WriteLine(swView.Name);

            //this is the location of the dimension in relationship to the outline of the view
            vOutline = (double[])swView.GetOutline();
            pos = (double[])swView.Position;

            var xDimCentrePos = pos[0];
            var yDimCentrePos = pos[1];

            swModel = (ModelDoc2)swApp.ActiveDoc;

            var xFirstEdge = xDimCentrePos + (xPositionOne / 1000);
            var yFirstEdge = yDimCentrePos + (yPositionOne / 1000) ;

            var xSecondEdge = xDimCentrePos + (xPositionTwo / 1000);
            var ySecondEdge = yDimCentrePos + (yPositionTwo / 1000);


            //TODO: change the Edges to vcertices when required etc.
            swModel.Extension.SelectByRay(xFirstEdge, yFirstEdge, 0, 0, 0, -1, precisionSize / 1000, (int)swSelectType_e.swSelEDGES, false, 0, 0);
                
            if (operatorChoice == "Double")
            {
                swModel.Extension.SelectByRay(xSecondEdge, ySecondEdge, 0, 0, 0, -1, precisionSize / 1000, (int)swSelectType_e.swSelEDGES, true, 0, 0);
            }

            var dimXpos = (dimX / 1000) + xDimCentrePos;
            var dimYpos = (dimY / 1000) + yDimCentrePos; 
            
            if(dimType == "Vertical") //vertical
            {
                swDispDim = (DisplayDimension)swModel.AddVerticalDimension2(dimXpos, dimYpos, 0.00);
            }
            else if(dimType == "Horizontal") //horizontal
            {
                swDispDim = (DisplayDimension)swModel.AddHorizontalDimension2(dimXpos, dimYpos, 0.00);
            }
            
        }
    }
}
