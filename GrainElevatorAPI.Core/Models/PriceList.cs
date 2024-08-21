namespace GrainElevatorAPI.Core.Models;

public partial class PriceList
{
    public int Id { get; set; }

    public string ProductTitle { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<PriceByOperation> PriceByOperations { get; set; } = new List<PriceByOperation>();


    public PriceList() { }

    public PriceList(string productTitle)
    {
        ProductTitle = productTitle;

        //OperationPrices = new Dictionary<string, double>()
        //    {
        //        { "Приемка", 130.00 },
        //        { "Первичная очистка", 1290.00 },
        //        { "Сушка в шахтной сушилке", 900.00 }
        //    };
    }


    public void AddOperation(string operationTitle, double operationPrice)
    {
        try
        {
            PriceByOperations?.Add(new PriceByOperation(operationTitle, operationPrice));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void AddOperationRange(params PriceByOperation[] operatoinPrices)
    {
        try
        {
            if (PriceByOperations as List<PriceByOperation> == null)
                return;

            foreach (var op in (PriceByOperations as List<PriceByOperation>)!)
                PriceByOperations?.Add(op);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void RemoveOperation(string operationTitle)
    {
        try
        {
            if (PriceByOperations as List<PriceByOperation> == null)
                return;

            foreach (var op in (PriceByOperations as List<PriceByOperation>)!)
            {
                if (op.OperationTitle == operationTitle)
                    (PriceByOperations as List<PriceByOperation>)?.Remove(op);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }











    // тест на КОНСОЛЬ ===============================================================================
    public void PrintPrice()
    {
        if (PriceByOperations == null)
        {
            Console.WriteLine("Прайс не сформирован.");
            return;
        }

        Console.WriteLine($"Наименование:    {ProductTitle}");

        foreach (var op in PriceByOperations)
            Console.WriteLine($"{op.OperationTitle} || {string.Format("{0:F2}", op.OperationPrice)} грн.");

        Console.WriteLine("\n");
    }

}

