using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Models;

public class ProductionBatch
{
    public int Id { get; set; }
    public double WeedinessBase { get; set; }

    public double MoistureBase { get; set; }

    public int Waste { get; set; }

    public int Shrinkage { get; set; }

    public int AccountWeight { get; set; }

    public int RegisterId { get; set; }

    public virtual LaboratoryCard IdNavigation { get; set; } = null!;

    public virtual Register Register { get; set; } = null!;



    public ProductionBatch() { }

    public ProductionBatch(InputInvoice inv, LaboratoryCard lc, double weedinessBase, double moistureBase)
    {
        Id = lc.Id;
        WeedinessBase = weedinessBase;
        MoistureBase = moistureBase;

        ProductionButchCalculator pbCalculator = new(inv, lc, this);

        pbCalculator.CalcResultProduction();
    }


    
    
    
    
    
    
    
    
    // вывод информации (ДЛЯ ТЕСТА НА КОНСОЛИ)
    public override string ToString()
    {
        return WeedinessBase.ToString();
          
    }


    public void PrintProductionBatch(InputInvoice inv, LaboratoryCard lc)
    {
        if (this != null)
        {
            Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", inv.ArrivalDate.ToString("dd.MM.yyyy"), inv.InvNumber, inv.PhysicalWeight, lc.Moisture, Shrinkage, lc.Weediness, Waste, AccountWeight);
            Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
        }
    }
}

