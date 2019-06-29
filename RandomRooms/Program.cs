using System;
using System.Collections.Generic;
using System.Threading;

namespace RandomRooms
{
    class Program
    {
        static void Main(string[] args)
        {
            // =========== Setup =========== 
            Console.CursorVisible = false;

            // =========== Classes =========== 
            RoomsMap roomMap = new RoomsMap();

            // =========== Variables =========== 
            
            // Stores map. 
            bool[,] map = roomMap.initializeMap();

            // Stores the centre point of all rooms created.
            List<int[]> centrePoints = new List<int[]>();

            // HACK: Sets the delay between printing each corridore. Development tool that can be removed later.
            int corridorPrintDelay = 0;

            // =========== Map Generation =========== 

            // Toggle DEBUG output from roomMap.
            roomMap.EnableDebugOutput = true;

            // Add rooms to map
            for (int i = 0; i < 10; i++)
            {
                centrePoints.Add(roomMap.AddRoom(i, map));
            }

            // Print map to console with rooms.
            roomMap.PrintMap(map);
             
            // Print the corridors between rooms.
            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCorridors(i, centrePoints);
                // HACK: This is just a devleopment aid. It can be removed later.
                Thread.Sleep(corridorPrintDelay);
            }

            // Mark the centre of each room with its room number.
            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCentre(centrePoints[i], i);
            }

            // Wait till user exits.
            Console.ReadLine();

        }
    }
}
