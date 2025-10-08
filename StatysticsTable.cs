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


        public StatysticsTable(char quantity, double[] examples)
        {
            _quantity = quantity;
            _examplesArr = examples;
        }


        public void PrintTableandCalculations()
        {
            CalculateAvgQ();
            Console.WriteLine("----- -----");
            Console.WriteLine();

            CalculateDeltaQi();
            Console.WriteLine("----- -----");
            Console.WriteLine();
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

            int n = _examplesArr.Length;
            Console.WriteLine($"<{_quantity}> = {sum} / {n}");

            AvgQ = (double)sum / n;
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
    }
}


