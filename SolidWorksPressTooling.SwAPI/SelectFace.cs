using SldWorks;
using SolidWorks.Interop.swconst;


namespace SolidWorksPressTooling.SwAPI
{
    public class SelectFace
    {
        ModelDoc2 swModel { get; set; }
        Face2 swFace { get; set; }
        PartDoc swPart { get; set; }
        Entity swEntity { get; set; }
        SelectData swSelData { get; set; }


        public void SelectFaceByName(string faceName, ModelDoc2 swModel)
        {
            swPart = (PartDoc)swModel;
            var swModelDocExt = swModel.Extension;

            swFace = swPart.GetEntityByName(faceName, (int)swSelectType_e.swSelFACES);
            swEntity = (Entity)swFace;
            var boolstatus = swEntity.Select4(true, swSelData);
        }

    }
}
