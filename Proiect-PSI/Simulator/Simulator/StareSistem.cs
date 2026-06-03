using System;

namespace Simulator
{
    public class StareSistem
    {
        private readonly object _lock = new();

        // --- Stările Butoanelor și Senzorilor (S0 - S8) ---
        public bool S0 { get; private set; } // Stop General (Ciuperca)
        public bool S1 { get; private set; } // Start Banda 1
        public bool S2 { get; private set; } // Start Banda 2
        public bool S3 { get; private set; } // Start Banda 3
        public bool S4 { get; private set; } // Start Banda 4
        public bool S5 { get; private set; } // Stop Benzile de intrare (1 & 2)
        public bool S6 { get; private set; } // Senzor/Clapetă stânga (Spre Banda 3)
        public bool S7 { get; private set; } // Senzor/Clapetă mijloc (Tranziție)
        public bool S8 { get; private set; } // Senzor/Clapetă dreapta (Spre Banda 4)

        // --- Stările Lămpilor/Motoarelor (P1 - P4) ---
        public bool P1 { get; private set; } // Lampă/Motor Banda 1
        public bool P2 { get; private set; } // Lampă/Motor Banda 2
        public bool P3 { get; private set; } // Lampă/Motor Banda 3
        public bool P4 { get; private set; } // Lampă/Motor Banda 4

        // --- Alarme și Mesaje ---
        public bool IsAlarmActive { get; private set; }
        public string MesajSistem { get; private set; } = "Sistem Pregătit";

        // --- Metode pentru interacțiunea din Interfață (UI) ---

        public void ApasaStartBanda(int numarBanda)
        {
            lock (_lock)
            {
                // Nu putem porni dacă Stop General sau Alarma sunt active
                if (S0 || IsAlarmActive) return;

                switch (numarBanda)
                {
                    case 1: S1 = true; S5 = false; break;
                    case 2: S2 = true; S5 = false; break;
                    case 3: S3 = true; break;
                    case 4: S4 = true; break;
                }
                MesajSistem = $"Comandă pornire trimisă pentru Banda {numarBanda}";
            }
        }

        public void ApasaStopBanda3Sau4(int numarBanda)
        {
            lock (_lock)
            {
                if (numarBanda == 3) S3 = false;
                if (numarBanda == 4) S4 = false;
            }
        }

        public void ApasaStopBenzileIntrareS5()
        {
            lock (_lock)
            {
                S5 = true;
                S1 = false;
                S2 = false;
                // Motoarele vor fi oprite de Worker în următorul ciclu
            }
        }

        public void ApasaStopGeneralS0()
        {
            lock (_lock)
            {
                S0 = true;
                ResetSistem();
                MesajSistem = "STOP GENERAL ACTIVAT!";
            }
        }

        public void ElibereazaStopGeneralS0()
        {
            lock (_lock)
            {
                S0 = false;
                MesajSistem = "Sistem deblocat. Gata de pornire.";
            }
        }

        public void SetPozitieClapeta(bool stangaS6, bool mijlocS7, bool dreaptaS8)
        {
            lock (_lock)
            {
                S6 = stangaS6;
                S7 = mijlocS7;
                S8 = dreaptaS8;
            }
        }

        // --- Metode folosite de Worker (LogicaM23) pentru a actualiza ieșirile ---

        public void ActualizeazaFunctionareBenzi(bool p1, bool p2, bool p3, bool p4)
        {
            lock (_lock)
            {
                P1 = p1;
                P2 = p2;
                P3 = p3;
                P4 = p4;
            }
        }

        public void SetAlarma(bool activa)
        {
            lock (_lock)
            {
                IsAlarmActive = activa;
                if (activa)
                {
                    P1 = P2 = P3 = P4 = false;
                    S1 = S2 = S3 = S4 = false;
                    MesajSistem = "ALARMĂ CRITICĂ!";
                }
            }
        }

        public void ResetSistem()
        {
            lock (_lock)
            {
                P1 = P2 = P3 = P4 = false;
                S1 = S2 = S3 = S4 = false;
                S5 = false;
                IsAlarmActive = false;
            }
        }
    }
}