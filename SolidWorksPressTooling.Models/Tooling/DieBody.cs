using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksPressTooling.Models.Tooling
{
    public class DieBody
    {
        public int Id { get; set; }
        public decimal BodyOD { get; set; }
        public decimal BodyOAL { get; set; }
        public decimal BodyBottomStepDia { get; set; }
        public decimal BodyBottomStepLength { get; set; }
        public decimal BodyTopStepDia { get; set; }
        public decimal BodyTopStepLength { get; set; }
        public decimal CarbideBoreDepth { get; set; }
        public decimal UCutDepth { get; set; }
        public decimal UCutHeight { get; set; }
        public string PressSize { get; set; }
        public string BaseName { get; set; }
        public string MainBodyName { get; set; }
        public string TopFaceName { get; set; }
        public string BodyToTopStepFaceName { get; set; }
        public string EjectorBodyBore { get; set; }
        public string EjectorHeadBore { get; set; }
        public string BottomEdge { get; set; }
    }
}
