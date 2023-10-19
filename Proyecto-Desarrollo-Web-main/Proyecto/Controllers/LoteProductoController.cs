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
    public class LoteProductoController : Controller
    {
        //recibir una lista de una api 

        private readonly string _url = "https://localhost:7252/api/LoteProducto";
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

                var listadoLoteProducto = JsonConvert.DeserializeObject<List<TblLoteProducto>>(responseString);

                return View(listadoLoteProducto);
            }



        }
        public ActionResult newLoteProductos()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> agregarLoteProducto(TblLoteProducto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var LoteProductoSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(LoteProductoSerializada, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }


       

        public async Task<ActionResult> modificarLoteProducto(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var LoteProducto = JsonConvert.DeserializeObject<TblLoteProducto>(responseString);

                return View(LoteProducto);
            }

        }

        //modifica los datos de la bd
        [HttpPost]

        public async Task<ActionResult> modificarLoteProducto(TblLoteProducto model)

        {
            using (var http = new HttpClient())
            {
                var LoteProductoSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(LoteProductoSerializada, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(_url + "/" + model.IdLoteProducto, content);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }
        //elimina los datos de la bd

        public async Task<string> eliminarLoteProducto(int id)
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