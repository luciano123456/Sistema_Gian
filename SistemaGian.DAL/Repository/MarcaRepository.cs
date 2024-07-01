﻿using Microsoft.EntityFrameworkCore;
using SistemaGian.DAL.DataContext;
using SistemaGian.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SistemaGian.DAL.Repository
{
    public class MarcaRepository : IGenericRepository<ProductosMarca>
    {

        private readonly SistemaGianContext _dbcontext;

        public MarcaRepository(SistemaGianContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(ProductosMarca model)
        {
            _dbcontext.ProductosMarcas.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            ProductosMarca model = _dbcontext.ProductosMarcas.First(c => c.Id == id);
            _dbcontext.ProductosMarcas.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(ProductosMarca model)
        {
            _dbcontext.ProductosMarcas.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductosMarca> Obtener(int id)
        {
            ProductosMarca model = await _dbcontext.ProductosMarcas.FindAsync(id);
            return model;
        }





        public async Task<IQueryable<ProductosMarca>> ObtenerTodos()
        {
            IQueryable<ProductosMarca> query = _dbcontext.ProductosMarcas;
            return await Task.FromResult(query);
        }




    }
}
