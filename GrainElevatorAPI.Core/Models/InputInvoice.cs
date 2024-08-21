namespace GrainElevatorAPI.Core.Models;

public partial class InputInvoice
{
    public int Id { get; set; }

    public string InvNumber { get; set; } = null!;

    public DateTime ArrivalDate { get; set; }

    public string VehicleNumber { get; set; } = null!;

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public int PhysicalWeight { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual LaboratoryCard? LaboratoryCard { get; set; }

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;


    public InputInvoice() { }

    public InputInvoice(string invNumber, DateTime date, string vehicleNumber, int supplierId, int productTitleId, int physicalWeight)
    {
        InvNumber = invNumber;
        ArrivalDate = date;
        VehicleNumber = vehicleNumber;
        SupplierId = supplierId;
        ProductTitleId = productTitleId;
        PhysicalWeight = physicalWeight;
    }










        // ДЛЯ ТЕСТА НА КОНСОЛИ===================================================================================

        public override string ToString()
    {
        return $"Приходная накладная №{InvNumber}\n" +
               $"Дата прихода:        {ArrivalDate.ToString("dd.MM.yyyy")}\n" +
               $"Номер ТС:            {VehicleNumber}\n" +
               $"Поставщик:           {Supplier}\n" +
               $"Наименование:        {ProductTitle}\n" +
               $"Вес нетто:           {PhysicalWeight} кг\n";
    }
}

