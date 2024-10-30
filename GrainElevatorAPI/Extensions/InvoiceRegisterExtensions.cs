using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs.Requests;

namespace GrainElevatorAPI.Extensions;

public static class InvoiceRegisterExtensions
{
    public static void UpdateFromRequest(this InvoiceRegister invoiceRegister, InvoiceRegisterUpdateRequest request)
    {
        invoiceRegister.RegisterNumber = request.RegisterNumber ?? invoiceRegister.RegisterNumber;
        invoiceRegister.WeedImpurityBase = request.WeedImpurityBase ?? invoiceRegister.WeedImpurityBase;
        invoiceRegister.MoistureBase = request.MoistureBase ?? invoiceRegister.MoistureBase;
        invoiceRegister.ProductionBatches = request.ProductionBatches ?? invoiceRegister.ProductionBatches;
    }
}