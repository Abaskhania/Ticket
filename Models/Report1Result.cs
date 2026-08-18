namespace SupportTicketSystem.Models
{
    public class Report1Result
    {
        public string FullName { get; set; }
        public int TotalTickets { get; set; }
        public int CompletedTickets { get; set; }
        public int InProcessTickets  { get; set; }
        public int ExpiredTickets { get; set; }

        public int? AverageResponseSeconds { get; set; }

        //public string AverageResponse {
        //    get {
        //        return (this.AverageResponseSeconds / 3600) + ":" + ((this.AverageResponseSeconds % 3600) / 60) + ":" + (this.AverageResponseSeconds % 60);
               
        //    } 
        //    set; }

    }
}
