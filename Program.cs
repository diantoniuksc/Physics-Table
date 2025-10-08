using SolvePhysics;

class Program
{
    static void Main()
    {
        StatysticsTable table = new('g', new double[] { 9.425, 8.959, 9.615, 9.4, 9.497, 9.35 });
        table.PrintTableandCalculations();
        table.SaveAsTableImage(@"C:\DuT\stats", "My_Stats");
    }
}
