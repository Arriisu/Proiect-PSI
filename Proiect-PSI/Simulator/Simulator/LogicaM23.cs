namespace Simulator
{
    public class LogicaM23 : BackgroundService
    {
        private readonly StareSistem _stare;

        public LogicaM23(StareSistem stare) => _stare = stare;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Citim butoanele și calculăm starea motoarelor P
                bool p1 = false, p2 = false, p3 = false, p4 = false;

                // Regula 1: Benzile de ieșire (3 și 4)
                p3 = _stare.S3 && !_stare.S0;
                p4 = _stare.S4 && !_stare.S0;

                // Regula 2: Benzile de intrare (1 și 2) - funcționează doar dacă ieșirea e ON
                // și în funcție de clapeta (S6 = Stânga/Banda 3, S8 = Dreapta/Banda 4)
                if (!_stare.S0 && !_stare.S5)
                {
                    // Banda 1 merge spre 3 sau 4
                    if (_stare.S1 && ((_stare.S6 && p3) || (_stare.S8 && p4))) p1 = true;
                    
                    // Banda 2 merge spre 3 sau 4 (și verificăm să nu meargă ambele de intrare simultan)
                    if (_stare.S2 && !p1 && ((_stare.S6 && p3) || (_stare.S8 && p4))) p2 = true;
                }

                // Trimitem starea calculată înapoi în "Memorie"
                _stare.ActualizeazaFunctionareBenzi(p1, p2, p3, p4);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}