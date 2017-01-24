using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeCalculator
{
    class elbowCal
    {
        //All the points form primitive data
        public List<Point> OriginalPoints = new List<Point>();

        public double Distance(Point p1, Point p2)
        {
            double d;
            d =
                Math.Sqrt(Math.Pow((p1.X - p2.X), 2) +
                    Math.Pow((p1.Y - p2.Y), 2) +
                    Math.Pow((p1.Z - p2.Z), 2));
            return d;
        }
    }
}
