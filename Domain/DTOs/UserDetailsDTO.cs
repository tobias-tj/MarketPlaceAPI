namespace Domain.DTOs
{
    public class UserDetailsDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? Location { get; set; }
        public bool Verified { get; set; }
        public decimal? BuyerRating { get; set; }
        public decimal? SellerRating { get; set; }
        public int? ItemsSold { get; set; }
        public int? ItemsBought { get; set; }
    }
}
