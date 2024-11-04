using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;

namespace GrainElevatorAPI.Extensions;

public static class OutputInvoiceExtensions
{
    public static void UpdateFromRequest(this OutputInvoice outputInvoice, OutputInvoiceUpdateRequest request)
    {
        outputInvoice.InvoiceNumber = request.InvoiceNumber ?? outputInvoice.InvoiceNumber;
        outputInvoice.ShipmentDate = request.ShipmentDate ?? outputInvoice.ShipmentDate;
        outputInvoice.VehicleNumber = request.VehicleNumber ?? outputInvoice.VehicleNumber;
        outputInvoice.ProductWeight = request.ProductWeight ?? outputInvoice.ProductWeight;
    }
}