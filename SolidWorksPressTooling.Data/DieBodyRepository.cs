using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorksPressTooling.Data.Contracts;
using SolidWorksPressTooling.Models.Drawings;
using SolidWorksPressTooling.Models.Tooling;

namespace SolidWorksPressTooling.Data
{
    public class DieBodyRepository : IDieBodyRepositories
    {
        public List<DieBody> Dies = new();
        public List<DrawingDimensionPlacement> DieDrawingPlacement = new List<DrawingDimensionPlacement>();

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

        public List<DrawingDimensionPlacement> GetDrawingPlacements()
        {

            List<DrawingDimensionPlacement> DimensionPlacementList = new List<DrawingDimensionPlacement>
                {
                    new DrawingDimensionPlacement
                    {
                        xPositionOne = -(69.85 / 2),
                        yPositionsOne = 22.23,
                        xPositionTwo = 69.85 / 2,
                        yPositionTwo = -22.23,
                        dimX = 0,
                        dimY = 0.65,
                        DimType = "Vertical",
                        EdgeType = "Edges",
                        OperatorChoice = "Double",
                        precisionSize = 0.10
                    },
                    new DrawingDimensionPlacement
                    {
                        xPositionOne = -(69.00 / 2),
                        yPositionsOne = -31.75,
                        xPositionTwo = 69.00 / 2,
                        yPositionTwo = -31.75,
                        dimX = 0,
                        dimY = -80 / 2,
                        DimType = "Vertical",
                        EdgeType = "Edges",
                        OperatorChoice = "Double",
                        precisionSize = 0.10
                    }
                };

            return DimensionPlacementList;
        }
    }
}
