using equifaxIdWin.Models;

namespace equifaxIdWin.Services
{
    public interface IequifaxId
    {
        Task<pedidoResponse> getDni(string numdoc);
        Task<pedidoResponse> ConsultarUser(int td, string numdoc);
        Task<respuestapreguntaResponse> resultadoEvaluacion(string cp1, string np1, string no1, string cp2, string np2, string no2, string cp3, string np3, string no3, string nopera);
    }
}
