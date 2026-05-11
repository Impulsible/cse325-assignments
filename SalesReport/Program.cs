using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string directoryPath = "SalesFiles";
        string outputFile = "SalesSummary.txt";
        
        GenerateSalesSummary(directoryPath, outputFile);
        Console.WriteLine("Sales summary generated. Check SalesSummary.txt");
    }

    static void GenerateSalesSummary(string directoryPath, string outputFileName)
    {
        var report = new StringBuilder();
        decimal totalSales = 0;
        
        report.AppendLine("Sales Summary");
        report.AppendLine("----------------------------");
        
        string[] files = Directory.GetFiles(directoryPath);
        var fileSales = new List<(string fileName, decimal sales)>();
        
        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            if (decimal.TryParse(content.Trim(), out decimal salesAmount))
            {
                totalSales += salesAmount;
                string fileName = Path.GetFileName(file);
                fileSales.Add((fileName, salesAmount));
            }
        }
        
        report.AppendLine($" Total Sales: {totalSales:C}");
        report.AppendLine();
        report.AppendLine(" Details:");
        
        foreach (var (fileName, sales) in fileSales)
        {
            report.AppendLine($"  {fileName}: {sales:C}");
        }
        
        File.WriteAllText(outputFileName, report.ToString());
    }
}