using System.ComponentModel.DataAnnotations.Schema;

namespace GrainElevatorAPI.Core.EnumsAndConstants;

[NotMapped]
public static  class TechnologicalOperationNames
{
    public const string Reception = "Приймання Сировини (зважування, лабораторний аналіз, розвантаження)";
    public const string PrimaryCleaning = "Первинне очищення";
    public const string Drying = "Сушка в шахтній сушарці";
    public const string Shipping = "Відвантаження (зважування, лабораторний аналіз, завантаження в транспортний засіб)";
    public const string WasteDisposal = "Утилізація невикористовуваних відходів";
}