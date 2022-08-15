using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksPressTooling.Models.Drawings
{
    public class DrawingDimensionPlacement
    {
        public string DimensionName { get; set; }
        public string DimensionDescription { get; set; }
        public double xPositionOne { get; set; }
        public double yPositionsOne { get; set; }
        public double xPositionTwo { get; set; }
        public double yPositionTwo { get; set; }
        public double dimX { get; set; }
        public double dimY { get; set; }
        public string OperatorChoice { get; set; }
        public double precisionSize { get; set; }
        public string DimType { get; set; }
        public string EdgeType { get; set; }

    }
}
