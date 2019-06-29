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
            for (int i = 0; i < 30; i++)
            {
                // Attempt to create room. On room spawn fail [0, 0] is returned.
                int[] roomCenter = roomMap.AddRoom(i, map);

                // If room center is not [0, 0] add the room to the center points list.
                if (roomCenter[0] != 0 && roomCenter[1] != 0)
                {
                    centrePoints.Add(roomCenter);
                }
            }

            // Print map to console with rooms.
            roomMap.PrintMap(map);
             
            // Print the corridors between rooms.
            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCorridor(i, centrePoints);
                // HACK: This is just a devleopment aid. It can be removed later.
                Thread.Sleep(corridorPrintDelay);
            }

            // Mark the centre of each room with its room number.
            for (int i = 0; i < centrePoints.Count; i++)
            {
                roomMap.PrintCentre(centrePoints[i]);
            }

            // Wait till user exits.
            Console.ReadLine();

        }
    }
}
