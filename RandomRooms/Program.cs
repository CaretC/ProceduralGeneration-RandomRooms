using System;
using System.Collections.Generic;
using System.Threading;

namespace RandomRooms
{
    class Program
    {
        static void Main(string[] args)
        {
            // ************** Actual *************************************
            //Console.CursorVisible = false;

            //RoomsMap roomMap = new RoomsMap();

            //bool[,] map = roomMap.initializeMap();
            //List<int[]> centrePoints = new List<int[]>();


            //for (int i = 0; i < 10; i++)
            //{
            //    centrePoints.Add(roomMap.AddRoom(map));
            //}

            //roomMap.PrintMap(map);

            //foreach (var centre in centrePoints)
            //{
            //    roomMap.PrintCentre(centre);
            //}

            //roomMap.PrintCorridors(centrePoints);

            //Console.ReadLine();


            // ************** Test *************************************
            Console.CursorVisible = false;

            RoomsMap roomMap = new RoomsMap();

            bool[,] map = roomMap.initializeMap();
            List<int[]> centrePoints = new List<int[]>();


            for (int i = 0; i < 10; i++)
            {
                centrePoints.Add(roomMap.AddRoom(map));
            }

            roomMap.PrintMap(map);

            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCentre(centrePoints[i], i);
            }

            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCorridors(i, centrePoints);
                Thread.Sleep(200);
            }

            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.SelectClosestRoom(i, centrePoints);
            }


            Console.ReadLine();

        }
    }
}
