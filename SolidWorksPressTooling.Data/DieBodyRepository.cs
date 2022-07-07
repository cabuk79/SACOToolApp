using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorksPressTooling.Data.Contracts;
using SolidWorksPressTooling.Models.Tooling;

namespace SolidWorksPressTooling.Data
{
    public class DieBodyRepository : IDieBodyRepositories
    {
        public List<DieBody> Dies = new();

        public DieBodyRepository()
        {
            Dies.Add(new DieBody
            {
                Id = 1,
                BodyOD = 69.85M,
                BodyOAL = 63.50M,
                BodyBottomStepDia = 69.00M,
                BodyBottomStepLength = 5.00M,
                BodyTopStepDia = 63.50M,
                BodyTopStepLength = 9.52M,
                UCutDepth = 1.00M,
                UCutHeight = 0.50M,
                CarbideBoreDepth = 8.00M,
                PressSize = "611",
                BaseName = "Die Body Base",
                MainBodyName = "Main Body",
                TopFaceName = "Die Body Top Face",
                BodyToTopStepFaceName = "Body To Top Step Face",
                EjectorBodyBore = "Ejector Body Bore",
                EjectorHeadBore = "Ejector Head Bore",
                BottomEdge = "Body Bottom Edge"
            });
        }


        public async Task<DieBody> GetDieAsync(string pressSize)
        {
            var die = Dies.Single(s => s.PressSize == pressSize);
            return die;
        }
    }
}
