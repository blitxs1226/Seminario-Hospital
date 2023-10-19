﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Models;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly ClinicaMedicaContext _context;

        public EmpleadosController(ClinicaMedicaContext context)
        {
            _context = context;
        }

        // GET: api/Empleado
        [HttpGet]
        public async Task<ActionResult> GetTblEmpleados()
        {
            var listadoEmpleados = _context.TblEmpleados.ToList().Join(_context.TblCargos,
                empleado => empleado.IdCargo,
                cargo => cargo.IdCargo,
                (empleado, cargo) => new
                {
                    IdEmpleado = empleado.IdEmpleado,
                    CodigoEmpleado=empleado.CodigoEmpleado,
                    Nombre = empleado.Nombre,
                    Apellido = empleado.Apellido,
                    Direccion = empleado.Direccion,
                    Telefono = empleado.Telefono,
                    IdCargo = empleado.IdCargo,
                    IdClinica = empleado.IdClinica,
                    Cargo = cargo.Cargo
                }).ToList().Join(_context.TblClinicas,
                empleado => empleado.IdClinica,
                clinica => clinica.IdClinica,
                (empleado, clinica) => new
                {
                    IdEmpleado = empleado.IdEmpleado,
                    CodigoEmpleado = empleado.CodigoEmpleado,
                    Nombre = empleado.Nombre,
                    Apellido = empleado.Apellido,
                    Direccion = empleado.Direccion,
                    Telefono = empleado.Telefono,
                    IdCargo = empleado.IdCargo,
                    IdClinica = empleado.IdClinica,
                    Cargo = empleado.Cargo,
                    Clinica = clinica.Nombre
                }).ToList();
            return Ok(listadoEmpleados);
        }


      
        [HttpGet("{id}")]
        public async Task<ActionResult<TblEmpleado>> GetTblEmpleados(int id)
        {
            var Empleado = await _context.TblEmpleados.FindAsync(id);

            if (Empleado == null)
            {
                return NotFound();
            }

            return Ok(Empleado);
        }


        // PUT: api/Empleado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblEmpleado(int id, TblEmpleado tblEmpleado)
        {
            if (id != tblEmpleado.IdEmpleado)
            {
                return BadRequest("El id no coincide");
            }

            _context.Entry(tblEmpleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblEmpleadoExists(id))
                {
                    return NotFound("No se encontro el empleado");
                }
                else
                {
                    throw;
                }
            }

            return Ok(tblEmpleado);
        }

        // POST: api/Empleado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblEmpleado>> PostTblEmpleado(TblEmpleado tblEmpleado)
        {
            _context.TblEmpleados.Add(tblEmpleado);
            await _context.SaveChangesAsync();

            return Ok(tblEmpleado);
        }

        // DELETE: api/Empleado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblEmpleado(int id)
        {
            var tblEmpleado = await _context.TblEmpleados.FindAsync(id);
            if (tblEmpleado == null)
            {
                return NotFound("No se encontro el empleado");
            }

            _context.TblEmpleados.Remove(tblEmpleado);
            await _context.SaveChangesAsync();

            return Ok("Empleado eliminado");
        }

        private bool TblEmpleadoExists(int id)
        {
            return _context.TblEmpleados.Any(e => e.IdEmpleado == id);
        }
    }
}
