using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOSD_CASE_Tool
{
    /// <summary>
    /// Represents an invisible Bounding Box (or border) around some object(s).
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// Upper Left X Coordinate of this BoundingBox.
        /// </summary>
        public double UpperLeftX { get; set; }

        /// <summary>
        /// Upper Left Y Coordinate of this BoundingBox.
        /// </summary>
        public double UpperLeftY { get; set; }

        /// <summary>
        /// Lower Right X Coordinate of this BoundingBox.
        /// </summary>
        public double LowerRightX { get; set; }

        /// <summary>
        /// Lower Right Y Coordinate of this BoundingBox.
        /// </summary>
        public double LowerRightY { get; set; }

        /// <summary>
        /// A BoundingBox has two pairs of coordinates that defines its boundary.
        /// (x1, y1) defines its upper left corner. (x2, y2) defines the lower right corner.
        /// </summary>
        /// <param name="x1">X Coordinate of the upper left corner.</param>
        /// <param name="y1">Y Coordinate of the upper left corner.</param>
        /// <param name="x2">X Coordinate of the lower right corner.</param>
        /// <param name="y2">Y Coordinate of the lower right corner.</param>
        public BoundingBox(double x1, double y1, double x2, double y2)
        {
            UpperLeftX = x1;
            UpperLeftY = y1;
            LowerRightX = x2;
            LowerRightY = y2;
        }
    }
}
