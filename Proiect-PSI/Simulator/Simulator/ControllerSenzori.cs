using Microsoft.AspNetCore.Mvc;

namespace Simulator
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerSenzori : ControllerBase
    {
        private readonly StareSistem _stare;

        public ControllerSenzori(StareSistem stare) => _stare = stare;

        [HttpPost("activare/{numeSenzor}")]
        public IActionResult ActivareSenzor(string numeSenzor)
        {
            switch (numeSenzor)
            {
                case "S1": _stare.ApasaStartBanda(1); break;
                case "S2": _stare.ApasaStartBanda(2); break;
                case "S3": _stare.ApasaStartBanda(3); break;
                case "S4": _stare.ApasaStartBanda(4); break;
                case "S5": _stare.ApasaStopBenzileIntrareS5(); break;
                case "S6": _stare.SetPozitieClapeta(true, false, false); break;
                case "S7": _stare.SetPozitieClapeta(false, true, false); break;
                case "S8": _stare.SetPozitieClapeta(false, false, true); break;
                case "MODE_1": _stare.SeteazaMod(1); break;
                case "MODE_2": _stare.SeteazaMod(2); break;
                case "ALARM": _stare.DeclanșeazăSimulareAlarmă(); break;
                case "S0": 
                    if (_stare.S0) _stare.ElibereazaStopGeneralS0();
                    else _stare.ApasaStopGeneralS0(); 
                    break;
                case "RESET": 
                    _stare.ResetSistem(); 
                    _stare.ElibereazaStopGeneralS0();
                    break;
            }
            return Ok();
        }
    }
}