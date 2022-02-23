using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace SnakeGame
{
    class Program
    {
        static Program prog= new Program();

        int time;
        List<Node> nodes = new List<Node>();
        int[] x_boundaries = new int[2];
        int[] y_boundaries = new int[2];
        int interval;
        Point direction;

        List<ConsoleColor> colors = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Cyan };

        Random random = new Random();

        static void Main(string[] args)
        {
            prog.SelectColor();
            prog.Initialize();
            while (true)
            {
                prog.Play();
            }           
        }

        void SelectColor()
        {
            ConsoleColor selected_color= colors[random.Next(0, colors.Count)];

            if (selected_color == Console.ForegroundColor)
            {
                SelectColor();
            }
            else
            {
                Console.ForegroundColor = selected_color;
            }
        }

        void Initialize()
        {            
            time = 0;
            nodes = new List<Node>();
            x_boundaries[0] = 0;
            x_boundaries[1] = 111;
            y_boundaries[0] = 0;
            y_boundaries[1] = 27;
            interval = 12000000;

            int width = x_boundaries[1] - x_boundaries[0];
            int height = y_boundaries[1] - y_boundaries[0];

            Console.SetWindowSize(width+1,height+1);
            Console.SetBufferSize(width+1,height+1);

            int start_point_x = random.Next(1, 55);
            int start_point_y = random.Next(1, 13);
            direction = new Point(1 * Math.Sign(random.Next(-1, 2)), 1 * Math.Sign(random.Next(-1, 2)));

            if (direction.X == 0)
            {
                direction.X += 1;
            }

            if (direction.Y == 0)
            {
                direction.Y += 1;
            }

            nodes.Add(new Node(new Point(start_point_x, start_point_y)));
            nodes.Add(new Node(new Point(start_point_x, start_point_y+1)));
            nodes.Add(new Node(new Point(start_point_x, start_point_y+2)));
            nodes.Add(new Node(new Point(start_point_x+1, start_point_y)));
            nodes.Add(new Node(new Point(start_point_x+1, start_point_y+2)));
            nodes.Add(new Node(new Point(start_point_x+2, start_point_y)));
            nodes.Add(new Node(new Point(start_point_x+2, start_point_y+2)));
            nodes.Add(new Node(new Point(start_point_x+3, start_point_y)));
            nodes.Add(new Node(new Point(start_point_x+3, start_point_y+1)));
            nodes.Add(new Node(new Point(start_point_x+3, start_point_y+2)));
        }

        void Play()
        {
            if (time >= interval)
            {
                prog.Move();
                prog.DetectCollision();
                prog.Draw();

                time = 0;
            }

            else
            {
                time++;
            }
        }

        void Move()
        {
            foreach (Node node in nodes)
            {
                node.location.X += direction.X;
                node.location.Y += direction.Y;
            }
        }

        void DetectCollision()
        {
            foreach (Node node in nodes)
            {
                if (node.location.X < x_boundaries[0] || node.location.X > x_boundaries[1])
                {
                    CollideVertical();
                    SelectColor();
                    break;
                }
            }

            foreach (Node node in nodes)
            {
                if (node.location.Y < y_boundaries[0] || node.location.Y > y_boundaries[1])
                {
                    CollideHorizontal();
                    SelectColor();
                    break;
                }
            }            
        }

        void CollideVertical()
        {
            direction = new Point(direction.X*-1, direction.Y);
        }

        void CollideHorizontal()
        {
            direction = new Point(direction.X, direction.Y * -1);
        }

        void Draw()
        {
            Console.Clear();

            for (int i = y_boundaries[0]; i <= y_boundaries[1]; i++)
            {
                List<int> x_indexes = new List<int>();

                foreach (Node node in nodes)
                {
                    if (node.location.Y == i)
                    {
                        x_indexes.Add(node.location.X);
                    }
                }

                if (x_indexes.Count == 0)
                {
                    Console.WriteLine(" ");
                }

                else
                {
                    string line = "";

                    for (int j = x_boundaries[0]; j <= x_boundaries[1]; j++)
                    {
                        if (!x_indexes.Contains(j))
                        {
                            line += " ";
                        }

                        else
                        {
                            line += "o";
                        }
                    }

                    Console.WriteLine(line);
                }
            }
        }
    }

    class Node
    {
        public Point location = new Point();

        public Node(Point location)
        {
            this.location = location;
        }
    }
}
