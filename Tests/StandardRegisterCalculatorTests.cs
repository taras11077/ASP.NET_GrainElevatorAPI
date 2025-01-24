using System;
using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Models;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests;

public class StandardRegisterCalculatorTests
{
    private readonly StandardRegisterCalculator _calculator;

    public StandardRegisterCalculatorTests()
    {
        _calculator = new StandardRegisterCalculator();
    }

    [Fact]
    public void CalcProductionBatch_ShouldCalculateCorrectWasteAndShrinkage()
    {
        // Arrange
        var inputInvoice = new InputInvoice
        {
            Id = 1,
            InvoiceNumber = "001",
            PhysicalWeight = 1000 
        };

        var laboratoryCard = new LaboratoryCard
        {
            InputInvoice = inputInvoice,
            WeedImpurity = 11.0, 
            Moisture = 18.0,
            InputInvoiceId = inputInvoice.Id
        };

        var invoiceRegister = new InvoiceRegister
        {
            WeedImpurityBase = 1,
            MoistureBase = 8 
        };

        var productionBatch = new ProductionBatch();

        // Act
        var result = _calculator.CalcProductionBatch(laboratoryCard, invoiceRegister, productionBatch);

        // Assert
        Assert.NotNull(result);

        // Перевірка конкретних значень
        Assert.Equal(101, productionBatch.Waste);           // Очікується, що відходи = 101 кг
        Assert.Equal(97, productionBatch.Shrinkage);        // Очікується, що усушка = 97 кг
        Assert.Equal(802, productionBatch.AccountWeight);   // Очікується, що облікова вага = 802 кг
    }


    [Fact]
    public void CalcProductionBatch_ShouldThrowException_WhenPhysicalWeightIsZero()
    {
        // Arrange
        var inputInvoice = new InputInvoice
        {
            Id = 1,
            PhysicalWeight = 0
        };

        var laboratoryCard = new LaboratoryCard
        {
            InputInvoice = inputInvoice,
            WeedImpurity = 5.0,
            Moisture = 10.0,
            InputInvoiceId = inputInvoice.Id
        };

        var invoiceRegister = new InvoiceRegister();
        var productionBatch = new ProductionBatch();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _calculator.CalcProductionBatch(laboratoryCard, invoiceRegister, productionBatch)
        );

        Assert.Contains("Фізична вага має бути більшою за 0", exception.Message);
    }

    [Fact]
    public void AddProductionBatch_ShouldUpdateInvoiceRegisterFields()
    {
        // Arrange
        var invoiceRegister = new InvoiceRegister
        {
            PhysicalWeightReg = 0,
            ShrinkageReg = 0,
            WasteReg = 0,
            AccWeightReg = 0,
            QuantitiesDryingReg = 0
        };

        var inputInvoice = new InputInvoice
        {
            Id = 1,
            PhysicalWeight = 1000
        };

        var productionBatch = new ProductionBatch
        {
            Waste = 50,
            Shrinkage = 20,
            AccountWeight = 930,
            QuantitiesDrying = 5
        };

        // Act
        var updatedRegister = _calculator.AddProductionBatch(inputInvoice, productionBatch, invoiceRegister);

        // Assert
        Assert.Equal(1000, updatedRegister.PhysicalWeightReg);
        Assert.Equal(20, updatedRegister.ShrinkageReg);
        Assert.Equal(50, updatedRegister.WasteReg);
        Assert.Equal(930, updatedRegister.AccWeightReg);
        Assert.Equal(5, updatedRegister.QuantitiesDryingReg);
    }
}

