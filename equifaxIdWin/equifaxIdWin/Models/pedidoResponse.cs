using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equifaxIdWin.Models
{
    public class pedidoResponse
    {
        public string? Mensaje { get; set; }
        public bool Error { get; set; }
        public modelopedido? Data { get; set; }
    }

    public class modelopedido {
        public string? numeroxOperacion{ get; set; }
        public string? contenido_pregunta1 { get; set; }
        public string? categoria_pregunta1 { get; set; }
        public string? numero_pregunta1 {get;set;}
        public string? numero_Opcionp11 {get;set;}
        public string? desc_opcion11 {get;set;}
        public string? numero_Opcionp12 {get;set;}
        public string? desc_opcion12 {get;set;}
        public string? numero_Opcionp13 {get;set;}
        public string? desc_opcion13 {get;set;}
        public string? contenido_pregunta2 { get; set; }
        public string? categoria_pregunta2 { get; set; }
        public string? numero_pregunta2 {get;set;}
        public string? numero_Opcionp21 {get;set;}
        public string? desc_opcion21 {get;set;}
        public string? numero_Opcionp22 {get;set;}
        public string? desc_opcion22 {get;set;}
        public string? numero_Opcionp23 {get;set;}
        public string? desc_opcion23 {get;set;}
        public string? contenido_pregunta3 { get; set; }
        public string? categoria_pregunta3 { get; set; }
        public string? numero_pregunta3 {get;set;}
        public string? numero_Opcionp31 {get;set;}
        public string? desc_opcion31 {get;set;}
        public string? numero_Opcionp32 {get;set;}
        public string? desc_opcion32 {get;set;}
        public string? numero_Opcionp33 {get;set;}
        public string? desc_opcion33 {get;set;}
    }

    public class respuestapreguntaResponse
    {
         public string? Mensaje { get; set; }
        public bool Error { get; set; }
        public modeloRespuestas? Data { get; set; } 
    }

    public class modeloRespuestas {
        public string? numeroOperacionRes {get;set;}
        public string? resultadoOperacion {get;set;}
        public string? totalDesaporbado {get;set;}
        public string? intentosFallidos {get;set;}
    }

}
