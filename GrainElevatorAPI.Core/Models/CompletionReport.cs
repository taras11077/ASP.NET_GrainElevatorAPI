using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Models
{
    public partial class CompletionReport
    {
        public int Id { get; set; }

        public int ReportNumber { get; set; }

        public DateTime ReportDate { get; set; }

        public int SupplierId { get; set; }

        public int ProductTitleId { get; set; }

        public int? PriceListId { get; set; }

        public double QuantityesDrying { get; set; }

        public double PhysicalWeightReport { get; set; }

        public bool IsFinalized { get; set; }

        public int? CreatedBy { get; set; }

  
        public virtual User? CreatedByNavigation { get; set; }

        public virtual PriceList? PriceList { get; set; }

        public virtual ProductTitle ProductTitle { get; set; } = null!;

        public virtual Supplier Supplier { get; set; } = null!;

        public virtual ICollection<Register> Registers { get; set; } = new List<Register>();

        public virtual ICollection<TechnologicalOperation> TechnologicalOperations { get; set; } = new List<TechnologicalOperation>();

        public CompletionReport()
        { }

        public CompletionReport(int reportNum, DateTime date, List<Register> registers)
        {
            ICompletionReportCalculator cRCalculator = new CompletionReportCalculator(this);

            if (registers != null)
            {
                ReportNumber = reportNum;
                ReportDate = date;


                SupplierId = (registers as List<Register>)![0].SupplierId;
                ProductTitleId = (registers as List<Register>)![0].ProductTitleId;
                Registers = registers;
        
                PhysicalWeightReport = cRCalculator.CalcSumWeightReport();
                QuantityesDrying = cRCalculator.CalcDryingQuantity();

            }
            initTechnologicalOperationList();
            initTechnologicalOperationsValue();
        }

        // первоначальная инициализация списка технологических операций
        private void initTechnologicalOperationList()
        {
            TechnologicalOperations = new List<TechnologicalOperation>()
            {
                new TechnologicalOperation("Приемка"),
                new TechnologicalOperation("Первичная очистка"),
                new TechnologicalOperation("Сушка в шахтной сушилке"),
            };
        }

        // присвоение технологическим операциям переменних количественних значений
        private void initTechnologicalOperationsValue()
        {
            foreach (var op in TechnologicalOperations)
            {
                switch (op.Title)
                {
                    case "Приемка":
                        op.Amount = PhysicalWeightReport;
                        break;

                    case "Первичная очистка":
                        op.Amount = PhysicalWeightReport;
                        break;

                    case "Сушка в шахтной сушилке":
                        op.Amount = QuantityesDrying;
                        break;
                }
            }
        }

        // добавление в Акт доработки Технологических операций по одной
        private void addOperations(TechnologicalOperation operation)
        {
            if (operation == null)
                return;

            try
            {
                TechnologicalOperations?.Add(operation);
            }
            catch (Exception ex)
            {
                // TODO
            }

        }

        //рассчет Акта доработка по заданному Прайсу
        public void CalcByPrice(PriceList pl)
        {
            if (pl.PriceByOperations == null)
                return;

            CompletionReportCalculator cRCalculator = new(this);
            try
            {
                cRCalculator.CalcByPrice(pl);

                IsFinalized = true;
                PriceListId = pl.Id;
            }
            catch (Exception ex)
            {
               // TODO
            }
        }

    }
}
