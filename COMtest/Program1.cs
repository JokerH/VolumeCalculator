using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using COMApi = Autodesk.Navisworks.Api.Interop.ComApi;

/// <summary>
/// the verison of selecting a single model item  
/// </summary>
namespace VolumeCalculator
{
    [PluginAttribute("Volume Calculator", "Alex", DisplayName = "Volume Calculator")]
    [AddInPluginAttribute(AddInLocation.AddIn)]
    public class Program : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            //Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            //ModelItemCollection mModelColl1 = oDoc.CurrentSelection.SelectedItems;

            // get the current selection
            ModelItemCollection oModelColl =
                Autodesk.Navisworks.Api.Application.
                    ActiveDocument.CurrentSelection.SelectedItems;



            //foreach (var item in ElbowList)
            StringBuilder Result = new StringBuilder();
            //convert to COM selection
            COMApi.InwOpState oState = ComBridge.State;
            COMApi.InwOpSelection oSel =
                    ComBridge.ToInwOpSelection(oModelColl);

            // create the callback object
            CallbackGeomListener callbkListener =
                    new CallbackGeomListener();

            foreach (COMApi.InwOaPath3 path in oSel.Paths())
            {
                foreach (COMApi.InwOaFragment3 frag in path.Fragments())
                {
                    // generate the primitives
                    frag.GenerateSimplePrimitives(
                        COMApi.nwEVertexProperty.eNORMAL,
                                       callbkListener);

                }
            }

            //string primitiveData = callbkListener.coordinate.ToString();

            //List<List<string>> originalPoints = new List<List<string>>();

            //string[] temp = primitiveData.Split('\r\n',',');
            string[] tempLine = Regex.Split(callbkListener.coordinate.ToString(), "\r\n", RegexOptions.IgnoreCase);

            PrimitiveData primitiveData = new PrimitiveData();
            //primitivePoints.createOriginalPoints(tempLine);
            List<Triangle> OriginalTriangles = primitiveData.createTriangle(tempLine);

            double BaseZvalue = 0;
            foreach (Triangle triangle in OriginalTriangles)
            {
                if (triangle.p1.Z < BaseZvalue)
                {
                    BaseZvalue = triangle.p1.Z;
                }
                if (triangle.p2.Z < BaseZvalue)
                {
                    BaseZvalue = triangle.p2.Z;
                }
                if (triangle.p3.Z < BaseZvalue)
                {
                    BaseZvalue = triangle.p3.Z;
                }
            }

            double Volume = 0;

            foreach (Triangle triangle in OriginalTriangles)
            {
                Volume += triangle.getVolume(BaseZvalue);
            }

            Result.Append("Volume is: " + Volume);
            //Elbow elbow = new Elbow();
            //string Dimensions = elbow.GetDimensions(primitiveData.originalPoints);

            //if (Dimensions != "ERROR")
            //{
            //    Result.Append(ElbowList[i].DisplayName.ToString() + ", " + Dimensions + "\r\n");
            //}
            //else
            //{
            //    errorNum++;
            //    Result.Append(ElbowList[i].DisplayName.ToString() + ", is not applicable to the algorithm."+ "\r\n");
            //}

            //MessageBox.Show(Result.ToString());
            FileStream fs = new FileStream(@"E:\result.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(Result);
            sw.Close();
            fs.Close();


            return 0;
        }
    }
}
