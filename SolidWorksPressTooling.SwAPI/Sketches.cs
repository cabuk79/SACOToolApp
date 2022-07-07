using SldWorks;

namespace SolidWorksPressTooling.SwAPI
{
    public class Sketches
    {
        ModelDoc2 swModel { get; set; }
        PartDoc swPart { get; set; }
        ModelDocExtension swModelDocExt { get; set; }
        SelectionMgr swSelMgr { get; set; }
        Entity swEntity { get; set; }
        SelectData swSelData { get; set; }

        public Sketches(ModelDoc2 SwModel)
        {
            swModel = SwModel;
            swModelDocExt = swModel.Extension;
            swSelMgr = swModel.SelectionManager;
            var SelectedObject = swSelMgr.GetSelectedObject6(1, -1);
            swEntity = SelectedObject;
            swSelData = swSelMgr.CreateSelectData();
        }

        public void CreateCircleSketch(string planeFaceName, string type, double diameterMM, double Y)
        {
            if(type == "PLANE")
            {
                var boolstatus = swModel.Extension.SelectByID2(planeFaceName, type, 0, Y, 0, true, 0, null, 0);
            }

            SketchManager swSketchManager = swModel.SketchManager;
            swSketchManager.InsertSketch(true);
            SketchSegment swSketchSegement = swSketchManager.CreateCircle(0, 0, 0, diameterMM, 0, 0);
        }
    }
}
