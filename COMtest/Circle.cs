using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeCalculator
{
    class Circle
    {
        //Circle serial number
        public int SN;

        //Points in the circle
        public List<Point> InnerPoints = new List<Point>();

        ////The circlesID connect to the current circle
        //public List<int> ConnectedCirclesID = new List<int>();

        //The circles connect to the current circle
        public List<Circle> ConnectedCircles = new List<Circle>();

        public Point CentralPoint;
        //Calculate central point

        public Point CPcalculation()
        {
            Point cpoint = new Point();

            double sumX = 0, sumY = 0, sumZ = 0;
            foreach (Point point in InnerPoints)
            {
                sumX += point.X;
                sumY += point.Y;
                sumZ += point.Z;
            }
            cpoint.X = sumX / InnerPoints.Count;
            cpoint.Y = sumY / InnerPoints.Count;
            cpoint.Z = sumZ / InnerPoints.Count;

            return cpoint;
        }
    }
}
