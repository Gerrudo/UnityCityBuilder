public interface IExpensable
{
    int Expenses { get; set; }
    
    int ConsumeTaxes();
}