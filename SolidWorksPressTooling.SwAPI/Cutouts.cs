using SldWorks;

namespace SolidWorksPressTooling.SwAPI
{
    public class Cutouts
    {
        Feature swFeature { get; set; }

        public void CreateFeatureCut(ModelDoc2 swModel, double lengthMM, string featureName, bool direction, double d2, bool flip, int T1)
        {
            swFeature = swModel.FeatureManager.FeatureCut4(true, flip, direction, T1, 0, lengthMM / 1000, d2, false, false, false,
                false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

            if(featureName != "")
            {
                swFeature.Name = featureName;
            }

            swModel.ClearSelection2(true);
        }
    }
}
