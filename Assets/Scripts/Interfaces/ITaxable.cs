public interface ITaxable
{
    public int TaxRevenue { get; set; }
    int GenerateTaxes();
}