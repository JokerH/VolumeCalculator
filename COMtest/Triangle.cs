using System.Windows.Media.Media3D;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace VolumeCalculator
{
    class Triangle
    {
        public Point p1 = new Point();
        public Point p2 = new Point();
        public Point p3 = new Point();

        public Vector3D normalVector = new Vector3D();


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

            Point[] Tetrahedron1 = { p1, p2, p3, p4 };
            Point[] Tetrahedron2 = { p1, p3, p4, p5 };
            Point[] Prism = { p1, p4, p5 };

            triangleVolume =
                getTetrahedronVolume(Tetrahedron1)
                + getTetrahedronVolume(Tetrahedron2)
                + getPrismVolume(Prism, BaseZvalue);

            int flag = 1;
            Vector3D Positive = new Vector3D(0, 0, 1);
            if (Vector3D.DotProduct(Positive, normalVector) <= 0)
                flag = -1;

            return triangleVolume*flag;
        }

        //private double getTetrahedronVolume(Point P1, Point P2, Point P3, Point P4)
        private double getTetrahedronVolume(Point[] points)
        {
            double volume = 0;
            DenseMatrix matrix = new DenseMatrix(4);
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                matrix[0, i] = 1;
                matrix[1, i] = points[i].X;
                matrix[2, i] = points[i].Y;
                matrix[3, i] = points[i].Z;
            }
            volume = System.Math.Abs(matrix.Determinant()) / 6;
             return volume;
            //to be checked
        }

        //private double getPrismVolume(Point P1, Point P2, Point P3)
        private double getPrismVolume(Point[] points, double BaseZvalue)
        {
            double volume = 0;
            double height = points[0].Z - BaseZvalue;

            //Heron's formula: S = sqrt(p*(p-a)*(p-b)*(p-c))
            double a = points[0].GetDistance(points[1]);
            double b = points[0].GetDistance(points[2]);
            double c = points[1].GetDistance(points[2]);
            double p = (a + b + c) / 2;

            volume = height * System.Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            return volume;
        }
        ///it has been checked that the area of triangle is correct. 
    }
}
