﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGian.Application.Models;
using SistemaGian.Application.Models.ViewModels;
using SistemaGian.BLL.Service;
using SistemaGian.Models;
using System.Diagnostics;

namespace SistemaGian.Application.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _Productoservice;
        private readonly IMarcaService _Marcaservice;
        private readonly ICategoriaService _Categoriaservice;
        private readonly IUnidadDeMedidaService _UnidadDeMedidaService;
        private readonly IProductosPrecioProveedorService _productoprecioProveedorService;
        private readonly IProductosPrecioClienteService _productoPrecioClienteService;
        private readonly IProveedorService _proveedorService;

        public ProductosController(IProductoService Productoservice, IMarcaService marcaservice, ICategoriaService categoriaservice, IUnidadDeMedidaService unidadDeMedidaService, IProductosPrecioProveedorService productoprecioProveedorService, IProductosPrecioClienteService productoPrecioClienteService, IProveedorService proveedorService)
        {
            _Productoservice = Productoservice;
            _Marcaservice = marcaservice;
            _Categoriaservice = categoriaservice;
            _UnidadDeMedidaService = unidadDeMedidaService;
            _productoprecioProveedorService = productoprecioProveedorService;
            _productoPrecioClienteService = productoPrecioClienteService;
            _proveedorService = proveedorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsignarProveedor([FromBody] VMAsignarProveedores modelo)
        {
            try
            {
                var result = await _productoprecioProveedorService.AsignarProveedor(modelo.productos, modelo.idProveedor);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(null);
            }


        }

        [HttpPost]
        public async Task<IActionResult> AumentarPrecios([FromBody] VMAumentoProductos modelo)
        {
            try
            {
                var result = await _Productoservice.AumentarPrecios(modelo.productos, modelo.idCliente, modelo.idProveedor, modelo.porcentajeCosto, modelo.porcentajeVenta);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(null);
            }


        }

        [HttpPost]
        public async Task<IActionResult> BajarPrecios([FromBody] VMAumentoProductos modelo)
        {
            try
            {
                var result = await _Productoservice.BajarPrecios(modelo.productos, modelo.idCliente, modelo.idProveedor, modelo.porcentajeCosto, modelo.porcentajeVenta);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(null);
            }


        }



        [HttpPost]
        public async Task<IActionResult> AsignarCliente([FromBody] VMAsignarClientes modelo)
        {
            try
            {
                var result = await _productoPrecioClienteService.AsignarCliente(modelo.productos, modelo.idCliente, modelo.idProveedor);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(null);
            }


        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var Productos = await _Productoservice.ObtenerTodos();

            var lista = Productos.Select(c => new VMProducto
            {
                Id = c.Id,
                FechaActualizacion = c.FechaActualizacion,
                Descripcion = c.Descripcion,
                Marca = c.IdMarcaNavigation.Nombre,       // Nombre de la Marca
                Categoria = c.IdCategoriaNavigation.Nombre, // Nombre de la Categoria
                UnidadDeMedida = c.IdUnidadDeMedidaNavigation.Nombre, // Nombre de la Unidad de Medida
                Moneda = c.IdMonedaNavigation.Nombre, // Nombre de la Moneda
                PCosto = c.PCosto,
                PVenta = c.PVenta,
                PorcGanancia = c.PorcGanancia,
                Image = c.Image
                
            }).ToList();

            return Ok(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaProductosFiltro(int idCliente, int idProveedor, string producto)
        {
            try
            {
                var Productos = await _Productoservice.ListaProductosFiltro(idCliente, idProveedor, producto);

                var lista = Productos.Select(c => new VMProducto
                {
                    Id = c.Id,
                    FechaActualizacion = c.FechaActualizacion,
                    Descripcion = c.Descripcion,
                    Marca = c.IdMarcaNavigation != null ? c.IdMarcaNavigation.Nombre : string.Empty,
                    Categoria = c.IdCategoriaNavigation != null ? c.IdCategoriaNavigation.Nombre : string.Empty,
                    UnidadDeMedida = c.IdUnidadDeMedidaNavigation != null ? c.IdUnidadDeMedidaNavigation.Nombre : string.Empty,
                    Moneda = c.IdMonedaNavigation != null ? c.IdMonedaNavigation.Nombre : string.Empty,
                    Proveedor = c.IdProveedor > 0 ? _proveedorService.Obtener((int)c.IdProveedor).Result.Nombre : null,
                    PCosto = c.PCosto,
                    PVenta = c.PVenta,
                    PorcGanancia = c.PorcGanancia,
                    Image = c.Image

                }).ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los productos: {ex.Message}");
            }
        }












        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMProducto model)
        {
            var Producto = new Producto
            {
                Id = model.Id,
                FechaActualizacion = model.FechaActualizacion,
                Descripcion = model.Descripcion,
                IdMarca = model.IdMarca,
                IdCategoria = model.IdCategoria,
                IdUnidadDeMedida = model.IdUnidadDeMedida,
                IdMoneda = model.IdMoneda,
                PCosto = model.PCosto,
                PVenta = model.PVenta,
                PorcGanancia = model.PorcGanancia,
                Image = model.Image
            };

            bool respuesta = await _Productoservice.Insertar(Producto);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMProducto model)
        {

            var respuesta = false;

            var Producto = new Producto
            {
                Id = model.Id,
                FechaActualizacion = model.FechaActualizacion,
                Descripcion = model.Descripcion,
                IdMarca = model.IdMarca,
                IdCategoria = model.IdCategoria,
                IdUnidadDeMedida = model.IdUnidadDeMedida,
                IdMoneda = model.IdMoneda,
                PCosto = model.PCosto,
                PVenta = model.PVenta,
                PorcGanancia = model.PorcGanancia,
                Image = model.Image
            };


            if (model.IdProveedor > 0 && model.IdCliente <= 0)
            {
                respuesta = await _productoprecioProveedorService.ActualizarProductoProveedor(Producto, model.IdProveedor);
            }
            else if (model.IdProveedor > 0 && model.IdCliente > 0)
            {
                respuesta = await _productoPrecioClienteService.ActualizarProductoCliente(Producto, model.IdCliente, model.IdProveedor);
            }
            else
            {
                respuesta = await _Productoservice.Actualizar(Producto);
            }
          
            

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id, int idProveedor, int idCliente)
        {

            bool respuesta = false;

            if(idProveedor > 0 && idCliente <= 0)
            {
                respuesta = await _productoprecioProveedorService.Eliminar(id, idProveedor);
            } else if (idProveedor > 0 && idCliente > 0)
            {
                respuesta = await _productoPrecioClienteService.Eliminar(id, idCliente, idProveedor);
            }
            else
            {
                respuesta = await _Productoservice.Eliminar(id);
            }
            

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }



        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id, int idCliente, int idProveedor)
        {
            var Producto = await _Productoservice.Obtener(id, idCliente, idProveedor);

            if (Producto != null)
            {
                return StatusCode(StatusCodes.Status200OK, Producto);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}