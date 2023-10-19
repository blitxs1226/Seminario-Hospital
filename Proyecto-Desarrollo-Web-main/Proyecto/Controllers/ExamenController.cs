﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.permisos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class ExamenController : Controller
    {
        //recibir una lista de una api 

        private readonly string _url = "https://localhost:7252/api/Examenes";
        private readonly string _urlExamenConsulta = "https://localhost:7252/api/ExamenesConsultas";
        public async Task<ActionResult> Index()

        {


            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var listadoExamenes = JsonConvert.DeserializeObject<List<TblExamene>>(responseString);

                return View(listadoExamenes);
            }



        }
        public ActionResult newExamen()
        {
            return View();
        }
        //agregar a el json
        [HttpPost]
        //siempre debe ser un model

        public async Task<ActionResult> agregarExamen(TblExamene model)

        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var examenSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(examenSerializada, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }


        

        public async Task<ActionResult> modificarExamen(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var examen = JsonConvert.DeserializeObject<TblExamene>(responseString);

                return View(examen);
            }

        }

        //modifica los datos de la bd
        [HttpPost]

        public async Task<ActionResult> modificarExamen(TblExamene model)

        {
            using (var http = new HttpClient())
            {
                var examenSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(examenSerializada, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(_url + "/" + model.IdExamen, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }


        public async Task<string> eliminarExamen(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.DeleteAsync(_url + "/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    return "Error";
                }
                return "Exito";
            }
        }


        [HttpPost]
        public async Task<JsonResult> agregarExamenConsulta(TblExamenesConsulta model)

        {
            if (!ModelState.IsValid)
            {
                return Json(null);
            }
            using (var http = new HttpClient())
            {
                var examenSerializado = JsonConvert.SerializeObject(model);
                var content = new StringContent(examenSerializado, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_urlExamenConsulta, content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(null);
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var examen = JsonConvert.DeserializeObject<List<ExamenConsultaViewModel>>(responseString);
                return Json(examen);
            }

        }
    }
}
