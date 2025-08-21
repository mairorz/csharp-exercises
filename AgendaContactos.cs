using System;

namespace AgendaContactosBasica
{
    class AgendaContactos
    {
        const int MAX = 100;
        static Contacto[] contactos = new Contacto[MAX];
        static int total = 0;
        static int siguienteId = 1;

        public static void Ejecutar()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== AGENDA DE CONTACTOS ===");
                Console.WriteLine("1) Agregar");
                Console.WriteLine("2) Listar");
                Console.WriteLine("3) Buscar");
                Console.WriteLine("4) Editar");
                Console.WriteLine("5) Eliminar");
                Console.WriteLine("0) Salir");
                Console.Write("Opción > ");
                string op = Console.ReadLine();

                try
                {
                    switch (op)
                    {
                        case "1":
                            Agregar();
                            break;
                        case "2":
                            Listar();
                            break;
                        case "3":
                            Buscar();
                            break;
                        case "4":
                            Editar();
                            break;
                        case "5":
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
        static void Agregar()
        {
            if (total >= MAX)
            {
                Pausa("Agenda llena.");
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Agregar contacto ===");
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";
            Console.Write("Telefono: ");
            string telefono = Console.ReadLine() ?? "";
            Console.Write("Correo (opcional): ");
            string correo = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Pausa("El nombre es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(telefono))
            {
                Pausa("El teléfono es obligatorio.");
                return;
            }

            try
            {
                Contacto contacto = new Contacto();
                contacto.Id = siguienteId++;
                contacto.Nombre = nombre;
                contacto.Telefono = telefono;
                contacto.Correo = (correo == "") ? "" : correo;

                contactos[total] = contacto;
                total++;

                Pausa("Contacto agregado.");
            }
            catch (Exception ex)
            {
                Pausa("No se pudo agregar el contacto: " + ex.Message);
            }
        }

        static void Listar()
        {
            Console.Clear();
            Console.WriteLine("=== Lista de contactos ===");

            if (total == 0)
            {
                Pausa("No hay contactos.");
                return;
            }

            Console.WriteLine("ID  | Nombre                | Teléfono         | Correo");
            Console.WriteLine("----+-----------------------+------------------+------------------------------");

            for (int i = 0; i < total; i++)
            {
                Contacto c = contactos[i];
                Console.WriteLine($"{c.Id,-3} | {c.Nombre,-23} | {c.Telefono,-16} | {((c.Correo == "") ? "-" : c.Correo)}");
            }

            Pausa();
        }

        static void Buscar()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar contacto ===");
            Console.WriteLine("Puedes buscar por nombre, teléfono o correo.");
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
                Contacto c = contactos[i];
                if ((c.Nombre == term) || (c.Telefono == term) || (c.Correo == term))
                {
                    Console.WriteLine($"{c.Id}. {c.Nombre} - {c.Telefono} - {((c.Correo == "") ? "-" : c.Correo)}");
                    encontrados++;
                }
            }

            if (encontrados == 0)
            {
                Console.WriteLine("Sin resultados.");
            }

            Pausa();
        }

        static void Editar()
        {
            if (total == 0)
            {
                Pausa("No hay contactos para editar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Editar contacto ===");
            Console.Write("ID a editar: ");
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

            bool encontrado = false;

            for (int i = 0; i < total; i++)
            {
                Contacto c = contactos[i];
                if (c.Id == id)
                {
                    encontrado = true;
                    Console.WriteLine("Deja vacío para mantener el valor actual.");
                    Console.Write($"Nombre ({c.Nombre}): ");
                    string n = Console.ReadLine() ?? "";
                    Console.Write($"Teléfono ({c.Telefono}): ");
                    string t = Console.ReadLine() ?? "";
                    Console.Write($"Correo ({((c.Correo == "") ? "-" : c.Correo)}): ");
                    string e = Console.ReadLine() ?? "";

                    c.Nombre = (n == "") ? c.Nombre : n;
                    c.Telefono = (t == "") ? c.Telefono : t;
                    c.Correo = (e == "") ? c.Correo : e;

                    contactos[i] = c;
                    Pausa("Perfil actualizado.");
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
                Pausa("No hay contactos para eliminar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Eliminar contacto ===");
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
                if (contactos[i].Id == id)
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
                contactos[j] = contactos[j + 1];
            }
            contactos[total - 1] = null;
            total--;

            Pausa("Contacto eliminado.");
        }

        static void Pausa(string msg = "Presiona una tecla para continuar...")
        {
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }

    class Contacto
    {
        public int Id;
        public string Nombre;
        public string Telefono;
        public string Correo;
    }
}
