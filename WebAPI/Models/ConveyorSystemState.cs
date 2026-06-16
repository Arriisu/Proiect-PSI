namespace WebAPI.Models
{
    public class ConveyorSystemState
    {
        public bool S0 { get; set; }
        public bool S1 { get; set; }
        public bool S2 { get; set; }
        public bool S3 { get; set; }
        public bool S4 { get; set; }
        public bool S5 { get; set; }
        public bool S6 { get; set; }
        public bool S7 { get; set; }
        public bool S8 { get; set; }
        public bool P1 { get; set; }
        public bool P2 { get; set; }
        public bool P3 { get; set; }
        public bool P4 { get; set; }
        public int CurrentMode { get; set; }
        public bool IsAlarmActive { get; set; }
        public string MesajSistem { get; set; } = string.Empty;
    }

    public class StateLogDto
    {
        public ConveyorSystemState State { get; set; } = new();
        public string ActionDescription { get; set; } = string.Empty;
        public bool AlarmActive { get; set; } = false;
    }

}