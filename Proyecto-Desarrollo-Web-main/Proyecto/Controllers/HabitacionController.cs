﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.permisos;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class HabitacionController : Controller
    {

        private readonly string _url = "https://localhost:7252/api/Habitaciones";
        private readonly string _urlClinica = "https://localhost:7252/api/Clinicas";
        private readonly string _urlHabsDispo= "https://localhost:7252/api/Habitaciones/HabitacionesDisponibles";
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
                var listadoHabitacion = JsonConvert.DeserializeObject<List<HabitacionesViewModel>>(responseString);
                return View(listadoHabitacion);
            }




        }
        public async Task<ActionResult> HabitacionesDisponibles()

        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_urlHabsDispo);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoHabitacion = JsonConvert.DeserializeObject<List<HabitacionesViewModel>>(responseString);
                return View(listadoHabitacion);
            }



        }
        [HttpGet]
        public async Task<ActionResult> GetAllData()
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_urlHabsDispo);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoHabitacion = JsonConvert.DeserializeObject<List<HabitacionesViewModel>>(responseString);
                return PartialView("_DataList", listadoHabitacion);
            }
        }


        public async Task<ActionResult> newHabitaciones()
        {


            using (var http = new HttpClient())
            {

                var response = await http.GetAsync(_urlClinica);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var listadoClinicas = JsonConvert.DeserializeObject<List<TblClinica>>(responseString);

               var listadoClinica = listadoClinicas.ConvertAll(r =>
               {
                   return new SelectListItem()
                   {
                       Text = (r.Nombre+", "+r.Direccion),
                       Value = r.IdClinica.ToString(),

                       Selected = false
                   };
               });
                ViewBag.listadoClinica = listadoClinica;

                return View();
            }        
        }

        [HttpPost]
        public async Task<ActionResult> agregarHabitacion(TblHabitacione model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var habitacionSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(habitacionSerializada, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }


        public async Task<ActionResult> modificarHabitacion(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var habitacion = JsonConvert.DeserializeObject<TblHabitacione>(responseString);

                var responseClinica = await http.GetAsync(_urlClinica);

                if (responseClinica.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseStringClinica = await responseClinica.Content.ReadAsStringAsync();

                var listadoClinicas = JsonConvert.DeserializeObject<List<TblClinica>>(responseStringClinica);

                var listadoClinica = listadoClinicas.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = (r.Nombre + ", " + r.Direccion),
                        Value = r.IdClinica.ToString(),

                        Selected = false
                    };
                });
                ViewBag.listadoClinica = listadoClinica;
                return View(habitacion);
            }

        }

        //modifica los datos de la bd
        [HttpPost]

        public async Task<ActionResult> modificarHabitacion(TblHabitacione model)

        {
            using (var http = new HttpClient())
            {
                var habitacionSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(habitacionSerializada, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(_url + "/" + model.IdHabitacion, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }

        public async Task<string> eliminarHabitacion(int id)
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
    }
}