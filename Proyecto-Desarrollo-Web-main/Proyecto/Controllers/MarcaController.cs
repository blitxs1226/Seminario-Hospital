﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.permisos;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class MarcaController : Controller
    {
        //recibir una lista de una api 
        private readonly string _url = "https://localhost:7252/api/Marcas";
        public async Task<ActionResult> Index()

        {
            using (var http =new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoMarcas = JsonConvert.DeserializeObject<List<TblMarca>>(responseString);
                return View(listadoMarcas);
            }


               
        }
        public ActionResult newMarcas()
        {
            return View();
        }
        //agregar a el json
        [HttpPost]
        //siempre debe ser un model
        public async Task<ActionResult> agregarMarca(TblMarca model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var marcaSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(marcaSerializada, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_url,content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }
               
        }


        public async Task<ActionResult> modificarMarca(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString =await  response.Content.ReadAsStringAsync();
                var marca = JsonConvert.DeserializeObject<TblMarca>(responseString);
                return View(marca);
            }
            
        }

        //modifica los datos de la bd
        [HttpPost]
        public async Task<ActionResult> modificarMarca(TblMarca model)
        {
            using (var http = new HttpClient())
            {
                var marcaSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(marcaSerializada, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(_url + "/" + model.IdMarca, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }
        //elimina los datos de la bd

        public async Task<string> eliminarMarca(int id)
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

