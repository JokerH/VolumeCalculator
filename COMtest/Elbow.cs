using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Forms;

namespace VolumeCalculator
{
    class Elbow
    {
        public string GetDimensions(List<Point> OriginalPoints)
        {
            bool errorGroup = false;
            //primitiveData PD = new primitiveData();
            elbowCal elbowcal_1 = new elbowCal();
            elbowcal_1.OriginalPoints = OriginalPoints;

            //#region Dense-based spatial clustering
            //need to make sure the start point is not the central of a circle!!!

            Point startPoint = elbowcal_1.OriginalPoints[2];
            //startPoint.Used = true;
            //elbowcal_1.OriginalPoints[0];
            //elbowcal_1.OriginalPoints.RemoveAt(0);
            double MinD = elbowcal_1.Distance(startPoint, elbowcal_1.OriginalPoints[3]);

            /// P0, P1, P2 might be the minD 
            for (int i = 0; i < elbowcal_1.OriginalPoints.Count; i++)
            {
                double distance = elbowcal_1.Distance(startPoint, elbowcal_1.OriginalPoints[i]);

                if (distance < MinD && distance != 0)
                {
                    MinD = distance;
                }
            }

            //Cluster points based on density and MinD
            List<Circle> circleList = new List<Circle>();
            //MinD multiply by a factor 1.2
            double Density = MinD * 1.01;
            //double MinDensity = MinD * 0.99;

            //int circleNum = 0;
            while (elbowcal_1.OriginalPoints.Count > 0)
            {
                Circle circle = new Circle();
                circle.SN = circleList.Count;
                circleList.Add(circle);
                startPoint = elbowcal_1.OriginalPoints[0];

                circle.InnerPoints.Add(startPoint);
                //startPoint.circleNumber = circle.SN;
                elbowcal_1.OriginalPoints.RemoveAt(0);

                ///find all points should be grouped in on circle
                ///Stop when no points' distance less than the Density
                for (int i = 0; i < elbowcal_1.OriginalPoints.Count; i++)
                {
                    double distance = elbowcal_1.Distance(startPoint, elbowcal_1.OriginalPoints[i]);
                    if (distance <= Density)
                    //if (distance < MaxDensity && distance > MinDensity)
                    {
                        startPoint = elbowcal_1.OriginalPoints[i];

                        circle.InnerPoints.Add(startPoint);
                        //startPoint.circleNumber = circle.SN;
                        elbowcal_1.OriginalPoints.RemoveAt(i);
                        ///start point has renewed, so i should start from 0;
                        i = 0;
                    }
                }
            }

            // Delete the group contains less than 3 points
            for (int i = 0; i < circleList.Count; i++)
            {
                //judge if it is a circle
                if (circleList[i].InnerPoints.Count < 3)
                {
                    circleList.RemoveAt(i);
                    //i--; //This will cause an error if i=0; 
                    i = 0;
                }
            }

            ///Verify the group by check if all points on the same plane
            ///
            #region OldPlaneBased
            ////Create the circleplanes list
            //List<CirclePlane> cPlaneList = new List<CirclePlane>();
            //Console.Write(1);
            //PD.openfile(@"E:\temp\origin1.txt");
            //while (PD.originalPoints.Count > 3)
            //{

            //    //cPlane.CreatePlane(PD.originalPoints[0], PD.originalPoints[1], PD.originalPoints[2]);
            //    for (int i = 0; i < PD.originalPoints.Count; i++)
            //    {
            //        CirclePlane cPlane = new CirclePlane();
            //        Point[] CreatPlanePoints = new Point[3];
            //        if (PD.originalPoints[i].Classified == false && PD.originalPoints[i].CreatedPlane == false)
            //        {
            //            CreatPlanePoints[0] = PD.originalPoints[i];
            //            PD.originalPoints[i].CreatedPlane = true;
            //            for (int j = 1; i + j < PD.originalPoints.Count; j++)
            //            {
            //                if (PD.originalPoints[i + j].Classified == false && PD.originalPoints[i + j].CreatedPlane == false)
            //                {
            //                    CreatPlanePoints[1] = PD.originalPoints[i + j];
            //                    PD.originalPoints[i + j].CreatedPlane = true;
            //                    for (int k = 1; i + j + k < PD.originalPoints.Count; k++)
            //                    {
            //                        if (PD.originalPoints[i + j + k].Classified == false && PD.originalPoints[i + j + k].CreatedPlane == false)
            //                        {
            //                            CreatPlanePoints[2] = PD.originalPoints[i + j + k];
            //                            PD.originalPoints[i + j + k].CreatedPlane = true;
            //                            break;
            //                        }
            //                    }
            //                    break;
            //                }
            //            }

            //            if (cPlane.CreatePlane(CreatPlanePoints[0], CreatPlanePoints[1], CreatPlanePoints[2]))
            //            {
            //                ///the plane generated
            //                ///detect points on the plane
            //                foreach (Point point in PD.originalPoints)
            //                {
            //                    if (point.Classified == false)
            //                    {
            //                        //if the point on the plane
            //                        if (cPlane.IfOnthePlane(point) > 0)
            //                        {
            //                            cPlane.InnerPoints.Add(point);
            //                        }

            //                    }
            //                }
            //                if (cPlane.InnerPoints.Count > 5)
            //                {
            //                    cPlaneList.Add(cPlane);
            //                    foreach (Point point in cPlane.InnerPoints)
            //                    {
            //                        point.Classified = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    int counter = 0;
            //    foreach (Point point in PD.originalPoints)
            //    {
            //        if (point.Classified == false)
            //        {
            //            counter++;
            //        }
            //    }
            //    if (counter > 3)
            //    {
            //        MessageBox.Show("More than 3 points have not been classified.");
            //    }
            //    else
            //        break;

            //}

            #endregion

            #region NewPlaneBased
            List<Circle> VerifiedCircle = new List<Circle>();
            foreach (Circle circle in circleList)
            {

                for (int i = 0; i < circle.InnerPoints.Count; i++)
                {
                    Plane cPlane = new Plane();
                    Point[] CreatPlanePoints = new Point[3];
                    if (circle.InnerPoints[i].Verified == false && circle.InnerPoints[i].CreatedPlane == false)
                    {
                        //Find the first point
                        CreatPlanePoints[0] = circle.InnerPoints[i];
                        circle.InnerPoints[i].CreatedPlane = true;
                        //circle.InnerPoints.RemoveAt(i);
                        for (int j = 1; i + j < circle.InnerPoints.Count; j++)
                        {
                            bool PlaneFound = false;
                            if (circle.InnerPoints[i + j].Verified == false && circle.InnerPoints[i + j].CreatedPlane == false)
                            {
                                //Find the second point
                                CreatPlanePoints[1] = circle.InnerPoints[i + j];
                                circle.InnerPoints[i + j].CreatedPlane = true;
                                for (int k = 1; i + j + k < circle.InnerPoints.Count; k++)
                                {
                                    if (circle.InnerPoints[i + j + k].Verified == false && circle.InnerPoints[i + j + k].CreatedPlane == false)
                                    {
                                        //Find the third point
                                        CreatPlanePoints[2] = circle.InnerPoints[i + j + k];
                                        //circle.InnerPoints[i + j + k].CreatedPlane = true;

                                        if (cPlane.CreatePlane(CreatPlanePoints[0], CreatPlanePoints[1], CreatPlanePoints[2]))
                                        {
                                            ///the plane generated
                                            ///detect points on the plane
                                            foreach (Point point in circle.InnerPoints)
                                            {
                                                if (point.Verified == false)
                                                {
                                                    //if the point on the plane
                                                    if (cPlane.IfOnthePlane(point) > 0)
                                                    {
                                                        cPlane.InnerPoints.Add(point);
                                                    }
                                                }
                                            }

                                            if (cPlane.InnerPoints.Count > 5)
                                            {
                                                foreach (Point point in cPlane.InnerPoints)
                                                {
                                                    point.Verified = true;
                                                }
                                                Circle Vcircle = new Circle();

                                                Vcircle.InnerPoints = cPlane.InnerPoints;
                                                Vcircle.SN = VerifiedCircle.Count;
                                                VerifiedCircle.Add(Vcircle);
                                                PlaneFound = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (PlaneFound == true)
                                {
                                    break;
                                }

                            }
                        }



                        //    int counter = 0;
                        //    foreach (Point point in PD.originalPoints)
                        //    {
                        //        if (point.Classified == false)
                        //        {
                        //            counter++;
                        //        }
                        //    }
                        //    if (counter > 3)
                        //    {
                        //        MessageBox.Show("More than 3 points have not been classified.");
                        //    }
                        //    else
                        //        break;

                        //}
                    }
                }
            }
            #endregion

            circleList = VerifiedCircle;

            foreach (Circle circle in circleList)
            {
                foreach (Point point in circle.InnerPoints)
                {
                    point.circleNumber = circle.SN;
                }
            }

            //find the connection orders of circles
            for (int i = 0; i < circleList.Count; i++)
            {
                foreach (Point connectedPoint in circleList[i].InnerPoints[0].ConnectedPoints)
                {
                    int circleID = connectedPoint.circleNumber;
                    //i is the same as current circle.InnerPoints[0].circleID
                    //The ID of each circle is supposed to be defined!!!!!!
                    //Redundant here!!!
                    //if (circleID != i) 
                    //{
                    //    //circleList[i].ConnectedCirclesID.Add(circleID);
                    //    //circleList[i].ConnectedCircles.Add(circleList[circleID]);
                    //}
                    foreach (Circle circle in circleList)
                    {
                        if (circle.SN == circleID && circleID != circleList[i].SN && !circleList[i].ConnectedCircles.Contains(circle))
                        {
                            circleList[i].ConnectedCircles.Add(circle);
                        }
                    }
                }
            }

            ///test connected circles
            //foreach (Circle circle in circleList)
            //{
            //    Console.Write(circle.SN + ": ");
            //    foreach (Circle connectedcircle in circle.ConnectedCircles)
            //    {
            //        Console.Write(connectedcircle.SN + ", ");
            //    }
            //    Console.WriteLine(".");
            //}

            #region ///delete the duplicated values in the list
            ////delete the duplicated values in the list
            //foreach (Circle circle in circleList)
            //{
            //    for (int i = 0; i < circle.ConnectedCircles.Count-1; i++)
            //    {
            //        for (int j = circle.ConnectedCircles.Count-1; j >i; j--)
            //        {
            //            if (circle.ConnectedCircles[j].SN == circle.ConnectedCircles[i].SN)
            //            {
            //                circle.ConnectedCircles.RemoveAt(j);
            //            }
            //        }
            //    }
            //}
            #endregion

            #region delete the one-way connection
            foreach (Circle circle in circleList)
            {
                for (int i = 0; i < circle.ConnectedCircles.Count; i++)
                {
                    if (!circle.ConnectedCircles[i].ConnectedCircles.Contains(circle))
                    {
                        circle.ConnectedCircles.RemoveAt(i);
                    }
                }
            }
            #endregion

            List<Circle> OrderedCircle = new List<Circle>();
            CentralLine centralLine = new CentralLine();
            #region ///GetCentralLine_Old
            ////get the centralLine, and it requires the connection order first
            //foreach (Circle circle in circleList)
            //{
            //    if (circle.ConnectedCircles.Count == 1)
            //    {
            //        OrderedCircle.Add(circle);
            //        circle.CentralPoint = circle.CPcalculation();
            //        centralLine.CentralPoints.Add(circle.CentralPoint);
            //        break;
            //    }
            //}

            //for (int i = 0; i < OrderedCircle.Count; i++)
            //{
            //    foreach (Circle circle in OrderedCircle[i].ConnectedCircles)
            //    {
            //        if (OrderedCircle.Contains(circle))
            //        {                        
            //        }
            //        else
            //        {
            //            OrderedCircle.Add(circle);
            //            circle.CentralPoint = circle.CPcalculation();
            //            centralLine.CentralPoints.Add(circle.CentralPoint);
            //        }
            //    } 
            //}
            #endregion

            ///Verify if there is any error groups
            //////get the end of the elbow and get the vertexs of the centralLine
            List<Circle> EndCircle = new List<Circle>();
            
            foreach (Circle circle in circleList)
            {
                if (circle.ConnectedCircles.Count == 1)
                {
                    EndCircle.Add(circle);
                }

                if (circle.ConnectedCircles.Count == 0 || circle.ConnectedCircles.Count > 2)
                {
                    errorGroup = true;
                    break;
                }

            }

            if (EndCircle.Count != 2)
            {
                //Console.WriteLine("The elbow has more than one end!");
                errorGroup = true;
            }

            if (errorGroup == true)
            {
                return "ERROR";
            }
            else  
            {

                OrderedCircle.Add(EndCircle[0]);
                EndCircle[0].CentralPoint = EndCircle[0].CPcalculation();
                centralLine.CentralPoints.Add(EndCircle[0].CentralPoint);
                for (int i = 0; i < OrderedCircle.Count; i++)
                {
                    foreach (Circle circle in OrderedCircle[i].ConnectedCircles)
                    {
                        if (OrderedCircle.Contains(circle))
                        {
                        }
                        else
                        {
                            OrderedCircle.Add(circle);
                            circle.CentralPoint = circle.CPcalculation();
                            centralLine.CentralPoints.Add(circle.CentralPoint);
                        }
                    }
                }

                //calculate the radius of the elbow
                double[] radius = new double[OrderedCircle.Count];
                for (int i = 0; i < OrderedCircle.Count; i++)
                {
                    radius[i] = elbowcal_1.Distance(
                        OrderedCircle[i].CentralPoint, OrderedCircle[i].InnerPoints[0]);
                }
                double R = radius.Average();


                //2016.11.15 Code has been checked until here.

                //calculate the length of the elbow
                double Length = centralLine.CentralLineLength();

                #region Calculate the angle of the elbow
                //double Angle = centralLine.CalculateAngle();
                //http://blog.sina.com.cn/s/blog_49cc616d010196nl.html
                //Using two end circles to calculate the angle of the elbow
                //calculate the angle of two planes
                double ElbowAngle;
                if (OrderedCircle[0].ConnectedCircles.Count == 1 &&
                    OrderedCircle[OrderedCircle.Count - 1].ConnectedCircles.Count == 1)
                {
                    Plane Plane1 = new Plane();
                    Plane Plane2 = new Plane();

                    for (int i = 1; i < 4; i++)
                    {
                        Plane1.InnerPoints.Add(OrderedCircle[0].InnerPoints[i]);
                        Plane2.InnerPoints.Add(OrderedCircle[OrderedCircle.Count - 1].InnerPoints[i]);
                    }

                    //Calculate the normal vector of 2 planes
                    //http://blog.sina.com.cn/s/blog_49cc616d010196nl.html
                    //Plane function: Ax+By+Cz+D=0
                    //Then vector: (A, B, C) is a normal vector of the plane
                    Vector3D NormalVector1 = new Vector3D();
                    NormalVector1 = Plane1.NormalVector(Plane1.InnerPoints);

                    Vector3D NormalVector2 = new Vector3D();
                    NormalVector2 = Plane2.NormalVector(Plane2.InnerPoints);

                    Vector3D DirectVector1 = new Vector3D();
                    DirectVector1.X = centralLine.CentralPoints[0].X - centralLine.CentralPoints[1].X;
                    DirectVector1.Y = centralLine.CentralPoints[0].Y - centralLine.CentralPoints[1].Y;
                    DirectVector1.Z = centralLine.CentralPoints[0].Z - centralLine.CentralPoints[1].Z;

                    Vector3D DirectVector2 = new Vector3D();
                    DirectVector2.X = centralLine.CentralPoints[centralLine.CentralPoints.Count - 1].X
                        - centralLine.CentralPoints[centralLine.CentralPoints.Count - 2].X;
                    DirectVector2.Y = centralLine.CentralPoints[centralLine.CentralPoints.Count - 1].Y
                        - centralLine.CentralPoints[centralLine.CentralPoints.Count - 2].Y;
                    DirectVector2.Z = centralLine.CentralPoints[centralLine.CentralPoints.Count - 1].Z
                        - centralLine.CentralPoints[centralLine.CentralPoints.Count - 2].Z;

                    if (Vector3D.DotProduct(NormalVector1, DirectVector1) < 0)
                    {
                        NormalVector1 = -1 * NormalVector1;
                    }
                    if (Vector3D.DotProduct(NormalVector2, DirectVector2) > 0)
                    {
                        NormalVector2 = -1 * NormalVector2;
                    }

                    ElbowAngle = Vector3D.AngleBetween(NormalVector1, NormalVector2);

                }

                else
                {
                    //there is an error
                    Console.WriteLine("There are more than two end!");
                    ElbowAngle = 0;
                }


                //Console.WriteLine("Radius: " + R.ToString() + ", Length: " + Length.ToString() + ", Angle: " + ElbowAngle.ToString());
                //MessageBox.Show("Radius: " + R.ToString() + ", Length: " + Length.ToString() + ", Angle: " + ElbowAngle.ToString());
                string result = "Radius: " + R.ToString() + ", Length: " + Length.ToString() + ", Angle: " + ElbowAngle.ToString();
                return result;
                #endregion
            }
        }
    }
}
