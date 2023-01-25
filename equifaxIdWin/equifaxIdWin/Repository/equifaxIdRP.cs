using Microsoft.AspNetCore.Mvc;
using equifaxIdWin.Models;
using Microsoft.EntityFrameworkCore;
using equifaxIdWin.Context;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using equifaxIdWin.Services;
using Microsoft.IdentityModel.Tokens;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceReference1;
using System.Collections.Generic;

namespace equifaxIdWin.Repository
{
    public class equifaxIdRP : IequifaxId
    {
        private readonly awsEquifaxContext _aws;

        public equifaxIdRP(awsEquifaxContext aws)
        {
            _aws = aws;
        }

       

        public async Task<pedidoResponse> getDni(string numdoc)
        {
            pedidoResponse response = new pedidoResponse();
            try
            {
                var cli = _aws.t_equifax.FirstOrDefaultAsync(x => x.dni == numdoc | x.ruc ==numdoc | x.ce==numdoc);
                
                if (cli.Result == null)
                {
                    response.Mensaje = "El usuario no se encuentra registrado en equifax";
                    response.Error = false;
                    response.Data = null;
                    
                }else
                {
                    response.Mensaje = "El usuario esta registrado en EQUIFAX";
                    response.Error = false;
                    response.Data = null;
                }
                return response;
                
            }
            catch(Exception e)
            {
                response.Mensaje = e.Message;
                response.Error = true;
                response.Data = null;
                return response;
            }
            
        }

