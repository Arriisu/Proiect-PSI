using Microsoft.AspNetCore.Mvc;

namespace Simulator
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerSenzori : ControllerBase
    {
        private readonly StareSistem _stare;

        public ControllerSenzori(StareSistem stare)
        {
            _stare = stare;
        }

        // Interfața apelează asta ca să vadă starea tuturor senzorilor
        [HttpGet]
        public ActionResult<StareSistem> GetStare()
        {
            return _stare;
        }

        // Interfața apelează asta ca să "simuleze" activarea unui senzor (ex: apasă S1)
        [HttpPost("activare/{numeSenzor}")]
        public IActionResult ActivareSenzor(string numeSenzor)
        {
            if (numeSenzor == "S1") 
            {
                _stare.ApasaStartBanda(1); 
            }
            else if (numeSenzor == "S0") 
            {
                _stare.ApasaStopGeneralS0();
            }

            return Ok($"Comanda pentru {numeSenzor} a fost procesată.");
        }
    }
}