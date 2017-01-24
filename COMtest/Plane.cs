using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace VolumeCalculator
{
    class Plane
    {
        //The function of the plane is Ax+By+Cz+D = 0;
        public double A, B, C, D;

        public List<Point> InnerPoints = new List<Point>();

        //using 3 points to determine a plane
        public bool CreatePlane(Point p1, Point p2, Point p3)
        {
            if (p1 != null && p2 != null && p3 != null)
            {
                this.A = (p2.Y - p1.Y) * (p3.Z - p1.Z) - (p2.Z - p1.Z) * (p3.Y - p1.Y);
                this.B = (p2.Z - p1.Z) * (p3.X - p1.X) - (p2.X - p1.X) * (p3.Z - p1.Z);
                this.C = (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
                this.D = -(this.A * p1.X + this.B * p1.Y + this.C * p1.Z);
                //this.D = 0 - (A * p1.X + B * p1.Y + C * p1.Z);
                return true;
            }
            else
                return false;
        }

        //Detect the point on the plane or not;
        //if the point on the plane, return 1;
        //if not, return 0;
        public int IfOnthePlane(Point P)
        {
            double temp = this.A * P.X + this.B * P.Y + this.C * P.Z + this.D;
            if (this.A * P.X + this.B * P.Y + this.C * P.Z + this.D <= 0.000001)
            {
                return 1;
            }
            else
                return 0;
        }

        //using a point to stand for the normal vector
        public Vector3D NormalVector(List<Point> points)
        {
            Vector3D Nvector = new Vector3D();

            double Px1 = points[0].X; double Py1 = points[0].Y; double Pz1 = points[0].Z;
            double Px2 = points[1].X; double Py2 = points[1].Y; double Pz2 = points[1].Z;
            double Px3 = points[2].X; double Py3 = points[2].Y; double Pz3 = points[2].Z;
            //Nvector.X = Py1 * Pz2 + Py2 * Pz3 + Py3 * Pz1 - Py1 * Pz3 - Py2 * Pz1 - Py3 * Pz2;
            //Nvector.Y = -(Px1 * Pz2 + Px2 * Pz3 + Px3 * Pz1 - Px3 * Pz2 - Px2 * Pz1 - Px1 * Pz3);
            //Nvector.Z = Px1 * Py2 + Px2 * Py3 + Px3 * Py1 - Px1 * Py3 - Px2 * Py1 - Px3 * Py2;

            ////Create 2 vectors by subtracting p3 from p1 and p2
            Vector3D v1 = new Vector3D(Px1 - Px3, Py1 - Py3, Pz1 - Pz3);
            Vector3D v2 = new Vector3D(Px2 - Px3, Py2 - Py3, Pz2 - Pz3);
            //Create cross product from the 2 vectors
            Nvector = Vector3D.CrossProduct(v1, v2);

            return Nvector;
        }

    }
}
