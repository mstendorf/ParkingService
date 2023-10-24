namespace CarTypeService.Models
{
    public class CarDescription
    {
        public CarDescription()
        {
            Make = "";
            Model = "";
            Variant = "";
        }

        public CarDescription(string make, string model, string variant)
        {
            Make = make;
            Model = model;
            Variant = variant;
        }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }

        public static CarDescription NONE = new();
    }
}
