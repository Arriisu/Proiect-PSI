using System;

namespace Simulator
{
    public class StareSistem
    {
        private readonly object _lock = new();

        public bool S0 { get; private set; } 
        public bool S1 { get; private set; } 
        public bool S2 { get; private set; } 
        public bool S3 { get; private set; } 
        public bool S4 { get; private set; } 
        public bool S5 { get; private set; } 
        public bool S6 { get; private set; } 
        public bool S7 { get; private set; } = true;
        public bool S8 { get; private set; } 

        public bool P1 { get; private set; } 
        public bool P2 { get; private set; } 
        public bool P3 { get; private set; } 
        public bool P4 { get; private set; } 

        public int CurrentMode { get; private set; } = 1;
        public bool IsAlarmActive { get; private set; }
        public bool IsAlarmSimulationRequested { get; private set; }
        public string MesajSistem { get; private set; } = "Sistem Pregatit";

        public void SeteazaMod(int mod)
        {
            lock (_lock) { CurrentMode = mod; }
        }

        public void DeclanșeazăSimulareAlarmă()
        {
            lock (_lock) { IsAlarmSimulationRequested = true; }
        }

        public void ResetAlarmSimulationRequest()
        {
            lock (_lock) { IsAlarmSimulationRequested = false; }
        }

        public void ApasaStartBanda(int numarBanda)
        {
            lock (_lock)
            {
                if (S0 || IsAlarmActive) return;

                switch (numarBanda)
                {
                    case 1: S1 = !S1; S5 = false; break;
                    case 2: S2 = !S2; S5 = false; break;
                    case 3: S3 = !S3; break;
                    case 4: S4 = !S4; break;
                }
            }
        }

        public void ApasaStopBenzileIntrareS5()
        {
            lock (_lock)
            {
                S5 = true; S1 = false; S2 = false;
            }
        }

        public void ApasaStopGeneralS0()
        {
            lock (_lock)
            {
                S0 = true; ResetSistem();
            }
        }

        public void ElibereazaStopGeneralS0()
        {
            lock (_lock) { S0 = false; }
        }

        public void SetPozitieClapeta(bool stangaS6, bool mijlocS7, bool dreaptaS8)
        {
            lock (_lock)
            {
                S6 = stangaS6; S7 = mijlocS7; S8 = dreaptaS8;
            }
        }

        public void ActualizeazaFunctionareBenzi(bool p1, bool p2, bool p3, bool p4)
        {
            lock (_lock)
            {
                P1 = p1; P2 = p2; P3 = p3; P4 = p4;
            }
        }

        public void SetAlarma(bool activa)
        {
            lock (_lock)
            {
                IsAlarmActive = activa;
                if (activa) ResetSistem();
            }
        }

        public void ResetSistem()
        {
            lock (_lock)
            {
                P1 = P2 = P3 = P4 = false;
                S1 = S2 = S3 = S4 = false;
                S5 = false;
            }
        }
    }
}