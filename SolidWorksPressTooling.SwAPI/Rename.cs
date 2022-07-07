using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SldWorks;
using SolidWorks.Interop.swconst;

namespace SolidWorksPressTooling.SwAPI
{    
    public class Rename
    {
       // public ModelDoc2 swModel { get; set; }
        public SelectionMgr swSelMgr { get; set; }
        public object swSelObject { get; set; }
        public Entity swEntity { get; set; }
        public PartDoc swPartDoc { get; set; }


        public void RenameFace(ModelDoc2 swModel, double worldX, double worldY, double worldZ, double rayX, double rayY, double rayZ, string name)
        {
            swPartDoc = (PartDoc)swModel;

            var boolstatus = swModel.Extension.SelectByRay(worldX, worldY, worldZ, rayX, rayY, rayZ, 0,
                (int)swSelectType_e.swSelFACES, false, 0, (int)swSelectOption_e.swSelectOptionDefault);

            swSelMgr = swModel.SelectionManager;
            swSelObject = swSelMgr.GetSelectedObject6(1, -1);
            swEntity = (Entity)swSelObject;

            boolstatus = swPartDoc.SetEntityName(swEntity, name);
            swModel.ClearSelection2(true);
        }

        public void RenameEdge(ModelDoc2 swModel, string faceName, double X, double Y, double Z, double YDir)
        {
            var boolstatus = swModel.Extension.SelectByRay(X, Y, Z, 0, YDir, 0, 0, (int)swSelectType_e.swSelEDGES, false, 0, (int)swSelectOption_e.swSelectOptionDefault);

            swSelMgr = swModel.SelectionManager;
            swSelObject = swSelMgr.GetSelectedObject6(1, -1);
            swEntity = (Entity)swSelObject;

            boolstatus = swPartDoc.SetEntityName(swEntity, faceName);
            swModel.ClearSelection2(true);
        }
    }
}
