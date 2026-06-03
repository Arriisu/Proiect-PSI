using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Simulator.Hubs;

namespace Simulator
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerSenzori : ControllerBase
    {
        private readonly StareSistem _stare;
        private readonly IHubContext<M23Hub> _hubContext;

        public ControllerSenzori(StareSistem stare, IHubContext<M23Hub> hubContext)
        {
            _stare = stare;
            _hubContext = hubContext;
        }

        // Interfața apelează asta ca să vadă starea tuturor senzorilor
        [HttpGet]
        public ActionResult<StareSistem> GetStare()
        {
            return _stare;
        }

        // Interfața apelează asta ca să "simuleze" activarea unui senzor (ex: apasă S1)
        [HttpPost("activare/{numeSenzor}")]
        public async Task<IActionResult> ActivareSenzor(string numeSenzor)
        {
            if (numeSenzor == "S1") 
            {
                _stare.ApasaStartBanda(1); 
            }
            else if (numeSenzor == "S0") 
            {
                _stare.ApasaStopGeneralS0();
            }

            await _hubContext.Clients.All.SendAsync("PrimesteStareNoua", _stare);

            return Ok($"Comanda pentru {numeSenzor} a fost procesată.");
        }
    }
}