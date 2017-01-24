using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeCalculator
{
    class PrimitivePoints
    {
        //OriginalPoints 
        public List<Point> originalPoints = new List<Point>();

        //Find out the connection relationship between points
        public void createOriginalPoints(string[] tempLine)
        {
            //foreach (string line in tempLine)
            for (int num = 0; num < tempLine.Length-1; num++)
            {
                int i = originalPoints.Count;

                Point point1 = new Point();
                point1.SN = i;
                Point point2 = new Point();
                point2.SN = i + 1;
                Point point3 = new Point();
                point3.SN = i + 2;

                //string[] temp = line.Split(',');
                string[] temp = tempLine[num].Split(',');
                point1.X = Convert.ToDouble(temp[0]);
                point1.Y = Convert.ToDouble(temp[1]);
                point1.Z = Convert.ToDouble(temp[2]);

                point2.X = Convert.ToDouble(temp[3]);
                point2.Y = Convert.ToDouble(temp[4]);
                point2.Z = Convert.ToDouble(temp[5]);

                point3.X = Convert.ToDouble(temp[6]);
                point3.Y = Convert.ToDouble(temp[7]);
                point3.Z = Convert.ToDouble(temp[8]);

                ////judge the reduplicative data
                //add the original 3 points
                if (i == 0)
                {
                    originalPoints.Add(point1);
                    originalPoints.Add(point2);
                    originalPoints.Add(point3);
                }

                else
                {
                    int flag = 0;
                    for (int j = 0; j < originalPoints.Count; j++)
                    {
                        flag += point1.EqualTo(originalPoints[j]);
                        if (flag > 0)
                        {
                            point1 = originalPoints[j];
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        originalPoints.Add(point1);
                    }

                    flag = 0;
                    for (int j = 0; j < originalPoints.Count; j++)
                    {
                        flag += point2.EqualTo(originalPoints[j]);
                        if (flag > 0)
                        {
                            point2 = originalPoints[j];
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        originalPoints.Add(point2);
                    }

                    flag = 0;
                    for (int j = 0; j < originalPoints.Count; j++)
                    {
                        flag += point3.EqualTo(originalPoints[j]);
                        if (flag > 0)
                        {
                            point3 = originalPoints[j];
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        originalPoints.Add(point3);
                    }
                }

                //add the connection id to the point
                point1.ConnectedPoints.Add(point2);
                point1.ConnectedPoints.Add(point3);
                point2.ConnectedPoints.Add(point1);
                point2.ConnectedPoints.Add(point3);
                point3.ConnectedPoints.Add(point1);
                point3.ConnectedPoints.Add(point2);
                
            }
        }
    }
}
