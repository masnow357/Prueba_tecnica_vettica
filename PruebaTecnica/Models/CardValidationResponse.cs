namespace PruebaTecnica.Models
{
    public class CardValidationResponse
    {
        public string card { get; set; }
        public bool isValid { get; set; }
        public string status { get; set; }
        public int statusCode { get; set; }
    }
}
