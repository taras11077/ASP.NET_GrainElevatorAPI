namespace GrainElevatorAPI.Core.Models
{
    public partial class DepotItem
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public int ProductTitleId { get; set; }

        public virtual ICollection<DepotItemCategory> DepotItemsCategories { get; set; } = new List<DepotItemCategory>();

        public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();

        public virtual ProductTitle ProductTitle { get; set; } = null!;

        public virtual Supplier Supplier { get; set; } = null!;


        public DepotItem() { }


        public DepotItem(Register register)
    {
        SupplierId = register.SupplierId;
        ProductTitleId = register.ProductTitleId;
        DepotItemsCategories = new List<DepotItemCategory>()
        {
            new DepotItemCategory("Кондиційна продукція", register.AccWeightReg),
            new DepotItemCategory("Відходи ", register.WasteReg)
        };
    }


        public DepotItem(List<Register> registers)
    {
        SupplierId = registers[0].SupplierId;
        ProductTitleId = registers[0].ProductTitleId;
        DepotItemsCategories = new List<DepotItemCategory>();

        foreach (Register r in registers)
        {
            if (SupplierId == r.SupplierId && ProductTitleId == r.ProductTitleId)
            {
                foreach (var c in DepotItemsCategories)
                {
                    if (c.CategoryTitle == "Кондиційна продукція")
                        c.CategoryValue += r.AccWeightReg;

                    if (c.CategoryTitle == "Відходи")
                        c.CategoryValue += r.WasteReg;
                }
            }
            else
                Console.WriteLine("Постачальник або Назва продукції Реєстра не відповідають Складської одиниці");
        }
    }

        public bool IsAddedRegister(Register register)
    {
   
            if (SupplierId == register.SupplierId && ProductTitleId == register.ProductTitleId)
            {

                SupplierId = register.SupplierId;
                ProductTitleId = register.ProductTitleId;
       
                foreach (var c in DepotItemsCategories)
                {
                    if (c.CategoryTitle == "Кондиційна продукція")
                        c.CategoryValue += register.AccWeightReg;

                    if (c.CategoryTitle == "Відходи")
                        c.CategoryValue += register.WasteReg;
                }
                return true;
            }
            else { return false; }
     
    }

    }
}