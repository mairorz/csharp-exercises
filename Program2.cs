using System;

namespace Giraffe

{
    class Program2
    {
        public static void Ejecutar()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("Escribe tus datos");
            Console.WriteLine("-----------------");
            Console.Write("Nombre completo: ");
            string nombre = Console.ReadLine();
            Console.Write("Edad: ");
            int edad = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("-----------------");
            Console.WriteLine(nombre);
            Console.WriteLine(nombre.GetType());
            Console.WriteLine();
            Console.WriteLine(edad);
            Console.WriteLine(edad.GetType());
            Console.ReadKey();
        }
    }
}
