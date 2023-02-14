namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool[,] map;

            Start();

            void Start()
            {
                List<string> instr = GetInstructions();

                map = GetMap();
                map = FoldSequence(map, instr);
                Console.WriteLine(" ");
                PrintMap(map);
                Console.WriteLine(CountDots(map));

            }
            bool[,] GetMap()
            {
                bool[,] map;
                int xMax = 0;
                int yMax = 0;
                List<(int, int)> instr = new List<(int, int)>();
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day13\dots.txt";
                List<string> input = File.ReadAllLines(path).ToList();

                //Fill instruction list with tuples (int,int) from input. Set xMax and yMax values from highest.
                for(int i=0;i<input.Count;i++)
                {
                    string[] str = input[i].Split(",");
                    int x = Convert.ToInt32(str[0]);
                    int y = Convert.ToInt32(str[1]);

                    instr.Add((x,y));

                    if(x > xMax){
                        xMax = x;
                    }
                    if (y > yMax)
                    {
                        yMax = y;
                    }
                }
                
                map = new bool[xMax+1,yMax+2];

                for(int i = 0; i < instr.Count; i++)
                {
                    map[instr[i].Item1, instr[i].Item2] = true;
                }
                Console.WriteLine("xMax = " + xMax + " yMax = " + yMax);
                return map;
            }
            
            bool[,] FoldSequence(bool[,] map, List<string> instr)
            {
                bool[,] foldMap = map; 
                for(int i = 0; i < instr.Count; i++)
                {
                    if (instr[i].Contains("x"))
                    {
                        //Fold map left on X
                        int x = Convert.ToInt32(instr[i].Substring(instr[i].LastIndexOf('=') + 1));
                        foldMap = XFold(foldMap, x);
                    }
                    else
                    {
                        //Fold map up on Y
                        int y = Convert.ToInt32(instr[i].Substring(instr[i].LastIndexOf('=') + 1));
                        foldMap = YFold(foldMap, y);
                    }
                }
                return foldMap;
            }
            //take map as argument and plot a folded map after it
            bool[,] XFold(bool[,] map, int index)
            {
                Console.WriteLine(map.GetLength(0) + " index:" + index + " maplength2: " + map.GetLength(1));
                bool[,] foldMap = new bool[index, map.GetLength(1)];

                for (int x = 0; x < index; x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] == true)
                        {
                            foldMap[x, y] = true;
                        }
                        if (map[map.GetLength(0) - x-1 , y] == true)
                        {
                            foldMap[x, y] = true;
                        }
                    }
                }
                Console.WriteLine(CountDots(foldMap));
                return foldMap;
            }
            //take map as argument and plot a folded map after it
            bool[,] YFold(bool[,] map, int index)
            {
                Console.WriteLine(map.GetLength(0) + " index:" + index + " maplength2: " + map.GetLength(1));
                bool[,] foldMap = new bool[map.GetLength(0) , index];

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < index; y++)
                    {
                        if (map[x, y] == true)
                        {
                            foldMap[x, y] = true;
                        }
                        if (map[x, map.GetLength(1) - y -1] == true)
                        {
                            foldMap[x, y] = true;
                        }
                    }
                }
                Console.WriteLine(CountDots(foldMap));
                return foldMap;
            }
            //loop through map and count amounts of dots (#)
            int CountDots(bool[,] map)
            {
                int count = 0;
                for (int x = 0; x < map.GetLength(0);x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x,y] == true)
                            count++;
                    }
                }
                return count;
            }

            //Returns a list of folding instructions
            List<string> GetInstructions()
            {
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day13\instructions.txt";
                List<string> input = File.ReadAllLines(path).ToList();
                for (int i = 0; i < input.Count; i++)
                {
                    input[i] = input[i].Replace("fold along ", "");
                }
                return input;
            }

            //Take map as argument and print corresponding value in console
            void PrintMap(bool[,] map)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        if (map[y, x] != false)
                        {
                            Console.Write("#");
                        }
                        else { Console.Write("."); }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}


/*
--- Day 13: Transparent Origami ---
You reach another volcanically active part of the cave. 
It would be nice if you could do some kind of thermal imaging so you could tell ahead of time which caves are too hot to safely enter.

Fortunately, the submarine seems to be equipped with a thermal camera! When you activate it, you are greeted with:

Congratulations on your purchase! To activate this infrared thermal imaging
camera system, please enter the code found on page 1 of the manual.
Apparently, the Elves have never used this feature. 
To your surprise, you manage to find the manual; as you go to open it, page 1 falls out. 
It's a large sheet of transparent paper! 
The transparent paper is marked with random dots and includes instructions on how to fold it up (your puzzle input). 
For example:

6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5
The first section is a list of dots on the transparent paper. 
0,0 represents the top-left coordinate. The first value, x, increases to the right. 
The second value, y, increases downward. So, the coordinate 3,0 is to the right of 0,0, and the coordinate 0,7 is below 0,0. 
The coordinates in this example form the following pattern, where # is a dot on the paper and . is an empty, unmarked position:

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
Then, there is a list of fold instructions. 
Each instruction indicates a line on the transparent paper and wants you to fold the paper up (for horizontal y=... lines) 
or left (for vertical x=... lines). 
In this example, the first fold instruction is fold along y=7, 
which designates the line formed by all of the positions where y is 7 (marked here with -):

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
-----------
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
Because this is a horizontal line, 
fold the bottom half up. Some of the dots might end up overlapping after the fold is complete, 
but dots will never appear exactly on a fold line. The result of doing this fold looks like this:

#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........
Now, only 17 dots are visible.

Notice, for example, the two dots in the bottom left corner before the transparent paper is folded; 
after the fold is complete, those dots appear in the top left corner (at 0,0 and 0,1). 
Because the paper is transparent, the dot just below them in the result (at 0,3) remains visible, 
as it can be seen through the transparent paper.

Also notice that some dots can end up overlapping; in this case, the dots merge together and become a single dot.

The second fold instruction is fold along x=5, which indicates this line:

#.##.|#..#.
#...#|.....
.....|#...#
#...#|.....
.#.#.|#.###
.....|.....
.....|.....
Because this is a vertical line, fold left:

#####
#...#
#...#
#...#
#####
.....
.....
The instructions made a square!

The transparent paper is pretty big, so for now, focus on just completing the first fold. 
After the first fold in the example above, 
17 dots are visible - dots that end up overlapping after the fold is completed count as a single dot.

How many dots are visible after completing just the first fold instruction on your transparent paper?
*/