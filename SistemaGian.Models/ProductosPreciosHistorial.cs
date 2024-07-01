﻿using System;
using System.Collections.Generic;

namespace SistemaGian.Models;

public partial class ProductosPreciosHistorial
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int IdProducto { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdCliente { get; set; }

    public decimal PCostoAnterior { get; set; }

    public decimal PCostoNuevo { get; set; }

    public decimal PVentaAnterior { get; set; }

    public decimal PVentaNuevo { get; set; }

    public decimal PorcGananciaAnterior { get; set; }

    public decimal PorGananciaNuevo { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}
