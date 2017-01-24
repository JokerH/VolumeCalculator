using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using COMApi = Autodesk.Navisworks.Api.Interop.ComApi;

namespace VolumeCalculator
{
    public class CallbackGeomListener : COMApi.InwSimplePrimitivesCB
    {

        public StringBuilder coordinate = new StringBuilder();
        //public float[,] points = new float[10000, 3];


        public void Line(COMApi.InwSimpleVertex v1,
            COMApi.InwSimpleVertex v2)
        {
            // do your work
            MessageBox.Show("Line!");
        }

        public void Point(COMApi.InwSimpleVertex v1)
        {
            // do your work
            MessageBox.Show("Point!");
        }

        public void SnapPoint(COMApi.InwSimpleVertex v1)
        {
            // do your work
            MessageBox.Show("SnapPoint!");
        }

        //int i = 0;
        public void Triangle(COMApi.InwSimpleVertex v1,
                COMApi.InwSimpleVertex v2,
                COMApi.InwSimpleVertex v3)
        {
            Array a1 = (Array)(object)v1.coord;
            Array a2 = (Array)(object)v2.coord;
            Array a3 = (Array)(object)v3.coord;
            Array color = (Array)(object)v1.color;
            Array normal = (Array)(object)v1.normal;


            float X1 = (float)(a1.GetValue(1));
            float Y1 = (float)(a1.GetValue(2));
            float Z1 = (float)(a1.GetValue(3));

            float X2 = (float)(a2.GetValue(1));
            float Y2 = (float)(a2.GetValue(2));
            float Z2 = (float)(a2.GetValue(3));

            float X3 = (float)(a3.GetValue(1));
            float Y3 = (float)(a3.GetValue(2));
            float Z3 = (float)(a3.GetValue(3));
            //MessageBox.Show("4: " + X + ", " + Y + ", " + Z);
            //this.points.Add("4: " + X + ", " + Y + ", " + Z);

            float X4 = (float)(normal.GetValue(1));
            float Y4 = (float)(normal.GetValue(2));
            float Z4 = (float)(normal.GetValue(3));

            coordinate.Append(X1.ToString() + "," + Y1.ToString() + "," + Z1.ToString() + ",");
            coordinate.Append(X2.ToString() + "," + Y2.ToString() + "," + Z2.ToString() + ",");
            coordinate.Append(X3.ToString() + "," + Y3.ToString() + "," + Z3.ToString() + ",");
            coordinate.Append(X4.ToString() + "," + Y4.ToString() + "," + Z4.ToString() + "\r\n");

            //points[i, 0] = X1;
            //points[i, 1] = Y1;
            //points[i, 2] = Z1;
            //i++;
            //points[i, 0] = X2;
            //points[i, 1] = Y2;
            //points[i, 2] = Z2;
            //i++;
            //points[i, 0] = X3;
            //points[i, 1] = Y3;
            //points[i, 2] = Z3;
            //i++;
        }
    }
}
