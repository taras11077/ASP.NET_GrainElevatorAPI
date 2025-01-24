using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.EnumsAndConstants;
using GrainElevatorAPI.Core.Models;

namespace Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class CompletionReportCalculatorTests
{
    private readonly CompletionReportCalculator _calculator;

    public CompletionReportCalculatorTests()
    {
        _calculator = new CompletionReportCalculator();
    }

    [Fact]
    public void CalculateWeights_ShouldCalculateCorrectValues()
    {
        // Arrange
        var registers = new List<InvoiceRegister>
        {
            new InvoiceRegister { PhysicalWeightReg = 1000, QuantitiesDryingReg = 50, ShrinkageReg = 20, WasteReg = 10, AccWeightReg = 900 },
            new InvoiceRegister { PhysicalWeightReg = 2000, QuantitiesDryingReg = 30, ShrinkageReg = 50, WasteReg = 20, AccWeightReg = 1800 }
        };
        var report = new CompletionReport();

        // Act
        _calculator.CalculateWeights(registers, report);

        // Assert
        const double tolerance = 1e-6; // допустима похибка
        Assert.Equal(3.0, report.PhysicalWeightReport.GetValueOrDefault(), tolerance); // 1000+2000 -> 3.0 (in tons)
        Assert.Equal(80, report.QuantitiesDryingReport.GetValueOrDefault()); // 50+30
        Assert.Equal(0.07, report.ShrinkageReport.GetValueOrDefault(), tolerance); // (20+50)*0.001
        Assert.Equal(0.03, report.WasteReport.GetValueOrDefault(), tolerance); // (10+20)*0.001
        Assert.Equal(2.7, report.AccWeightReport.GetValueOrDefault(), tolerance); // (900+1800)*0.001
    }


    [Fact]
    public void CalculateWeights_ShouldThrowException_WhenRegistersIsNull()
    {
        // Arrange
        CompletionReport report = new CompletionReport();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _calculator.CalculateWeights(null, report));
    }

    [Fact]
    public void CalculateWeights_ShouldThrowException_WhenReportIsNull()
    {
        // Arrange
        var registers = new List<InvoiceRegister>
        {
            new InvoiceRegister { PhysicalWeightReg = 1000, QuantitiesDryingReg = 50 }
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _calculator.CalculateWeights(registers, null));
    }

    [Fact]
    public void MapOperationToReportField_ShouldReturnCorrectField()
    {
        // Arrange
        var report = new CompletionReport
        {
            PhysicalWeightReport = 3.0,
            QuantitiesDryingReport = 80,
            ShrinkageReport = 0.07,
            WasteReport = 0.03,
            AccWeightReport = 3.6
        };

        var operation = new TechnologicalOperation { Title = TechnologicalOperationNames.Drying };

        // Act
        var result = _calculator.MapOperationToReportField(operation, report);

        // Assert
        Assert.Equal(80, result);
    }

    [Fact]
    public void CalculateTotalCost_ShouldCalculateCorrectTotalCost()
    {
        // Arrange
        var priceList = new PriceList
        {
            PriceListItems = new List<PriceListItem>
            {
                new PriceListItem { TechnologicalOperationId = 1, OperationPrice = 100 },
                new PriceListItem { TechnologicalOperationId = 2, OperationPrice = 200 }
            }
        };

        var completionReport = new CompletionReport
        {
            CompletionReportOperations = new List<CompletionReportOperation>
            {
                new CompletionReportOperation { TechnologicalOperationId = 1, Amount = 2 },
                new CompletionReportOperation { TechnologicalOperationId = 2, Amount = 1.5 }
            }
        };

        // Act
        _calculator.CalculateTotalCost(completionReport, priceList);

        // Assert
        Assert.Equal(2 * 100 + 1.5m * 200, completionReport.TotalCost); // 500
    }

    [Fact]
    public void CalculateTotalCost_ShouldThrowException_WhenPriceListIsNull()
    {
        // Arrange
        var completionReport = new CompletionReport();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _calculator.CalculateTotalCost(completionReport, null));
    }

    [Fact]
    public void CalculateTotalCost_ShouldThrowException_WhenCompletionReportIsNull()
    {
        // Arrange
        var priceList = new PriceList();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _calculator.CalculateTotalCost(null, priceList));
    }
}
