using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationTool_New
{
    class Point
    {
        //Point serial number 
        public int SN;

        //coordinates 
        public double X;
        public double Y;
        public double Z;

        //The points connect to the current points
        //????
        public List<Point> ConnectedPoints = new List<Point>();

        //Label to indicate if the point has been verified on the plane
        public bool Verified = false;
        //Label to indicate if the point has been used to created a plane
        public bool CreatedPlane = false;

        //indicate the circle of point belongs to
        public int circleNumber;

        //Identify value
        //IV = X + Y + Z; 
        public int EqualTo(Point point)
        {
            if (this.X == point.X)
            {
                if (this.Y == point.Y)
                {
                    if (this.Z == point.Z)
                    {
                        return 1;
                    }
                    else return 0;
                }
                else return 0;
            }
            else return 0;
        }

        //public Point Minus(Point point)
        //{
        //    Point Result = new Point();
        //    Result.X = this.X - point.X;
        //    Result.Y = this.Y - point.Y;
        //    Result.Z = this.Z - point.Z;

        //    return Result;
        //}

        //calculate the distance with another point
        public double GetDistance(Point point)
        {
            double distance;

            distance = 
                Math.Sqrt(Math.Pow((X - point.X), 2) +
                    Math.Pow((Y - point.Y), 2) +
                    Math.Pow((Z - point.Z), 2));

            return distance;
        } 

    }
}
