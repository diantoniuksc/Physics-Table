using SolvePhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Versioning;
using System.IO;
using System.Runtime.InteropServices;

namespace SolvePhysics
{
    internal class StatysticsTable
    {
        private char _quantity;
        private double[] _examplesArr;


        public StatysticsTable(char quantity, double[] examples)
        {
            _quantity = quantity;
            _examplesArr = examples;
            N = examples.Length;
        }


        // n
        public static int N { get; set; }

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

        public const double Alpha = 0.95;

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
            Console.WriteLine("----- -----");
            Console.WriteLine();

            Console.WriteLine($"FINAL ANSWER: {_quantity} = (<{_quantity}> +- delta<{_quantity}>) if aplha = ...");
            Console.WriteLine($"FINAL ANSWER: {_quantity} = ({AvgQ} +- {DeltaQT}) if aplha = ...");
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
            Console.WriteLine($"ANSWER: <{_quantity}> = {AvgQ}");
        }

        public void CalculateDeltaQi()
        {
            Console.WriteLine($"Delta{_quantity}i = {_quantity}i - <{_quantity}>");

            Console.WriteLine("ANSWERS:");
            int count = 0;
            DeltaQi = new double[N];

            foreach (double value in _examplesArr)
            {
                DeltaQi[count] = value - AvgQ;
                Console.WriteLine($"Delta {_quantity}{count + 1} = {value} - {AvgQ} = {DeltaQi[count]}");
                count++;
            }
        }

        public void CalculateSQ()
        {
            Console.WriteLine($"S<{_quantity}> = sqrt(sum(delta({_quantity}i^2)) / (n * (n - 1)))");

            double sumDeltaQiSq = 0;

            foreach(var value in DeltaQi)
            {
                sumDeltaQiSq += value * value;
            }

            Console.WriteLine($"sum(delta({_quantity}i^2)) = {sumDeltaQiSq}");
            Console.WriteLine($"n = {N}");

            SQ = Math.Sqrt((double)sumDeltaQiSq / N / (N - 1));
            Console.WriteLine($"ANSWER: S<{_quantity}> = {SQ}");
        }

