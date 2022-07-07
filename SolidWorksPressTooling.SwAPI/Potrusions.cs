using SldWorks;

namespace SolidWorksPressTooling.SwAPI
{
    public class Potrusions
    {
        Feature swFeature { get; set; }

        public void CreateFeatureExtrusion(ModelDoc2 swModel, decimal lengthMM, string featureName)
        {
            //create the body potrusion
            swFeature = swModel.FeatureManager.FeatureExtrusion3(true, false, false, 0, 0, (double)lengthMM / 1000, 0, false, false,
                false, false, 0, 0, false, false,
                true, true, false, false, false, 0, 0, false);

            if(featureName != "")
            {
                swFeature.Name = featureName;
            }
        }
    }
}
