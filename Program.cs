static int LeerEntero(string mensaje, int min, int max)
{
    int valor;

    while (true)
    {
        Console.Write(mensaje);

        if (int.TryParse(Console.ReadLine(), out valor))
        {
            if (valor >= min && valor <= max)
            {
                return valor;
            }
        }

        Console.WriteLine($"Ingrese un numero entero entre {min} y {max}");
    }
}

static decimal LeerDecimal(string mensaje, decimal min)
{
    decimal valor;

    while (true)
    {
        Console.Write(mensaje);

        if (decimal.TryParse(Console.ReadLine(), out valor))
        {
            if (valor >= min)
            {
                return valor;
            }
        }

        Console.WriteLine($"Ingrese un numero mayor o igual a {min}");
    }
}

static decimal CalcularFactura(decimal precio, int cantidad, bool tieneDescuento, out decimal iva, out decimal descuento)
{
    decimal subtotal = precio * cantidad;

    if (tieneDescuento)
    {
        descuento = subtotal * 0.10m;
    }
    else
    {
        descuento = 0;
    }

    iva = (subtotal - descuento) * 0.19m;

    decimal total = subtotal - descuento + iva;

    return total;
}

static void ImprimirEncabezado(string titulo)
{
    Console.Clear();

    Console.WriteLine("--------------------------------------------------------------");
    Console.WriteLine($"              {titulo}");
    Console.WriteLine("--------------------------------------------------------------");
}

int opcion = 0;
List<string> nombreProducto = new List<string>();
List<decimal> precioProducto = new List<decimal>();
List<int> stockInicial = new List<int>();
List<int> unidadesVendidas = new List<int>();
int ventas = 0;
decimal totalAcumulado = 0;

