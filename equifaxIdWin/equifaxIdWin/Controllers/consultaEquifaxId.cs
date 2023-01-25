using Microsoft.AspNetCore.Mvc;
using equifaxIdWin.Models;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using equifaxIdWin.Services;
using equifaxIdWin.Repository;
using equifaxIdWin.Context;

namespace equifaxIdWin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class consultaEquifaxId : ControllerBase
    {
        private readonly IequifaxId _equifax;
        public consultaEquifaxId(IequifaxId equifax)
        {
            _equifax = equifax;
        }

        [HttpGet("getDniusuario")]
        public async Task<IActionResult> getDniusuario(string numerodocumento)
        {
            pedidoResponse response = new pedidoResponse();
            try
            {
                response = await _equifax.getDni(numerodocumento);
                if (!response.Error)
                {
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
            }
            catch(Exception ex)
            {
                response.Error = true;
                response.Mensaje = ex.Message;
                response.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("consultarxpregunta")]
        public async Task<IActionResult> consultarxpregunta( int td, string numdoc)
        {
            pedidoResponse response = new pedidoResponse();
            try
            {
                response = await _equifax.ConsultarUser(td,numdoc);
                if (!response.Error)
                {
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Mensaje = ex.Message;
                response.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("consultaxrespuestas")]
        public async Task<IActionResult> consultaxrespuestas(string cp1, string np1, string no1, string cp2, string np2, string no2, string cp3, string np3, string no3, string nopera)
        {
            respuestapreguntaResponse response = new respuestapreguntaResponse();
            try
            {
                response = await _equifax.resultadoEvaluacion(cp1, np1, no1, cp2, np2, no2, cp3, np3, no3, nopera);
                if (!response.Error)
                {
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Mensaje = ex.Message;
                response.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
