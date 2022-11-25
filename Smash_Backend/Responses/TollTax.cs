namespace Smash_Backend.Responses
{
    public class TollTax
    {
        public decimal baseRate { get; set; }
        public decimal totalDistance { get; set; }
        public decimal totalDistanceCost { get; set; }
        public decimal subTotal { get; set; }
        public decimal discounts { get; set; }
        public decimal netTax { get; set; }
    }
}
