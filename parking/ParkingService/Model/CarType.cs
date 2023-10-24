namespace CarTypeService.Models
{

    public class CarType
    {
        public string registration_number { get; set; }
        public string status { get; set; }
        public DateTime status_date { get; set; }
        public string type { get; set; }
        public string use { get; set; }
        public string first_registration { get; set; }
        public string vin { get; set; }
        public object own_weight { get; set; }
        public int cerb_weight { get; set; }
        public int total_weight { get; set; }
        public int axels { get; set; }
        public int pulling_axels { get; set; }
        public int seats { get; set; }
        public bool coupling { get; set; }
        public int trailer_maxweight_nobrakes { get; set; }
        public int trailer_maxweight_withbrakes { get; set; }
        public object doors { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string variant { get; set; }
        public string model_type { get; set; }
        public int model_year { get; set; }
        public string color { get; set; }
        public string chassis_type { get; set; }
        public int engine_cylinders { get; set; }
        public int engine_volume { get; set; }
        public int engine_power { get; set; }
        public string fuel_type { get; set; }
        public string registration_zipcode { get; set; }
        public long vehicle_id { get; set; }
        public Mot_Info mot_info { get; set; }
        public bool is_leasing { get; set; }
        public string leasing_from { get; set; }
        public string leasing_to { get; set; }
    }

    public class Mot_Info
    {
        public string type { get; set; }
        public string date { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public string status_date { get; set; }
        public int mileage { get; set; }
    }
}
