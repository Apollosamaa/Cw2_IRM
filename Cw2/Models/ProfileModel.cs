namespace Cw2.Models
{
    public class ProfileModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }  
        public string Location { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        // Add more properties as needed
    }
}