do
{
    ImprimirEncabezado("Sistema Gestor de Ventas e Inventario");
    Console.WriteLine("1. Registrar producto nuevo en invetario");
    Console.WriteLine("2. Consultar inventario");
    Console.WriteLine("3. Registrar venta de productos");
    Console.WriteLine("4. Ver reporte y estadistica diaria");
    Console.WriteLine("5. Salir del sistema");
    Console.WriteLine("-----------------------------------------------------------------");

    opcion = LeerEntero("Ingrese una opcion: ", 1, 5);
    
    switch (opcion)
    {
        case 1:
            ImprimirEncabezado("Registrar Producto Nuevo");

            while (true)
            {
                Console.Write("\nIngrese nombre del producto: ");
                string? nombre = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("El nombre no puede estar vacio");
                    continue;
                }      

                if (nombreProducto.Contains(nombre.ToLower().Trim()))
                {
                    Console.WriteLine("Ese producto ya se registro. Intente con otro nombre.");
                    continue;
                }

                nombreProducto.Add(nombre.ToLower().Trim());
                break;
            }
            
            decimal precio = LeerDecimal("\nIngrese precio del producto: ", 0.01m);

            precioProducto.Add(precio);

            int stock = LeerEntero("\nIngrese la cantidad del producto: ", 0, int.MaxValue);

            stockInicial.Add(stock);
            unidadesVendidas.Add(0);
            break;

        case 2:
            ImprimirEncabezado("Lista de Productos");
            if (nombreProducto.Count == 0)
            {
                Console.WriteLine("No hay productos actualmente, debe de registrar primero");
            }
            else
            {
                for (int i = 0; i < nombreProducto.Count; i++)
                {
                    string nombre = nombreProducto[i];
                    nombre = char.ToUpper(nombre[0]) + nombre.Substring(1);

                    if (stockInicial[i] >= 5)
                    {
                        Console.WriteLine($"{i + 1}. {nombre} | Precio: {precioProducto[i]:C} | Stock: {stockInicial[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {nombre} | Precio: {precioProducto[i]:C} | Stock: {stockInicial[i]} [Bajo Stock]");
                    }
                }
            }
            Console.WriteLine("--------------------------------------------------------------");
            Console.ReadKey();
            break;

        case 3:
            ImprimirEncabezado("Registrar Venta");
            if (nombreProducto.Count == 0)
            {
                Console.WriteLine("No hay productos actualmente, debe de registrar primero");
            }
            else
            {
                for (int i = 0; i < nombreProducto.Count; i++)
                {
                    string nombre = nombreProducto[i];
                    nombre = char.ToUpper(nombre[0]) + nombre.Substring(1);

                    if (stockInicial[i] >= 5)
                    {
                        Console.WriteLine($"{i + 1}. {nombre} | Precio: {precioProducto[i]:C} | Stock: {stockInicial[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {nombre} | Precio: {precioProducto[i]:C} | Stock: {stockInicial[i]} [Bajo Stock]");
                    }
                }

                int numProd = LeerEntero("\nIngrese el número del producto que desea vender: ", 1, nombreProducto.Count);
                numProd--;

                if (stockInicial[numProd] == 0)
                {
                    Console.WriteLine("Este producto no tiene stock disponible.");
                    Console.ReadKey();
                    break;
                }

                int cantidad = LeerEntero("\nIngrese la cantidad que desea vender: ", 1, stockInicial[numProd]);
                stockInicial[numProd] -= cantidad;
                unidadesVendidas[numProd] += cantidad;

                bool descuentoCF = false;
                while (true)
                {
                    Console.Write("\nAplicar descuento de cliente frecuente (10%)? (SI/NO): ");
                    string? clienteFrecuente = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(clienteFrecuente))
                    {
                        Console.WriteLine("Debe ingresar SI o NO");
                        continue;
                    }      

                    if (clienteFrecuente.ToUpper().Trim() == "SI")
                    {
                        Console.WriteLine("Se aplicara descuento de cliente frecuente");
                        descuentoCF = true;
                    }
                    else if (clienteFrecuente.ToUpper().Trim() == "NO")
                    {
                        Console.WriteLine("No aplicara descuento de cliente frecuente");
                    }
                    else
                    {
                        Console.WriteLine("Debe ingresar SI o NO");
                        continue;
                    }
                    break;
                }

                decimal iva = 0;
                decimal descuento = 0;

                decimal total = CalcularFactura(precioProducto[numProd], cantidad, descuentoCF, out iva, out descuento);
                totalAcumulado += total;

                decimal subtotal = precioProducto[numProd] * cantidad;

                ImprimirEncabezado("Venta");
                Console.WriteLine($"Producto:           {nombreProducto[numProd]} (x{cantidad})");
                Console.WriteLine($"Subtotal:           {subtotal:C}");
                Console.WriteLine($"Descuento          -{descuento:C}");
                Console.WriteLine($"Monto IVA (19%):   +{iva:C}");
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine($"Total a pagar:      {total:C}");
                Console.WriteLine("--------------------------------------------------------------");

                Console.WriteLine($"\nVenta Realizada, el stock se a actualizado: {stockInicial[numProd]} unidades");
                ventas++;
                Console.ReadKey();
            }
            break;

        case 4:
            decimal promedioVentas = 0;

            if (ventas > 0)
            {
                promedioVentas = totalAcumulado / ventas;
            }

            Console.WriteLine("Estadisticas Diarias");
            Console.WriteLine($"Total de ventas realizadas: {ventas}");
            Console.WriteLine($"Total de dinero ingresado: {totalAcumulado:C}");
            Console.WriteLine($"Promedio de dinero por venta: {promedioVentas:C}");

            if (nombreProducto.Count == 0 || ventas == 0)
            {
                Console.WriteLine("Producto mas vendido: Aun no has vendido");
            }
            else
            {
                int masVendido = 0;

                for (int i = 1; i < unidadesVendidas.Count; i++)
                {
                    if (unidadesVendidas[i] > unidadesVendidas[masVendido])
                    {
                        masVendido = i;
                    }
                }

                Console.WriteLine($"Producto mas vendido: {nombreProducto[masVendido]}");
                Console.WriteLine($"Unidades vendidas: {unidadesVendidas[masVendido]}");
            }
            Console.WriteLine("--------------------------------------------------------------");
            break;

        case 5:
            Console.WriteLine("Saliendo del sistema... Adios");
            break;
    }

} while (opcion != 5);