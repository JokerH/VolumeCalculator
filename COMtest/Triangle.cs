using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;

namespace VolumeCalculator
{
    class Triangle
    {
        Point p1;
        Point p2;
        Point p3;

        Vector3D normalVector = new Vector3D();


        /// <summary>
        /// calculate the volume of the area between the triangle and its projection
        /// </summary>
        /// <param name="BaseZvalue">Z minmum value for the item</param>
        /// <returns></returns> 
        public double getVolume(double BaseZvalue)
        {
            double triangleVolume = 0;
            Point temp = new Point();
            double Zmin = p1.Z;
            if (p2.Z < Zmin)
            {
                p1 = temp;
                p1 = p2;
                p2 = temp;
                Zmin = p1.Z;
            }
            if (p3.Z < Zmin)
            {
                p1 = temp;
                p1 = p3;
                p3 = temp;
                Zmin = p1.Z;
            }
            Point p4 = new Point(); //the projection of p2 on p1.Z-plane
            p4.X = p2.X; p4.Y = p2.Y; p4.Z = p1.Z;
            Point p5 = new Point(); //the projection of p3 on p1.Z-plane
            p5.X = p3.X; p5.Y = p3.Y; p5.Z = p1.Z;

            ///    p2 ----- p3
            ///    |       / |
            ///    |     /   |
            ///    |   /     |
            ///    | /       |
            ///    p4 ----- p5

            triangleVolume = 
                this.getTetrahedronVolume(p1, p2, p3, p4) 
                + this.getTetrahedronVolume(p1, p3, p4, p5) 
                + this.getPrismVolume(p1, p4, p5);

            return triangleVolume;
        }

        private double getTetrahedronVolume(Point P1, Point P2, Point P3, Point P4)
        {
            double volume = 0;

            return volume;
        }

        private double getPrismVolume(Point P1, Point P2, Point P3)
        {
            double volume = 0;

            return volume;
        }

    }
}
