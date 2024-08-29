using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Extensions;

public static class InputInvoiceExtensions
{
    public static void UpdateFromRequest(this InputInvoice inputInvoice, InputInvoiceUpdateRequest request)
    {
        inputInvoice.InvoiceNumber = request.InvoiceNumber ?? inputInvoice.InvoiceNumber;
        inputInvoice.ArrivalDate = request.ArrivalDate ?? inputInvoice.ArrivalDate;
        inputInvoice.SupplierId = request.SupplierId ?? inputInvoice.SupplierId;
        inputInvoice.ProductId = request.ProductId ?? inputInvoice.ProductId;
        inputInvoice.VehicleNumber = request.VehicleNumber ?? inputInvoice.VehicleNumber;
        inputInvoice.PhysicalWeight = request.PhysicalWeight ?? inputInvoice.PhysicalWeight;
    }
}