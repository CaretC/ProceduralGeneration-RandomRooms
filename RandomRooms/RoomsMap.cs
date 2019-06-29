using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace RandomRooms
{
    class RoomsMap
    {
        // =========== Properites =========== 

        /* 
         * Sets the delay between printing each cell of a corridore. This is mainly a development tool.
         * It will be removed at a later date.
         */
        public int CorridorePrintDelay { get; set; } = 50;

        // Sets the character to print for corridore cells.
        public char CorridoreChar { get; set; } = '.';

        // Sets the wall cell color.
        public ConsoleColor WallColor { get; set; } = ConsoleColor.Green;

        // Sets the character to print for wall cells.
        public char WallChar { get; set; } = '█';

        // Sets the default console forground color.
        public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;

        // Sets the character to represent room center point.
        public char CenterPointChar { get; set; } = 'X';

        // Sets the center point color.
        public ConsoleColor CenterPointColor { get; set; } = ConsoleColor.Red;

        // Sets max room dimention.
        public int MaxRoomDimention { get; set; } = 13;

        // Enables/ disables DEBUG output.
        public bool EnableDebugOutput { get; set; } = false;


        // =========== Classes =========== 
        Random random = new Random();


        // =========== Public Methods =========== 

        // Initializes a new map as a bool[,] and fills the map with 'Walls'. True values.
        public bool[,] initializeMap()
        {
            bool[,] map = new bool[Console.WindowWidth, Console.WindowHeight];

            for (int x = 0; x < Console.WindowWidth; x++)
            {
                for (int y = 0; y < Console.WindowHeight; y++)
                {
                    map[x, y] = true;
                }
            }

            // Prints DEBUG message
            if (EnableDebugOutput)
            {
                Debug.WriteLine("Map Initialized.");
            }

            return map;
        }
        
        // Add as room of a random odd height and width to the map, as a rectangle of false values.
        // Returns the center point of the room as an int[] ([xPos, yPos]).
        // REFACTOR: Check to see if this can be simplifed at all. 
        public int[] AddRoom(int roomNumber, bool[,] map)
        {
            // Prints DEBUG message
            if (EnableDebugOutput)
            {
                Debug.WriteLine(String.Format("Adding Room {0} ...", roomNumber));
            }
                       
            int width = OddNumber(MaxRoomDimention);
            int height = OddNumber(MaxRoomDimention);
            int[] startPoint = { random.Next(1, (Console.WindowWidth - (1 + width))), random.Next(1, (Console.WindowHeight - (1 + height)))};
            int[] centerPoint = { startPoint[0] + (width / 2), startPoint[1] + (height / 2) };

            // HACK: Currently only prints out if the room overlaps another in the DEBUG console. 
            // TODO: Ensure if room overlaps another it is not created.
            noOverlap(roomNumber, map, startPoint, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[(startPoint[0] + x), (startPoint[1] + y)] = false;                    
                }
            }

            return centerPoint;
        }

        // Prints the map to the console. All ture values in map bool[,] will be represented with the WallChar.
        // Map walls will be printed in WallColor.
        // After map is printed the forground color is returned to DefaultColor.
        public void PrintMap(bool[,] map)
        {
            Console.ForegroundColor = WallColor;

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x,y])
                    {                       
                        Console.SetCursorPosition(x, y);
                        Console.Write(WallChar);
                    }
                }
            }

            Console.ForegroundColor = DefaultColor;
        }

        // Used to print the centre of each room. This is a development tool and will be removed in future.
        // int[] centerPoint should be in the format ([xPos, yPos]).
        public void PrintCentre(int[] centerPoint)
        {
            Console.ForegroundColor = CenterPointColor;
            Console.SetCursorPosition(centerPoint[0], centerPoint[1]);
            Console.Write(CenterPointChar);
            Console.ForegroundColor = DefaultColor;
        }

        // An overload of the PrintCentre() that can print the room number at the center point of a room.
        // This is also a development feature and will be removed at a later date.
        // int[] centerPoint should be in the format ([xPos, yPos]).
        public void PrintCentre(int[] centerPoint, int roomNumber)
        {
            Console.ForegroundColor = CenterPointColor;
            Console.SetCursorPosition(centerPoint[0], centerPoint[1]);
            Console.Write(roomNumber);
            Console.ForegroundColor = DefaultColor;
        }

        // Takes in the index of a startRoom and the list of all room center points on the map and prints a corridore to the closest room.
        // BUG: Fix the issue that the closest distance between two rooms will often create a loop as the the closest room for one is also the second room.         
        public void PrintCorridors(int startRoom, List<int[]> roomCentrePoints)
        {
            int closest = SelectClosestRoom(startRoom, roomCentrePoints);

            int[] closestCentre = roomCentrePoints[closest];

            // Prints DEBUG message
            if (EnableDebugOutput)
            {
                Debug.WriteLine("Printing corridore between Room {0} and Room {1}.", startRoom, closest);
            }

            PrintVertical(roomCentrePoints[startRoom], roomCentrePoints[closest]);
            PrintHorizontal(roomCentrePoints[startRoom], roomCentrePoints[closest]);
        }

        // Takes in a roomIndex and the map room list and returns the index of the closest room. 
        public int SelectClosestRoom(int room, List<int[]> rooms)
        {
            int indexOfClosest = 0;
            // HACK: The value of 1000 has been arbritarily set as a BIG distance. 
            double closestDistance = 1000;

            for (int i = 0; i < rooms.Count; i++)
            {
                    double distance = Math.Sqrt(Math.Pow((rooms[room][0] - rooms[i][0]), 2) + Math.Pow((rooms[room][1] - rooms[i][1]), 2));

                    if (distance != 0)
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            indexOfClosest = i;
                        }

                    // Prints DEBUG message
                    if (EnableDebugOutput)
                    {
                        Debug.WriteLine(String.Format("Distance from Room {0} to Room {1}: {2}", room, i, distance));
                    }
                        
                    }
                             
            }
            return indexOfClosest;
        }

        // =========== Private Methods =========== 

        // Checks to see if the room about to be created overlaps with another room.
        // HACK: This was done in a hurry, this could definitly be made cleaner.
        private bool noOverlap(int roomNumber, bool[,] map, int[] startPoint, int width, int height)
        {
            bool noOverlap = false;
            int roomCellCount = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int xPos = startPoint[0] + x;
                    int yPos = startPoint[1] + y;

                    if (!map[xPos, yPos])
                    {
                        roomCellCount++;
                    }
                }
            }

            // Prints DEBUG message
            if (EnableDebugOutput)
            {
                if (roomCellCount == 0)
                {
                    noOverlap = true;
                    Debug.WriteLine(String.Format("Room {0} has no overlaps.", roomNumber));
                }

                else
                {
                    Debug.WriteLine(String.Format("Room {0} has overlaps.", roomNumber));
                }
            }

            return noOverlap;
        }

        // Prints the vertical component of a corridore using the CorridoreChar for each cooridore cell.
        private void PrintVertical(int[] room1, int[] room2)
        {
            int vSteps = Math.Abs(room1[1] - room2[1]) + 1;

            // Prints Down
            if (room1[1] < room2[1])
            {
                for (int i = 0; i < vSteps; i++)
                {
                    Console.SetCursorPosition(room1[0], (room1[1] + i));
                    Console.Write(CorridoreChar);
                    Thread.Sleep(CorridorePrintDelay);
                }
            }

            // Prints Up
            else
            {
                for (int i = 0; i < vSteps; i++)
                {
                    Console.SetCursorPosition(room1[0], (room1[1] - i));
                    Console.Write(CorridoreChar);
                    Thread.Sleep(CorridorePrintDelay);
                }
            }
        }

        // Prints the horizontal component of a corridore using the CorridoreChar for each cooridore cell.
        private void PrintHorizontal(int[] room1, int[] room2)
        {
            int hSteps = Math.Abs(room1[0] - room2[0]) + 1;

            // Prints Right
            if (room1[0] > room2[0])
            {
                for (int i = 0; i < hSteps; i++)
                {
                    Console.SetCursorPosition((room2[0] + i), room2[1]);
                    Console.Write(CorridoreChar);
                    Thread.Sleep(CorridorePrintDelay);
                }
            }

            // Prints Left
            else
            {
                for (int i = 0; i < hSteps; i++)
                {
                    Console.SetCursorPosition((room2[0] - i), room2[1]);
                    Console.Write(CorridoreChar);
                    Thread.Sleep(CorridorePrintDelay);
                }
            }
        }

        // Returns an odd number between 3 and max. 
        private int OddNumber(int max)
        {
            int number = random.Next(3, max);

            while (number % 2 == 0)
            {
                number = random.Next(3, max);
            }

            return number;
        }
    }


}
