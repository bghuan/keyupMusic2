using System;
using System.Globalization;
using System.Text;

class Program
{
    static void Main()
    {
        int score1 = 0, score2 = 0, score3 = 0;
        int times = 10000;
        var random = new Random();
        int random1 = 1,random2 = 7;
        //random1--; random2--;
        for (int i = 0; i < times; i++)
        {
            int a = 0, b = 0;
            for (int j = 0; j < 12; j++)
            {
                a += random.Next(random1, random2);
            }
            for (int j = 0; j < 11; j++)
            {
                b += random.Next(random1, random2);
            }
            if (a > b) { score1++; }
            else if (a < b) { score2++; }
            else { score3++; }
        }
        Console.WriteLine(score1 + "    " + (score2 + score3));
    }
}