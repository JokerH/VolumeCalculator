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

namespace VolumeCalculator
{
    [PluginAttribute("QElbow1201", "Alex", DisplayName = "QElbow1201")]
    [AddInPluginAttribute(AddInLocation.AddIn)]
    public class Program : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            //MessageBox.Show("Start to retrieve the selection tree.");

            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            ModelItemCollection mModelColl1 = oDoc.CurrentSelection.SelectedItems;

            // get the current selection
            //ModelItemCollection oModelColl =
            //    Autodesk.Navisworks.Api.Application.
            //        ActiveDocument.CurrentSelection.SelectedItems;

            #region test
            //IEnumerable<ModelItem> items = oDoc.Models[0].RootItem.DescendantsAndSelf.AsEnumerable<ModelItem>;


            //string fileName = @"D:\GoogleDrive\Francis_Alex\FHSE-3D-MODEL_2016-01-26.nwd";
            //DocumentInfo documentInfo = Autodesk.Navisworks.Api.Application.TryLoadDocumentInfo(fileName);
            //if (documentInfo != null)
            //{
            //    foreach (SavedItem item in documentInfo.Children)
            //    {
            //        SheetInfo sheetInfo = item as SheetInfo;

            //        if (sheetInfo != null)
            //        {
            //            if (Autodesk.Navisworks.Api.Application.ActiveDocument.TryAppendSheet(sheetInfo))
            //            {
            //                MessageBox.Show(string.Format("Appended the '{0}' Sheet", item.DisplayName));
            //            }
            //        }
            //    }
            //}
            //MessageBox.Show("1");
            #endregion

            //get specific model in the document
            Model model = oDoc.Models[0];
            ModelItem modelItem = model.RootItem;
            List<ModelItem> ItemsList = new List<ModelItem>();
            List<ModelItem> LowestItemsList = new List<ModelItem>();

            foreach (var item in modelItem.Children)
            {

                if (item.DisplayName == "FHSE_L1.rvm")
                {
                    MessageBox.Show(string.Format("Find the piping file: {0}", item.DisplayName));
                    modelItem = item;
                    //MessageBox.Show(modelItem.DisplayName);
                    break;
                }
            }

            ///write down all the displaynames of items in a txt file
            //FileStream fs = new FileStream(@"D:\temp\ELBOW.txt", FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);

            ///Tree traversal Problem 
            ///Find the lowest items whose name contains "tube"/"elbow"
            ItemsList.Add(modelItem);
            //foreach (ModelItem Item in ItemsList)\
            for (int i = 0; i < ItemsList.Count; i++)
            //for (int i = 0; i < 5; i++)
            {
                if (ItemsList[i].IsHidden == false)
                {
                    ///Method 1
                    //if (ItemsList[i].Children.First != null)
                    //{
                    //    foreach (ModelItem item in ItemsList[i].Children)
                    //    {
                    //        ItemsList.Add(item);
                    //    }
                    //}
                    //else
                    //{
                    //    LowestItemsList.Add(ItemsList[i]);
                    //}

                    ///Method 2
                    if (ItemsList[i].Children.First == null)
                    {
                        LowestItemsList.Add(ItemsList[i]);
                        //sw.WriteLine(ItemsList[i].DisplayName);
                    }
                    else
                    {
                        foreach (ModelItem item in ItemsList[i].Children)
                        {
                            ItemsList.Add(item);
                        }
                    }
                }

            }
            MessageBox.Show(LowestItemsList.Count.ToString()+" model items found in the current model.");

            //ModelItemCollection oModelColl = new ModelItemCollection();
            List<ModelItem> ElbowList = new List<ModelItem>();
            //int num = 0;
            foreach (var item in LowestItemsList)
            {
                if (item.DisplayName.IndexOf("ELBOW") >= 0)
                {
                    //sw.WriteLine("{0}: The display name is '{1}'", num, item.DisplayName);
                    //num++;
                    ElbowList.Add(item);
                }
            }

            //sw.Close();
            //fs.Close();

            //Console.WriteLine("1");


            //foreach (var item in ElbowList)
            StringBuilder Result = new StringBuilder();
            int errorNum = 0;
            for (int i = 0; i < ElbowList.Count; i++)
            {
                ModelItemCollection oModelColl = new ModelItemCollection();
                //oModelColl.Add(item);
                oModelColl.Add(ElbowList[i]);
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

                string primitiveData = callbkListener.coordinate.ToString();

                //List<List<string>> originalPoints = new List<List<string>>();

                //string[] temp = primitiveData.Split('\r\n',',');
                string[] tempLine = Regex.Split(primitiveData, "\r\n", RegexOptions.IgnoreCase);

                #region//Obtain original points
                //foreach (string line in tempLine)
                //{
                //    if (line != "")
                //    {
                //        string[] temp = line.Split(',');

                //        {
                //            for (int i = 0; i < 9; i += 3)
                //            {

                //                List<string> list = new List<string>();
                //                for (int j = i; j < i + 3; j++)
                //                {
                //                    list.Add(temp[j]);
                //                }
                //                originalPoints.Add(list);

                //            }
                //        }
                //    }
                //}
                #endregion

                #region//delete the duplicate points in the list
                ////http://blog.sina.com.cn/s/blog_6148930e0100odqx.html

                //List<List<string>> newPoints = new List<List<string>>();

                //newPoints.Add(originalPoints[0]);
                //int flag = 0;

                //for (int i = 0; i < originalPoints.Count; i++)
                //{
                //    flag = 0;

                //    for (int j = 0; (j < newPoints.Count) && (flag != 1); j++)
                //    {
                //        //if ((points[i][0] != Npoints[j][0]) && (points[i][1] != Npoints[j][1]) && (points[i][2] != Npoints[j][2]))
                //        if ((originalPoints[i][0] == newPoints[j][0]) && (originalPoints[i][1] == newPoints[j][1]) && (originalPoints[i][2] == newPoints[j][2]))
                //        {
                //            //if (!(points[i][1] == Npoints[j][1]))
                //            //{
                //            //    if (!(points[i][2] == Npoints[j][2]))
                //            //    {
                //            //        Npoints.Add(points[i]);
                //            //        k++;
                //            //    }
                //            //}
                //            flag = 1;
                //        }
                //    }

                //    if (flag == 0)
                //    {
                //        newPoints.Add(originalPoints[i]);
                //    }
                //}
                #endregion

                PrimitivePoints primitivePoints = new PrimitivePoints();
                primitivePoints.createOriginalPoints(tempLine);

                Elbow elbow = new Elbow();
                string Dimensions = elbow.GetDimensions(primitivePoints.originalPoints);

                if (Dimensions != "ERROR")
                {
                    Result.Append(ElbowList[i].DisplayName.ToString() + ", " + Dimensions + "\r\n");
                }
                else
                {
                    errorNum++;
                    Result.Append(ElbowList[i].DisplayName.ToString() + ", is not applicable to the algorithm."+ "\r\n");
                }
            }
            //MessageBox.Show(Result.ToString());
            FileStream fs = new FileStream(@"D:\result.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(Result);
            sw.Close();
            fs.Close();


            return 0;
        }
    }
}
