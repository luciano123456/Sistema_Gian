using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
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
    public class ProductosPrecioHistorialRepository : IProductosPrecioHistorialRepository<ProductosPreciosHistorial>
    {

        private readonly SistemaGianContext _dbcontext;
        

        public ProductosPrecioHistorialRepository(SistemaGianContext context)
        {
            _dbcontext = context;
            
        }

        public async Task<bool> Insertar(ProductosPreciosHistorial model)
        {
            try
            {
                _dbcontext.ProductosPreciosHistorial.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            } catch {  return false; }
        }

        public async Task<bool> Actualizar(ProductosPreciosHistorial model)
        {
            try
            {
                _dbcontext.ProductosPreciosHistorial.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            } catch {  return false; }
        }

        public async Task<ProductosPreciosHistorial> Obtener(int idProducto, int idProveedor, int idCliente)
        {
                ProductosPreciosHistorial result = await _dbcontext.ProductosPreciosHistorial
                    .Where(x => x.IdProducto == idProducto && (x.IdCliente == idCliente || idCliente == -1) && (x.IdProveedor == idProveedor || idProveedor == -1))
                    .Include(p => p.IdProductoNavigation)
                    .Include(p => p.IdClienteNavigation)
                    .Include(p => p.IdProveedorNavigation)
                    .OrderBy(x => x.Id)
                    .LastOrDefaultAsync()
                    ;
                return result;
        }


        public async Task<ProductosPreciosHistorial> ObtenerFecha(int idProducto, int idProveedor, int idCliente, DateTime Fecha)
        {
            ProductosPreciosHistorial result = await _dbcontext.ProductosPreciosHistorial
                .Where(x => x.IdProducto == idProducto && (x.IdCliente == idCliente || idCliente == -1) && (x.IdProveedor == idProveedor || idProveedor == -1) && x.Fecha.Date == Fecha.Date)
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdProveedorNavigation)
                .OrderBy(x => x.Id)
                .LastOrDefaultAsync();

            return result;
        }

    }
}
