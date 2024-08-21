namespace GrainElevatorAPI.Core.Models;

public partial class LaboratoryCard
{
    public int Id { get; set; }

    public int LabCardNumber { get; set; }

    public double Weediness { get; set; }

    public double Moisture { get; set; }

    public double? GrainImpurity { get; set; }

    public string? SpecialNotes { get; set; }

    public bool? IsProduction { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual InputInvoice IdNavigation { get; set; } = null!;

    public virtual ProductionBatch? ProductionBatch { get; set; }

    public LaboratoryCard() { }

    public LaboratoryCard(InputInvoice inv, int labCardNumber, double weediness, double moisture, double grainImpurity = 0, string specialNotes = "", bool? isProduction = false)
    {
        Id = inv.Id;
        LabCardNumber = labCardNumber;
        Weediness = weediness;
        Moisture = moisture;
        GrainImpurity = grainImpurity;
        SpecialNotes = specialNotes;
        IsProduction = isProduction;

   

    }





    // для теста на КОНСОЛИ===================================================================================================
    public override string ToString()
    {
        return $"Id:                      {(Id != null ? Id.ToString() : "тут null")}\n" +
               $"Номер Карточки анализа: {(LabCardNumber != null ? LabCardNumber.ToString() : "тут null")}\n" +
               $"Сорная примесь:         {(Weediness != null ? Weediness.ToString() : "тут null")}\n" +
               $"Влажность:              {(Moisture != null ? Moisture.ToString() : "тут null")}\n" +
               $"GrainImpurity:          {(GrainImpurity != null ? GrainImpurity.ToString() : "тут null")}\n" +
               $"SpecialNotes:           {(SpecialNotes != null ? SpecialNotes.ToString() : "тут null")}\n" +
               $"IsProduction:           {(IsProduction != null ? IsProduction.ToString() : "тут null")}\n" +
               $"User's id who created:  {(CreatedByNavigation != null && CreatedByNavigation.Id != null ? CreatedByNavigation.Id.ToString() : "тут null")}\n" +
               $"ProductionBatch:        {(ProductionBatch != null ? ProductionBatch.ToString() : "тут null")}\n" +
               $"IdNavigation id:        {(IdNavigation != null && IdNavigation.Id != null ? IdNavigation.Id.ToString() : "тут null")}\n" +
               $"IdNavigation date:      {(IdNavigation != null ? IdNavigation.ArrivalDate : "тут null")}\n"+
               $"IdNavigation ProductTitleId: {(IdNavigation != null ? IdNavigation.ProductTitleId : "тут null")}\n" +
               $"IdNavigation SupplierId:     {(IdNavigation != null ? IdNavigation.SupplierId : "тут null")}\n" +
               $"IdNavigation VehicleNumber:  {(IdNavigation != null ? IdNavigation.VehicleNumber : "тут null")}\n";
    }

}

