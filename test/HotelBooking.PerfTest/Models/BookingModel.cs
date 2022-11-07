namespace HotelBooking.PerfTest.Models
{
    public class BookingModel
    {
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? totalprice { get; set; }
        public string? depositpaid { get; set; }
        public DateModel? bookingdates { get; set; }
    }

    public class DateModel
    {
        public string? checkin { get; set; }
        public string? checkout { get; set; }
    }
}
