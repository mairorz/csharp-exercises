using System;

namespace BancoBasico

{
    class Banco
    {
        const int MAX = 100;
        static Cuenta[] cuentas = new Cuenta[MAX];
        static int total = 0;
        static int siguienteId = 1;

        public static void Ejecutar()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BANCO ===");
                Console.WriteLine("1) Crear cuenta");
                Console.WriteLine("2) Listar cuentas");
                Console.WriteLine("3) Buscar");
                Console.WriteLine("4) Depositar");
                Console.WriteLine("5) Retirar");
                Console.WriteLine("6) Eliminar cuenta");
                Console.WriteLine("0) Salir");
                Console.Write("Opción > ");
                string op = Console.ReadLine();

                try
                {
                    switch (op)
                    {
                        case "1":
                            CrearCuenta();
                            break;
                        case "2":
                            Listar();
                            break;
                        case "3":
                            Buscar();
                            break;
                        case "4":
                            Depositar();
                            break;
                        case "5":
                            Retirar();
                            break;
                        case "6":
                            Eliminar();
                            break;
                        case "0":
                            return;
                        default:
                            Pausa("Opción inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Pausa("Ocurrió un error: " + ex.Message);
                }
            }
        }

        static void CrearCuenta()
        {
            if (total >= MAX)
            {
                Pausa("No se pueden crear más cuentas.");
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Crear cuenta ===");
            Console.Write("Titular: ");
            string titular = Console.ReadLine() ?? "";
            Console.Write("Depósito inicial (opcional): ");
            string dep = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(titular))
            {
                Pausa("El titular es obligatorio.");
                return;
            }

            double deposito = 0.0;
            if (!string.IsNullOrWhiteSpace(dep))
            {
                try
                {
                    deposito = Convert.ToDouble(dep);
                    if (deposito < 0)
                    {
                        Pausa("El depósito no puede ser negativo.");
                        return;
                    }
                }
                catch
                {
                    Pausa("Monto inválido.");
                    return;
                }
            }

            try
            {
                Cuenta c = new Cuenta();
                c.Id = siguienteId++;
                c.Titular = titular;
                c.Saldo = deposito;

                cuentas[total] = c;
                total++;

                Pausa("Cuenta creada. ID de cuenta: " + c.Id);
            }
            catch (Exception ex)
            {
                Pausa("No se pudo crear la cuenta: " + ex.Message);
            }
        }

        static void Listar()
        {
            Console.Clear();
            Console.WriteLine("=== Lista de cuentas ===");

            if (total == 0)
            {
                Pausa("No hay cuentas.");
                return;
            }

            Console.WriteLine("ID  | Titular               | Saldo");
            Console.WriteLine("----+-----------------------+----------------");

            for (int i = 0; i < total; i++)
            {
                Cuenta c = cuentas[i];
                Console.WriteLine($"{c.Id,-3} | {c.Titular,-23} | {c.Saldo}");
            }

            Pausa();
        }

        static void Buscar()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar cuenta ===");
            Console.WriteLine("Puedes buscar por titular o por ID (escribe el número).");
            string term = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(term))
            {
                Pausa("Escribe un término de búsqueda.");
                return;
            }

            int encontrados = 0;
            Console.WriteLine();

            for (int i = 0; i < total; i++)
            {
                Cuenta c = cuentas[i];

                bool coincide = false;

                if (c.Titular == term)
                {
                    coincide = true;
                }
                else
                {

                    try
                    {
                        int num = Convert.ToInt32(term);
                        if (c.Id == num) coincide = true;
                    }
                    catch
                    {
                    }
                }

                if (coincide)
                {
                    Console.WriteLine($"{c.Id}. {c.Titular} - Saldo: {c.Saldo}");
                    encontrados++;
                }
            }

            if (encontrados == 0)
            {
                Console.WriteLine("Sin resultados.");
            }

            Pausa();
        }

        static void Depositar()
        {
            if (total == 0)
            {
                Pausa("No hay cuentas para operar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Depositar ===");
            Console.Write("ID de la cuenta: ");
            string sId = Console.ReadLine();
            Console.Write("Monto: ");
            string sMonto = Console.ReadLine();

            int id;
            double monto;

            try { id = Convert.ToInt32(sId); }
            catch { Pausa("El ID debe ser un número."); return; }

            try { monto = Convert.ToDouble(sMonto); }
            catch { Pausa("El monto debe ser numérico."); return; }

            if (monto <= 0)
            {
                Pausa("El monto debe ser mayor a 0.");
                return;
            }

            bool encontrado = false;
            for (int i = 0; i < total; i++)
            {
                if (cuentas[i].Id == id)
                {
                    cuentas[i].Saldo += monto;
                    encontrado = true;
                    Pausa("Depósito realizado. Saldo actual: " + cuentas[i].Saldo);
                    break;
                }
            }

            if (!encontrado)
            {
                Pausa("ID no encontrado.");
            }
        }

        static void Retirar()
        {
            if (total == 0)
            {
                Pausa("No hay cuentas para operar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Retirar ===");
            Console.Write("ID de la cuenta: ");
            string sId = Console.ReadLine();
            Console.Write("Monto: ");
            string sMonto = Console.ReadLine();

            int id;
            double monto;

            try { id = Convert.ToInt32(sId); }
            catch { Pausa("El ID debe ser un número."); return; }

            try { monto = Convert.ToDouble(sMonto); }
            catch { Pausa("El monto debe ser numérico."); return; }

            if (monto <= 0)
            {
                Pausa("El monto debe ser mayor a 0.");
                return;
            }

            bool encontrado = false;
            for (int i = 0; i < total; i++)
            {
                if (cuentas[i].Id == id)
                {
                    if (cuentas[i].Saldo < monto)
                    {
                        Pausa("Fondos insuficientes.");
                        return;
                    }

                    cuentas[i].Saldo -= monto;
                    encontrado = true;
                    Pausa("Retiro realizado. Saldo actual: " + cuentas[i].Saldo);
                    break;
                }
            }

            if (!encontrado)
            {
                Pausa("ID no encontrado.");
            }
        }

        static void Eliminar()
        {
            if (total == 0)
            {
                Pausa("No hay cuentas para eliminar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Eliminar cuenta ===");
            Console.Write("ID a eliminar: ");
            string entrada = Console.ReadLine();

            int id;
            try
            {
                id = Convert.ToInt32(entrada);
            }
            catch
            {
                Pausa("El ID debe ser un número.");
                return;
            }

            int indice = -1;
            for (int i = 0; i < total; i++)
            {
                if (cuentas[i].Id == id)
                {
                    indice = i;
                    break;
                }
            }

            if (indice == -1)
            {
                Pausa("ID no encontrado.");
                return;
            }

            for (int j = indice; j < total - 1; j++)
            {
                cuentas[j] = cuentas[j + 1];
            }
            cuentas[total - 1] = null;
            total--;

            Pausa("Cuenta eliminada.");
        }

        static void Pausa(string msg = "Presiona una tecla para continuar...")
        {
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }

    class Cuenta
    {
        public int Id;
        public string Titular;
        public double Saldo;
    }
}