        public void CalculateTAlphaN()
        {
            Console.WriteLine("YOU MUST FIND t alpha, n in Student's table");
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

        /// <summary>
        /// Renders an image containing a table of all properties of this class
        /// </summary>
        [SupportedOSPlatform("windows")]
        public void SaveAsTableImage(string filePath, string? title = null, bool overwrite = true)
        {
            //resolve path
            string resolvedPath = filePath;
            try
            {
                resolvedPath = Path.GetFullPath(filePath);
            }
            catch
            {
               
            }


            if (string.IsNullOrWhiteSpace(Path.GetExtension(resolvedPath)))
            {
                resolvedPath = resolvedPath.TrimEnd('.') + ".png";
            }

            // create path if not existing
            string? dir = Path.GetDirectoryName(resolvedPath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Build table data
            string qtyStr = _quantity.ToString();
            string FormatDouble(double? v, string fmt = "G6") => v.HasValue && !double.IsNaN(v.Value) && !double.IsInfinity(v.Value) ? v.Value.ToString(fmt) : "-";

            // Main rows
            var summaryRows = new List<(string Key, string Value)>
            {
                ($"Quantity", qtyStr),
                ($"n", N.ToString()),
                ($"Alpha", Alpha.ToString("G4")),
                ($"<{_quantity}>", FormatDouble(AvgQ)),
                ($"S<{_quantity}>", FormatDouble(SQ)),
                ($"t(α, n)", FormatDouble(TAlphaN)),
                ($"δ<{_quantity}>", FormatDouble(DeltaQT)),
                ($"E, %", FormatDouble(E * 100))
            };

            // Detail rows
            var detailsHeader = new[] { "i", $"{_quantity}ᵢ", $"Δ{_quantity}ᵢ" };
            int mCount = _examplesArr?.Length ?? 0;
            var detailRows = new List<string[]>();
            for (int i = 0; i < mCount; i++)
            {
                string qi = FormatDouble(_examplesArr?[i]);
                string dqi = (DeltaQi != null && i < DeltaQi.Length) ? FormatDouble(DeltaQi[i]) : "-";
                detailRows.Add(new[] { (i + 1).ToString(), qi, dqi });
            }

            var bg = Color.White;
            var grid = Color.FromArgb(220, 220, 220);
            var headerBg = Color.FromArgb(245, 245, 245);
            var text = Color.Black;
            var titleColor = Color.FromArgb(40, 40, 40);

            int pad = 12;            
            int rowH = 26;          
            int headerH = 30;       
            int titlePad = 12;       
            int sectionGap = 16;    

            using var titleFont = new Font("Segoe UI", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            using var headerFont = new Font("Segoe UI", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            using var cellFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            using var tmpBmp = new Bitmap(1, 1);
            using var tmpG = Graphics.FromImage(tmpBmp);
            tmpG.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


            int sumCol0 = (int)Math.Ceiling(summaryRows.Select(r => tmpG.MeasureString(r.Key, cellFont).Width).DefaultIfEmpty(0).Max()) + 2 * pad;
            int sumCol1 = (int)Math.Ceiling(summaryRows.Select(r => tmpG.MeasureString(r.Value, cellFont).Width).DefaultIfEmpty(0).Max()) + 2 * pad;
            int summaryWidth = Math.Max((int)Math.Ceiling(tmpG.MeasureString("Summary", headerFont).Width) + 2 * pad, sumCol0 + sumCol1);

            int[] detCols = new int[detailsHeader.Length];
            for (int c = 0; c < detailsHeader.Length; c++)
            {
                float headW = tmpG.MeasureString(detailsHeader[c], headerFont).Width;
                float cellW = detailRows.Select(r => tmpG.MeasureString(r[c], cellFont).Width).DefaultIfEmpty(0f).Max();
                detCols[c] = (int)Math.Ceiling(Math.Max(headW, cellW)) + 2 * pad;
            }
            int detailsWidth = detCols.Sum();

            int contentWidth = Math.Max(summaryWidth, detailsWidth);
            int titleHeight = !string.IsNullOrWhiteSpace(title) ? (int)Math.Ceiling(tmpG.MeasureString(title!, titleFont).Height) + titlePad : 0;

            int summaryHeight = headerH + summaryRows.Count * rowH;
            int detailsHeight = (detailRows.Count > 0 ? headerH + detailRows.Count * rowH : 0);

            int width = contentWidth + 2;
            int height = titleHeight + summaryHeight + (detailsHeight > 0 ? sectionGap + detailsHeight : 0) + 2;

            using var bmp = new Bitmap(Math.Max(width, 320), Math.Max(height, 200));
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(bg);

            int x = 1;
            int y = 1;

            // Title
            if (!string.IsNullOrWhiteSpace(title))
            {
                using var titleBrush = new SolidBrush(titleColor);
                g.DrawString(title!, titleFont, titleBrush, x + pad, y);
                y += titleHeight;
            }

            // Summary section
            using (var gridPen = new Pen(grid))
            using (var headerBrush = new SolidBrush(headerBg))
            using (var textBrush = new SolidBrush(text))
            {
                // Header background
                g.FillRectangle(headerBrush, x, y, summaryWidth, headerH);
                g.DrawRectangle(gridPen, x, y, summaryWidth, headerH);
                g.DrawString("Summary", headerFont, textBrush, x + pad, y + (headerH - headerFont.Height) / 2);
                y += headerH;

                int kx = x;
                int vx = x + sumCol0;
                foreach (var (Key, Value) in summaryRows)
                {
                    // Key cell
                    g.DrawRectangle(gridPen, kx, y, sumCol0, rowH);
                    g.DrawString(Key, cellFont, textBrush, kx + pad, y + (rowH - cellFont.Height) / 2);

                    // Value cell
                    g.DrawRectangle(gridPen, vx, y, sumCol1, rowH);
                    g.DrawString(Value, cellFont, textBrush, vx + pad, y + (rowH - cellFont.Height) / 2);

                    y += rowH;
                }

                // Details section (if any)
                if (detailRows.Count > 0)
                {
                    y += sectionGap;

                    // Header background
                    g.FillRectangle(headerBrush, x, y, detailsWidth, headerH);
                    g.DrawRectangle(gridPen, x, y, detailsWidth, headerH);

                    int cx = x;
                    for (int c = 0; c < detailsHeader.Length; c++)
                    {
                        g.DrawLine(gridPen, cx, y, cx, y + headerH);
                        g.DrawString(detailsHeader[c], headerFont, textBrush, cx + pad, y + (headerH - headerFont.Height) / 2);
                        cx += detCols[c];
                    }
                    g.DrawLine(gridPen, x + detailsWidth, y, x + detailsWidth, y + headerH);
                    y += headerH;

                    // Rows
                    foreach (var row in detailRows)
                    {
                        cx = x;
                        for (int c = 0; c < row.Length; c++)
                        {
                            g.DrawRectangle(gridPen, cx, y, detCols[c], rowH);
                            g.DrawString(row[c], cellFont, textBrush, cx + pad, y + (rowH - cellFont.Height) / 2);
                            cx += detCols[c];
                        }
                        y += rowH;
                    }
                }
            }

            // Save
            try
            {
                if (File.Exists(resolvedPath))
                {
                    if (overwrite)
                    {
                        File.Delete(resolvedPath);
                    }
                    else
                    {
                        string name = Path.GetFileNameWithoutExtension(resolvedPath);
                        string ext = Path.GetExtension(resolvedPath);
                        string baseDir = string.IsNullOrEmpty(dir) ? "." : dir!;
                        string candidate;
                        int i = 1;
                        do
                        {
                            candidate = Path.Combine(baseDir, $"{name}_{DateTime.Now:yyyyMMdd_HHmmss}_{i}{ext}");
                            i++;
                        } while (File.Exists(candidate));
                        resolvedPath = candidate;
                    }
                }

                bmp.Save(resolvedPath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex) when (ex is ExternalException || ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new InvalidOperationException($"Failed to save table image to '{resolvedPath}'. Ensure the path is valid, you have write permissions, and the file is not open in another program.", ex);
            }
        }
    }
}


