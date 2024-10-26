using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Requests;

namespace GrainElevatorAPI.Extensions;

public static class InvoiceRegisterExtensions
{
    public static void UpdateFromRequest(this InvoiceRegister register, InvoiceRegisterUpdateRequest request)
    {
        register.RegisterNumber = request.RegisterNumber ?? register.RegisterNumber;
        register.WeedImpurityBase = request.WeedImpurityBase ?? register.WeedImpurityBase;
        register.MoistureBase = request.MoistureBase ?? register.MoistureBase;
        register.ProductionBatches = request.ProductionBatches ?? register.ProductionBatches;
    }
}