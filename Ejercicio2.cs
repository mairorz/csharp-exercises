using System;

namespace Giraffe

{
    class Ejercicio2
    {
        public static void Ejecutar()
        {
            //Tabla de multiplicacion

            int n = 10;

            Console.Write("Escriba la tabla que quiere ver: ");
            int tabla = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine(tabla + " * " + i + " = " + (tabla * i));
            }

            Console.ReadKey();
        }
    }
}
