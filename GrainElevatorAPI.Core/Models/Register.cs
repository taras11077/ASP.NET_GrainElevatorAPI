using System.Text;

namespace GrainElevatorAPI.Core.Models;

public partial class Register
{
    public int Id { get; set; }

    public int RegisterNumber { get; set; }

    public DateTime ArrivalDate { get; set; }

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public int PhysicalWeightReg { get; set; }

    public int ShrinkageReg { get; set; }

    public int WasteReg { get; set; }

    public int AccWeightReg { get; set; }

    public double QuantityesDryingReg { get; private set; }

    public int? CompletionReportId { get; set; }
    public virtual CompletionReport? CompletionReport { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();

    public virtual Supplier Supplier { get; set; } = null!;


    public Register()
    { }

    public Register(int regNum, double weedinessBase, double moistureBase, List<LaboratoryCard> laboratoryCards)
    {
        if (laboratoryCards.Count != 0)
        {
            ProductionBatches = new List<ProductionBatch>();

            RegisterNumber = regNum;
            ArrivalDate = laboratoryCards[0].IdNavigation.ArrivalDate;
            SupplierId = laboratoryCards[0].IdNavigation.SupplierId;
            ProductTitleId = laboratoryCards[0].IdNavigation.ProductTitleId;

            InitProductionBatches(weedinessBase, moistureBase, laboratoryCards);
        }
    }

    private void InitProductionBatches(double weedinessBase, double moistureBase, List<LaboratoryCard> laboratoryCards)
    {
        foreach (var lc in laboratoryCards!)
        {
            try
            {
                ProductionBatch pb = new ProductionBatch(lc.IdNavigation, lc, weedinessBase, moistureBase);
                ProductionBatches.Add(pb);
                double QuantityesDryingProductionBatche = ((lc.IdNavigation.PhysicalWeight - pb.Waste) * (lc.Moisture - pb.MoistureBase) / 1000);
                PhysicalWeightReg += lc.IdNavigation.PhysicalWeight;
                AccWeightReg += pb.AccountWeight;
                WasteReg += pb.Waste;
                ShrinkageReg += pb.Shrinkage;
                QuantityesDryingReg += QuantityesDryingProductionBatche < 0 ? 0 : QuantityesDryingProductionBatche;
            }
            catch (Exception ex)
            {
                // TODO
            }
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    // ТЕСТ ДЛЯ КОНСОЛИ ==============================================================================================================
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine($"Реестр:          №{RegisterNumber}");
        result.AppendLine($"Поставщик:       {Supplier}");
        result.AppendLine($"Наименование:    {ProductTitle}");
        result.AppendLine($"Наименование:    {ProductionBatches.ToString()}");


        //if (ProductionBatches != null)
        //{
        //    foreach (var pb in ProductionBatches)
        //        result.AppendLine(pb.PrintProductionBatch(pb.IdNavigation.IdNavigation, pb.IdNavigation));
        //}



        return result.ToString();
    }

}

