using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace RandomRooms
{
    class RoomsMap
    {
        public int CorridorePrintDelay { get; set; } = 100;

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

            return map;
        }

        Random random = new Random();

        public int[] AddRoom(bool[,] map)
        {
            int width = OddNumber();
            int height = OddNumber();
            int[] startPoint = { random.Next(1, (Console.WindowWidth - (1 + width))), random.Next(1, (Console.WindowHeight - (1 + height)))};
            int[] centerPoint = { startPoint[0] + (width / 2), startPoint[1] + (height / 2) };
            

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[(startPoint[0] + x), (startPoint[1] + y)] = false;                    
                }
            }

            return centerPoint;
        }

        public void PrintMap(bool[,] map)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x,y])
                    {
                        
                        Console.SetCursorPosition(x, y);
                        Console.Write("█");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void PrintCentre(int[] centerPoint)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(centerPoint[0], centerPoint[1]);
            Console.Write("X");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void PrintCentre(int[] centerPoint, int roomNumber)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(centerPoint[0], centerPoint[1]);
            Console.Write(roomNumber);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void PrintCorridors(int startRoom, List<int[]> roomCentrePoints)
        {
            int closest = SelectClosestRoom(startRoom, roomCentrePoints);

            int[] closestCentre = roomCentrePoints[closest];

            PrintVertical(roomCentrePoints[startRoom], roomCentrePoints[closest]);
            PrintHorizontal(roomCentrePoints[startRoom], roomCentrePoints[closest]);
        }

        public int SelectClosestRoom(int room, List<int[]> rooms)
        {
            int indexOfClosest = 0;
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

                        Debug.WriteLine(String.Format("Distance from Room {0} to Room {1}: {2}", room, i, distance));
                    }
                             
            }

            Debug.WriteLine(String.Format("Closest room to Room {0} is Room {1}", room, indexOfClosest));
            return indexOfClosest;
        }

        private void PrintVertical(int[] room1, int[] room2)
        {
            int vSteps = Math.Abs(room1[1] - room2[1]) + 1;

            if (room1[1] < room2[1])
            {
                for (int i = 0; i < vSteps; i++)
                {
                    Console.SetCursorPosition(room1[0], (room1[1] + i));
                    Console.Write("*");
                    Thread.Sleep(CorridorePrintDelay);
                }
            }

            else
            {
                for (int i = 0; i < vSteps; i++)
                {
                    Console.SetCursorPosition(room1[0], (room1[1] - i));
                    Console.Write("*");
                    Thread.Sleep(CorridorePrintDelay);
                }
            }
        }

        private void PrintHorizontal(int[] room1, int[] room2)
        {
            int hSteps = Math.Abs(room1[0] - room2[0]) + 1;

            if (room1[0] > room2[0])
            {
                for (int i = 0; i < hSteps; i++)
                {
                    Console.SetCursorPosition((room2[0] + i), room2[1]);
                    Console.Write("*");
                    Thread.Sleep(CorridorePrintDelay);
                }
            }

            else
            {
                for (int i = 0; i < hSteps; i++)
                {
                    Console.SetCursorPosition((room2[0] - i), room2[1]);
                    Console.Write("*");
                    Thread.Sleep(CorridorePrintDelay);
                }
            }
        }



        private int OddNumber()
        {
            int number = random.Next(3, 13);

            while (number % 2 == 0)
            {
                number = random.Next(3, 13);
            }

            return number;
        }
    }


}
