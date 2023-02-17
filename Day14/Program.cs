using System.Security.Cryptography;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Day14();
            Part2(); //Part 2, work with dictionaries

            void Day14() //Part 1, work with string.
            {
                //Part 1
                Dictionary<string, string> templ = GetTemplate();
                string seq = GetSequence();

                int iterations = 10;
                for (int i = 0; i < iterations; i++)
                {
                    //Iterating the sequence string.
                    seq = Insertion(templ, seq);
                }
                Console.WriteLine(GetOutput(seq));
            }

            void Part2()
            {
                Insertion2(40);
            }

            long GetOutput(string seq)
            {
                long output = 0;
                string letters = "";

                //save each occuring letter once in letters
                for (int i = 0; i < seq.Length; i++)
                {
                    if (letters.Contains(seq[i]))
                    {
                        continue;
                    }
                    else
                    {
                        letters +=seq[i];
                    }
                }

                long[] occurance = new long[letters.Length];

                //loop through each letter and count how many times they occure in sequence
                for (int i=0;i< letters.Length; i++)
                {
                    occurance[i] = seq.Count(t => t == letters[i]);
                }
                output = occurance.Max()-occurance.Min();
                return output;
            }

            //Insert each corresponding letter to the sequence
            string Insertion(Dictionary<string, string> templ, string seq)
            {
                string result = "";
                for (int i = 0; i < seq.Length - 1; i++)
                {
                    string one = Convert.ToString(seq[i]);
                    string two = Convert.ToString(seq[i + 1]);
                    result += one + templ[one+two];
                }
                //For loop does not include the very last character, so added outside:
                result += seq.Last();

                return result;
            }

            void Insertion2(int x)
            {
                //Template: ((c1, c2), c3)
                //Polymer: ((c1,c1), count)
                Dictionary<(char, char), char> template = GetTemplate2();
                Dictionary<(char, char), long> polymer = GetSequence2();

                for (int i = 0; i < x; i++)
                {
                    Dictionary<(char, char), long> newSeq = new Dictionary<(char, char), long>();
                    
                    foreach (var ((c1, c2), count) in polymer)
                    {
                        char newChar = template[(c1, c2)];

                        if (!newSeq.ContainsKey((c1, newChar)))
                        {
                            newSeq.Add((c1, newChar), count);
                        }
                        else
                        {
                            newSeq[(c1, newChar)]+=count;
                        }
                        if (!newSeq.ContainsKey((newChar, c2)))
                        {
                            newSeq.Add((newChar, c2), count);
                        }
                        else
                        {
                            newSeq[(newChar, c2)]+=count;
                        }
                    }
                    polymer.Clear();
                    foreach (var ((c1, c2), count) in newSeq)
                    {
                        polymer[(c1, c2)] = count;
                    }
                }

                Dictionary<char, long> result = new Dictionary<char, long>();
                foreach(var ((c1, c2), count) in polymer)
                {
                    if (result.ContainsKey(c1))
                    {
                        result[c1] += count;
                    }
                    else
                    {
                        result[c1] = count;
                    }
                }

                //Last char is not accounted for
                char lostChar = GetSequence()[GetSequence().Length -1];
                result[lostChar] += 1;
                Console.WriteLine(result.Values.Max() - result.Values.Min());
            }

            //Get template input
            Dictionary<string,string> GetTemplate()
            {
                Dictionary<string,string> templ = new Dictionary<string,string>();
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day14\template.txt";
                List<string> input = File.ReadAllLines(path).ToList();

                for(int i = 0; i < input.Count(); i++)
                {
                    string[] items = input[i].Split(" -> ");
                    templ[items[0]] = items[1];
                }
                return templ;
            }

            Dictionary<(char,char),char> GetTemplate2()
            {
                Dictionary<(char, char), char> templ = new Dictionary<(char, char), char>();
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day14\template.txt";
                List<string> input = File.ReadAllLines(path).ToList();

                for (int i = 0; i < input.Count(); i++)
                {
                    string[] items = input[i].Split(" -> ");
                    templ.Add((items[0][0],items[0][1]), items[1][0]);
                }
                return templ;
            }
            //Get sequence input
            string GetSequence()
            {
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day14\seq.txt";
                string input = File.ReadAllText(path);
                
                return input;
            }
            Dictionary<(char, char), long> GetSequence2()
            {
                Dictionary<(char, char), long> seq = new Dictionary<(char, char), long>();
                string path = @"C:\Users\Tardis\Documents\School\Advent of Code\2021\Day14\seq.txt";
                string input = File.ReadAllText(path);

                for (int i = 0; i < input.Length-1; i++)
                {
                    char c1 = input[i];
                    char c2 = input[i+1];
                    if (seq.ContainsKey((c1, c2)))
                    {
                        seq[(c1, c2)] += 1;
                    }
                    else
                    {
                        seq.Add((c1, c2), 1);
                    }
                }
                
                return seq;
            }
        }
    }
}
/*
--- Day 14: Extended Polymerization ---
The incredible pressures at this depth are starting to put a strain on your submarine. 
The submarine has polymerization equipment that would produce suitable materials to reinforce the submarine, 
and the nearby volcanically-active caves should even have the necessary input elements in sufficient quantities.

The submarine manual contains instructions for finding the optimal polymer formula; 
specifically, it offers a polymer template and a list of pair insertion rules (your puzzle input). 
You just need to work out what polymer would result after repeating the pair insertion process a few times.

For example:

NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C
The first line is the polymer template - this is the starting point of the process.

The following section defines the pair insertion rules. 
A rule like AB -> C means that when elements A and B are immediately adjacent, element C should be inserted between them. 
These insertions all happen simultaneously.

So, starting with the polymer template NNCB, the first step simultaneously considers all three pairs:

The first pair (NN) matches the rule NN -> C, so element C is inserted between the first N and the second N.
The second pair (NC) matches the rule NC -> B, so element B is inserted between the N and the C.
The third pair (CB) matches the rule CB -> H, so element H is inserted between the C and the B.
Note that these pairs overlap: the second element of one pair is the first element of the next pair. 
Also, because all pairs are considered simultaneously, 
inserted elements are not considered to be part of a pair until the next step.

After the first step of this process, the polymer becomes NCNBCHB.

Here are the results of a few steps using the above rules:

Template:     NNCB
After step 1: NCNBCHB
After step 2: NBCCNBBBCBHCB
After step 3: NBBBCNCCNBBNBNBBCHBHHBCHB
After step 4: NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB
This polymer grows quickly. After step 5, it has length 97; 
After step 10, it has length 3073. After step 10, B occurs 1749 times, C occurs 298 times, H occurs 161 times, 
and N occurs 865 times; 
taking the quantity of the most common element (B, 1749) and subtracting the quantity of the least common element (H, 161) 
produces 1749 - 161 = 1588.

Apply 10 steps of pair insertion to the polymer template and find the most and least common elements in the result. 
What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

*/
/*
                for (int i = 0; i < seq.Length-1; i++)
                {
                    foreach(string key in templ.Keys)
                    {
                        string one = Convert.ToString(seq[i]);
                        string two = Convert.ToString(seq[i + 1]);
                        string check = one+two;
                        if (key == check)
                        {
                            result += one+templ[key];
                        }
                    }
                }
*/
/*
 * 
NNCB

NN, NC, CB
 
NN -> NC, CN
NC -> NB,BC
CB -> CH, HB

NCNBCHB

NC, CN, NB, BC, CH, HB
NC -> NB; BC
CN -> CC, CN
NB -> NB, BB...
2x NB -> 2x 2B, 2x BB
*/