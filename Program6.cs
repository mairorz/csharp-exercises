using System;

namespace ArrayMultidimensional

{
    class Program6
    {
        public static void Ejecutar()
        {
            String[,] estacionamiento =
            {
                {"Carro 1", "Carro 2", "Carro 3"},
                {"Carro 4", "Carro 5", "Carro 6"},
                {"Carro 7", "Carro 8", "Carro 9"}
            };

            for (int i = 0; i < estacionamiento.GetLength(0); i++)
            {
                for (int j = 0; j < estacionamiento.GetLength(1); j++)
                {
                    Console.Write(estacionamiento[i, j] + "");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