        public async Task<pedidoResponse> ConsultarUser( int td, string numdoc)
        {
            pedidoResponse response = new pedidoResponse();
            string str2;
            int first, last;
            string[] arr;
            char[] mychar = { '>', '<', '/' };
            

            try
            {
                var cli = _aws.t_equifax.FirstOrDefaultAsync(x => x.dni == numdoc | x.ruc == numdoc | x.ce == numdoc);


                if (cli.Result == null)
                {
                    response.Mensaje = "El usuario no está en EQUIFAX";
                    response.Error = false;
                    response.Data = null;
                    return response;
                }
                if (cli.Result.dni != null)
                {
                   
                    td = 1;
                }
                if (cli.Result.ruc != null)
                {
                    
                    td = 5;
                }
                if (cli.Result.ce != null)
                {
                    
                    td = 3;
                }

                WSIdValidatorEndpointClient endpoint = new WSIdValidatorEndpointClient();
                EstructuraListadoPreguntas listado = new EstructuraListadoPreguntas();
                listado.candidato = new Candidato()
                {
                    nombres = "",
                    apellidoMaterno = "",
                    apellidoPaterno = "",
                    codigoInterno = "0",
                    codigoInterno1 = "0",
                    codigoInterno2 = "0",
                    inicioValidacion = "E",
                    numeroDocumento = numdoc,
                    tipoDocumento = td,
                    digitoVerificador = "",
                };
                listado.header = new Header()
                {
                    canal = "AIX6",
                    clave = "hNY8ZTvXXh6SwF7JbsbEvMcbGPAnvE56",
                    modelo = "00103903",
                    usuario = "WSUATIDVALWIN"
                };
                listadoPreguntasResponse res = await endpoint.listadoPreguntasAsync(listado);
                string stringXml = res.result.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(stringXml);
                string json = JsonConvert.SerializeXmlNode(doc);
                bool validator = stringXml.Contains("<NumeroOperacion>");
                // explicacion
                bool validador_final = stringXml.Contains("<explicacion>");
                if(validator && validador_final ==false){
                    first = stringXml.IndexOf("numeroOperacion") + "numeroOperacion".Length;
                    last = stringXml.LastIndexOf("numeroOperacion");
                    str2 = stringXml.Substring(first, last - first);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Mensaje = "Sin errores";
                    response.Error = false;
                    response.Data.numeroxOperacion = str2;
                    str2="";
                    var descpreini = stringXml.IndexOf("<descripcionPregunta>") + "<descripcionPregunta>".Length;
                    var descprefin = stringXml.IndexOf("</descripcionPregunta>") + "</descripcionPregunta>".Length;
                    str2 = stringXml.Substring(descpreini, descprefin- descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.contenido_pregunta1=str2;
                    var nuevoxml = stringXml.Substring(descprefin);
                    
                    // Pregunta 1 y sus alternativas
                    
                    descpreini = nuevoxml.IndexOf("<categoriaPregunta>") + "<categoriaPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</categoriaPregunta>") + "</categoriaPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.categoria_pregunta1=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<numeroPregunta>") + "<numeroPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroPregunta>") + "</numeroPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_pregunta1=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp11 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion11 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp12 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion12 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp13 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion13 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);

                    // Pregunta 2 y sus alternativas
                    descpreini = nuevoxml.IndexOf("<categoriaPregunta>") + "<categoriaPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</categoriaPregunta>") + "</categoriaPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.categoria_pregunta2=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<numeroPregunta>") + "<numeroPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroPregunta>") + "</numeroPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_pregunta2=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp21 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion21 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp22 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion22 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp23 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion23 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    
                    // Pregunta 3 y sus alternativas

                    descpreini = nuevoxml.IndexOf("<categoriaPregunta>") + "<categoriaPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</categoriaPregunta>") + "</categoriaPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.categoria_pregunta3=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<numeroPregunta>") + "<numeroPregunta>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroPregunta>") + "</numeroPregunta>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_pregunta3=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp31 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion31 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp32 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion32 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                      descpreini = nuevoxml.IndexOf("<numeroOpcion>") + "<numeroOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</numeroOpcion>") + "</numeroOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                     str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.numero_Opcionp33 = str2;
                    nuevoxml = nuevoxml.Substring(descprefin);
                    descpreini = nuevoxml.IndexOf("<descripcionOpcion>") + "<descripcionOpcion>".Length;
                    descprefin = nuevoxml.IndexOf("</descripcionOpcion>") + "</descripcionOpcion>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.desc_opcion33 = str2;


                }
                if(validator==false && validador_final ==false){
                    response.Mensaje = "Se ingresó un valor incorrecto";
                    response.Error = true;
                    response.Data = null;
                    
                }

                if(validador_final){
                    response.Mensaje = "Número de intentos máximos alcanzado, vuelva a intentarlo en 24 horas";
                    response.Error = true;
                    response.Data = null;
                }
                
                return response;


            }
            catch (Exception e)
            {
                response.Mensaje = e.Message;
                response.Error = true;
                response.Data = null;
                return response;
            }

        }

        public async Task<respuestapreguntaResponse> resultadoEvaluacion(string cp1, string np1, string no1, string cp2, string np2, string no2, string cp3, string np3, string no3, string nopera)
        {
            respuestapreguntaResponse response = new respuestapreguntaResponse();
            string str2,str2_error;
            int first, firsterror,last, lasterror;
            string[] arr;
             char[] mychar = { '>', '<', '/' };
            try
            {
                  WSIdValidatorEndpointClient endpoint = new WSIdValidatorEndpointClient();
                //EstructuraListadoPreguntas
                EstructuraValidacionPreguntas listado = new EstructuraValidacionPreguntas();
                
                var datospres = new List<DatoPregunta>();
                
                // Numero de pregunta y respuesta enviado por el usuario 1
                DatoPregunta pregunta_1 = new DatoPregunta() {
                    categoriaPregunta = cp1,
                    numeroOpcion = np1,
                    numeroPregunta = no1,
                };

                // Numero de pregunta y respuesta enviado por el usuario 2
                DatoPregunta pregunta_2 = new DatoPregunta() {
                    categoriaPregunta = cp2,
                    numeroOpcion = np2,
                    numeroPregunta = no2,
                };
                
                // Numero de pregunta y respuesta enviado por el usuario 3
                DatoPregunta pregunta_3 = new DatoPregunta() {
                    categoriaPregunta = cp3,
                    numeroOpcion = np3,
                    numeroPregunta = no3,
                };

                datospres.Add(pregunta_1);
                datospres.Add(pregunta_2);
                datospres.Add(pregunta_3);
                
                // Arreglo Header con valores de la conexion al sistema Win
                listado.header = new Header()
                {
                    canal = "AIX6",
                    clave = "hNY8ZTvXXh6SwF7JbsbEvMcbGPAnvE56",
                    modelo = "00103903",
                    usuario = "WSUATIDVALWIN"
                };
                // Numero de operacion  enviado por el usuario
                listado.numeroOperacion = nopera;

                //Respuesta del servidor equifax a la solicitud
                validacionPreguntasResponse res = await endpoint.validacionPreguntasAsync(listado);
                
                string stringXml = res.result.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(stringXml);
                string json = JsonConvert.SerializeXmlNode(doc);

                bool validator = stringXml.Contains("<NumeroOperacion>");
  
                 if(validator){
                    first = stringXml.IndexOf("numeroOperacion") + "numeroOperacion".Length;
                    last = stringXml.LastIndexOf("numeroOperacion");
                    str2 = stringXml.Substring(first, last - first);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Mensaje = "Sin errores";
                    response.Error = false;
                    response.Data.numeroOperacionRes = str2;

                    // RESPUESTA EQUIFAX ID

                    str2="";
                    var descpreini = stringXml.IndexOf("<resultadoEvaluacion>") + "<resultadoEvaluacion>".Length;
                    var descprefin = stringXml.IndexOf("</resultadoEvaluacion>") + "</resultadoEvaluacion>".Length;
                    str2 = stringXml.Substring(descpreini, descprefin- descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.resultadoOperacion=str2;
                    var nuevoxml = stringXml.Substring(descprefin);

                    descpreini = nuevoxml.IndexOf("<totalDesaprobados>") + "<totalDesaprobados>".Length;
                    descprefin = nuevoxml.IndexOf("</totalDesaprobados>") + "</totalDesaprobados>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.totalDesaporbado=str2;
                    nuevoxml = nuevoxml.Substring(descprefin);

                    descpreini = nuevoxml.IndexOf("<intentosFallidos>") + "<intentosFallidos>".Length;
                    descprefin = nuevoxml.IndexOf("</intentosFallidos>") + "</intentosFallidos>".Length;
                    str2 = nuevoxml.Substring(descpreini, descprefin - descpreini);
                    str2 = str2.TrimStart(mychar);
                    str2 = str2.TrimEnd(mychar);
                    str2 = str2.Trim();
                    response.Data.intentosFallidos=str2;
                 }
                 else{
                    response.Mensaje = "Se ingresó un valor incorrecto, debe reiniciar intento";
                    response.Error = true;
                    response.Data = null;
                 }
                
                
                //response.Data = str2;
                return response;
            }
            catch(Exception e)
            {
                response.Mensaje = e.Message;
                response.Error = true;
                response.Data = null;
                return response;
            }
        }
    }
}
