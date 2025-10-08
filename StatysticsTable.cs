using SolvePhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvePhysics
{
    internal class StatysticsTable
    {
        private char _quantity;
        private double[] _examplesArr;

        //<quantity> - <p> <g>
        public double AvgQ { get; set; }

        //delta(quantity(i)) - delta p1, delta g4
        public double[] DeltaQi { get; set; }

        //S<quantity> - S<q>, S<p>
        public double SQ { get; set; }

        //t(alpha, n) - from Sudent's Table - t(0.95, 4)
        public double TAlphaN { get; set; }

        //delta(quantity) - Total for all measurements
        public double DeltaQT { get; set; }

        //E - expected error in %
        public double E { get; set; }

        // n
        public int N { get; set; }

        public const double Alpha = 0.95;

        public StatysticsTable(char quantity, double[] examples)
        {
            _quantity = quantity;
            _examplesArr = examples;
            N = examples.Length;
        }


        public void PrintTableandCalculations()
        {
            CalculateAvgQ();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateDeltaQi();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateSQ();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateTAlphaN();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateDeltaQT();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateE();
        }


        public void CalculateAvgQ()
        {
            Console.WriteLine($"<{_quantity}> = Sum({_quantity}(i)) / n");

            double sum = 0; 
            foreach (double i in _examplesArr) 
            {
                sum += i;
            }
            Console.WriteLine(sum.ToString());

            Console.WriteLine($"<{_quantity}> = {sum} / {N}");

            AvgQ = (double)sum / N;
            Console.WriteLine($"ANSWER: <{_quantity} = {AvgQ}>");
        }

        public void CalculateDeltaQi()
        {
            Console.WriteLine($"Delta{_quantity}i = {_quantity}i - <{_quantity}>");

            Console.WriteLine("ANSWERS:");
            int count = 1;
            foreach (double value in _examplesArr)
            {
                DeltaQi[count] = value - AvgQ;
                Console.WriteLine($"Delta{_quantity}{count} = {value} - {AvgQ} = {DeltaQi[count]}");
                count++;
            }
        }

        public void CalculateSQ()
        {
            Console.WriteLine($"S<{_quantity}> = sqrt(sum(delta({_quantity}i^2)) / n)");

            double sumDeltaQiSq = 0;

            foreach(var value in DeltaQi)
            {
                sumDeltaQiSq += value * value;
            }

            Console.WriteLine($"sum(delta({_quantity}i^2)) = {sumDeltaQiSq}");
            Console.WriteLine($"n = {N}");

            SQ = Math.Sqrt((double)sumDeltaQiSq / N);
            Console.WriteLine($"ANSWER: S<{_quantity}> = {SQ}");
        }

        public void CalculateTAlphaN()
        {
            Console.WriteLine("YOU MUST FIND t alpha, n in Studen't table");
            Console.WriteLine("Input t(alpha, n)");
            TAlphaN = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine($"ANSWER: {TAlphaN}");
        }

        public void CalculateDeltaQT()
        {
            Console.WriteLine($"delta<{_quantity}> = S<{_quantity}> * t(alpha, n)");
            DeltaQT = SQ * TAlphaN;
            Console.WriteLine($"delta<{_quantity}> = {SQ} * {TAlphaN} = {DeltaQT}");
            Console.WriteLine($"ANSWER: {DeltaQT}");
        }

        public void CalculateE()
        {
            Console.WriteLine($"E = delta<{_quantity}> / <{_quantity}>");
            E = DeltaQT / AvgQ;
            Console.WriteLine($"E = {DeltaQT} / {AvgQ} = {E}");
            Console.WriteLine($"ANSWER: E = {E}");
        }
    }
}


