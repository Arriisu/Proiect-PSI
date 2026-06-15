namespace WebAPI.Models
{
    public class StateLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ChangedByUser { get; set; } = "system";
        public string ActionDescription { get; set; } = string.Empty;
        public string StateJson { get; set; } = "{}"; // placeholder for now, for conveyor belt state later
        public bool AlarmActive { get; set; } = false; //pt a sti cand a fost activa alarma si cand s-a oprit
    }
}