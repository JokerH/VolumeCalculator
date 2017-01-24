using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeCalculator
{
    class CentralLine
    {
        public List<Point> CentralPoints = new List<Point>();

        //public double Length
        public double CentralLineLength()
        {
            double length = 0;
            //for (int i = 0; i < CentralPoints.Count; i++)
            for (int i = 0; i < CentralPoints.Count - 1; i++)
            {
                length += Distance(CentralPoints[i], CentralPoints[i + 1]);
            }
            return length;
        }

        ////get the angle of central line
        //public double CalculateAngle()
        //{
        //    double angle;

        //    // p1, p2 are 2 vectors
        //    Point p1 = new Point();
        //    Point p2 = new Point();

        //    p1.X = CentralPoints[0].X - CentralPoints[1].X;
        //    p1.Y = CentralPoints[0].Y - CentralPoints[1].Y;
        //    p1.Y = CentralPoints[0].Z - CentralPoints[1].Z;

        //    int n = CentralPoints.Count;
        //    //p2.X = CentralPoints[CentralPoints.Count].X - CentralPoints[(CentralPoints.Count-1)].X;
        //    //p2.Y = CentralPoints[CentralPoints.Count].Y - CentralPoints[(CentralPoints.Count-1)].Y;
        //    //p2.Y = CentralPoints[CentralPoints.Count].Z - CentralPoints[(CentralPoints.Count-1)].Z;
        //    p2.X = CentralPoints[n-1].X - CentralPoints[n - 2].X;
        //    p2.Y = CentralPoints[n-1].Y - CentralPoints[n-2].Y;
        //    p2.Y = CentralPoints[n-1].Z - CentralPoints[n-2].Z;

        //    ///angel θ
        //    ///cosθ = p1*p2/(|p1|*|p2|)
        //    ///p1*p2 = x1*x2 + y1*y2 + z1*z2
        //    ///|p1| = √(x1^2 + y1^2 +z1^2)
        //    ///|p1| = √(x2^2 + y2^2 +z2^2)
        //    ///θ = arc cosθ
        //    double cos =
        //        (p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z) /
        //        (Math.Sqrt((p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z) * (p2.X * p2.X + p2.Y * p2.Y + p2.Z * p2.Z)));
        //    angle = Math.Acos(cos);
        //    //radian to degree
        //    angle = 180 / Math.PI * angle;

        //    return angle;
        //}

        double Distance(Point p1, Point p2)
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
