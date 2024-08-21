namespace GrainElevatorAPI.Core.Models;

public partial class OutputInvoice
{
    public int Id { get; set; }

    public string OutInvNumber { get; set; } = null!;

    public DateTime ShipmentDate { get; set; }

    public string VehicleNumber { get; set; } = null!;

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public int DepotItemId { get; set; }

    public string Category { get; set; } = null!;

    public int ProductWeight { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual DepotItem DepotItem { get; set; } = null!;

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;


    public OutputInvoice() { }

    public OutputInvoice(string outInvNumber, DateTime date, string venicleNumber, int supplierId, int productTitleId, string category, int productWeight)
    {
        OutInvNumber = outInvNumber;
        ShipmentDate = date;
        VehicleNumber = venicleNumber;
        SupplierId = supplierId;
        ProductTitleId = productTitleId;
        Category = category;
        ProductWeight = productWeight;
    }

    public OutputInvoice(string outInvNumber, DateTime date, string vehicleNumber, DepotItem depotItem, string category, int productWeight)
    {
        OutInvNumber = outInvNumber;
        ShipmentDate = date;
        VehicleNumber = vehicleNumber;
        SupplierId = depotItem.SupplierId;
        ProductTitleId = depotItem.ProductTitleId;
        DepotItemId = depotItem.Id;
        Category = category;
        ProductWeight = productWeight;
    }

    public bool Shipment(DepotItem depotItem)
    {
        if (depotItem.DepotItemsCategories.Count == 0)
        {
            //NotificationManager.ShowErrorMessageBox("Нет продукции");
            return false;
        }

        foreach (var c in depotItem.DepotItemsCategories)
        {
            if (c.CategoryTitle == Category)
            {
                if (c.CategoryValue >= ProductWeight)
                    c.CategoryValue -= ProductWeight;
                
                else
                {
                    //NotificationManager.ShowErrorMessageBox("На складе нет необходимого количества указанной продукции");
                    return false;
                }
            }
        }
        return true;
    }






    // вивод на консоль
    public override string ToString()
    {
        return $"\nРасходная накладная № {OutInvNumber}.\n" +
               $"---------------------------\n" +
               $"Дата отгрузки:             {ShipmentDate.ToString("dd.MM.yyyy")}\n" +
               $"Номер ТС:                  {VehicleNumber}\n" +
               $"Категория продукции:       {Category}\n" +
               $"Вес нетто:                 {ProductWeight} кг\n\n";
    }
}

