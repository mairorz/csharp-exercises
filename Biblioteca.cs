using System;

namespace BibliotecaBasica
{
    class Biblioteca
    {
        const int MAX = 100;
        static Libro[] libros = new Libro[MAX];
        static int total = 0;
        static int siguienteId = 1;

        public static void Ejecutar()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BIBLIOTECA ===");
                Console.WriteLine("1) Agregar libro");
                Console.WriteLine("2) Listar libros");
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
                Pausa("Biblioteca llena.");
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Agregar libro ===");
            Console.Write("Título: ");
            string titulo = Console.ReadLine() ?? "";
            Console.Write("Autor: ");
            string autor = Console.ReadLine() ?? "";
            Console.Write("ISBN (opcional): ");
            string isbn = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(titulo))
            {
                Pausa("El título es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(autor))
            {
                Pausa("El autor es obligatorio.");
                return;
            }

            try
            {
                Libro l = new Libro();
                l.Id = siguienteId++;
                l.Titulo = titulo;
                l.Autor = autor;
                l.ISBN = (isbn == "") ? "" : isbn;

                libros[total] = l;
                total++;

                Pausa("Libro agregado.");
            }
            catch (Exception ex)
            {
                Pausa("No se pudo agregar: " + ex.Message);
            }
        }

        static void Listar()
        {
            Console.Clear();
            Console.WriteLine("=== Lista de libros ===");

            if (total == 0)
            {
                Pausa("No hay libros.");
                return;
            }

            Console.WriteLine("ID  | Título                 | Autor                 | ISBN");
            Console.WriteLine("----+------------------------+-----------------------+---------------------------");

            for (int i = 0; i < total; i++)
            {
                Libro l = libros[i];
                Console.WriteLine($"{l.Id,-3} | {l.Titulo,-22} | {l.Autor,-21} | {((l.ISBN == "") ? "-" : l.ISBN)}");
            }

            Pausa();
        }

        static void Buscar()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar libro ===");
            Console.WriteLine("Puedes buscar por título, autor o ISBN.");
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
                Libro l = libros[i];
                if (l.Titulo == term || l.Autor == term || l.ISBN == term)
                {
                    Console.WriteLine($"{l.Id}. {l.Titulo} - {l.Autor} - {((l.ISBN == "") ? "-" : l.ISBN)}");
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
                Pausa("No hay libros para editar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Editar libro ===");
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
                Libro l = libros[i];
                if (l.Id == id)
                {
                    encontrado = true;
                    Console.WriteLine("Deja vacío para mantener el valor actual.");
                    Console.Write($"Título ({l.Titulo}): ");
                    string t = Console.ReadLine() ?? "";
                    Console.Write($"Autor ({l.Autor}): ");
                    string a = Console.ReadLine() ?? "";
                    Console.Write($"ISBN ({((l.ISBN == "") ? "-" : l.ISBN)}): ");
                    string s = Console.ReadLine() ?? "";

                    l.Titulo = (t == "") ? l.Titulo : t;
                    l.Autor = (a == "") ? l.Autor : a;
                    l.ISBN  = (s == "") ? l.ISBN  : s;

                    libros[i] = l;
                    Pausa("Libro actualizado.");
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
                Pausa("No hay libros para eliminar.");
                return;
            }

            Console.Clear();
            Listar();
            Console.WriteLine("=== Eliminar libro ===");
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
                if (libros[i].Id == id)
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
                libros[j] = libros[j + 1];
            }
            libros[total - 1] = null;
            total--;

            Pausa("Libro eliminado.");
        }

        static void Pausa(string msg = "Presiona una tecla para continuar...")
        {
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }

    class Libro
    {
        public int Id;
        public string Titulo;
        public string Autor;
        public string ISBN;
    }
}
