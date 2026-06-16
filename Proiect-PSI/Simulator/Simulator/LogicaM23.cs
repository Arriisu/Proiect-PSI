using Microsoft.AspNetCore.SignalR;
using Simulator.Hubs;

namespace Simulator
{
    public class LogicaM23 : BackgroundService
    {
        private readonly StareSistem _stare;
        private readonly IHubContext<M23Hub> _hubContext;
        private int _alarmTicksRemaining = 0;

        public LogicaM23(StareSistem stare, IHubContext<M23Hub> hubContext) 
        {
            _stare = stare;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                bool p1 = false, p2 = false, p3 = false, p4 = false;

                int senzoriClapetaActivi = (_stare.S6 ? 1 : 0) + (_stare.S7 ? 1 : 0) + (_stare.S8 ? 1 : 0);
                
                if (senzoriClapetaActivi >= 2 || _stare.IsAlarmSimulationRequested)
                {
                    if (!_stare.IsAlarmActive)
                    {
                        _stare.SetAlarma(true);
                        _alarmTicksRemaining = 50; // 50 * 100ms = 5 secunde
                        _stare.ResetAlarmSimulationRequest();
                    }
                }

                if (_stare.IsAlarmActive)
                {
                    _alarmTicksRemaining--;
                    if (_alarmTicksRemaining <= 0)
                    {
                        _stare.SetAlarma(false); // resetare automata dupa 5 secunde
                    }
                }

                if (!_stare.S0 && !_stare.IsAlarmActive)
                {
                    p3 = _stare.S3;
                    p4 = _stare.S4;

                    if (!_stare.S5)
                    {
                        // FUNCTIONALITATEA 1
                        if (_stare.CurrentMode == 1)
                        {
                            if (_stare.S1)
                            {
                                if (_stare.S6 && p3) p1 = true;
                                else if (_stare.S8 && p4) p1 = true;
                                else if (_stare.S7 && (p3 || p4)) p1 = true;
                            }
                            // conditie: M2 merge doar daca M1 e oprit
                            if (_stare.S2 && !p1)
                            {
                                if (_stare.S6 && p4) p2 = true;
                                else if (_stare.S8 && p3) p2 = true;
                                else if (_stare.S7 && (p3 || p4)) p2 = p1 = false; // Interblocare
                            }
                        }
                        // FUNCTIONALITATEA 2
                        else if (_stare.CurrentMode == 2)
                        {
                            if (_stare.S7 && p3 && p4)
                            {
                                if (_stare.S1) p1 = true;
                                if (_stare.S2) p2 = true;
                            }
                            else
                            {
                                if (_stare.S1)
                                {
                                    if (_stare.S6 && p3) p1 = true;
                                    if (_stare.S8 && p4) p1 = true;
                                }
                                if (_stare.S2)
                                {
                                    if (_stare.S6 && p4) p2 = true;
                                    if (_stare.S8 && p3) p2 = true;
                                }
                            }
                        }
                    }
                }

                _stare.ActualizeazaFunctionareBenzi(p1, p2, p3, p4);

                await _hubContext.Clients.All.SendAsync("PrimesteStareNoua", _stare, stoppingToken);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}