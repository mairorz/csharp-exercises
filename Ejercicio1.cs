using System;

namespace Giraffe
{
    class Ejercicio1
    {
        public static void Ejecutar()
        {
            int opI = 0;

            while (opI != 2)
            {
                Console.WriteLine("------------------");
                Console.WriteLine("Calculadora básica");
                Console.WriteLine("------------------");
                Console.WriteLine("¿Qué quieres hacer?");
                Console.WriteLine();
                Console.WriteLine("1. Sumar");
                Console.WriteLine("2. Restar");
                Console.WriteLine("3. Multiplicar");
                Console.WriteLine("4. Dividir");
                Console.WriteLine("5. Salir");
                Console.WriteLine();
                Console.Write("> ");
                int op = Convert.ToInt32(Console.ReadLine());

                switch (op)
                {
                    case 1:
                    {
                        int op1 = 0;
                        while (op1 != 2)
                        {
                            Console.WriteLine();
                            Console.WriteLine("¿Cuántos números quieres sumar?");
                            Console.Write(">");
                            int num = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine();
                            
                            if (num <= 0) { Console.WriteLine("Nada que sumar."); break; }

                            int resultado = 0;

                            for (int i = 1; i <= num; i++)
                            {
                                Console.Write($"Ingresa el número {i}: ");
                                int numero = Convert.ToInt32(Console.ReadLine());
                                resultado += numero;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Resultado: " + resultado);
                            Console.WriteLine("¿Quieres seguir sumando? (1. Sí, 2. No)");
                            Console.Write("> ");
                            op1 = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine();
                        break;
                    }

                    case 2:
                    {
                        int op2 = 0;
                        while (op2 != 2)
                        {
                            Console.WriteLine();
                            Console.WriteLine("¿Cuántos números quieres restar?");
                            Console.Write(">");
                            int num = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine();

                            if (num <= 0) { Console.WriteLine("Nada que restar."); break; }

                            Console.Write("Ingresa el número 1: ");
                            int resultado = Convert.ToInt32(Console.ReadLine());

                            for (int i = 2; i <= num; i++)
                            {
                                Console.Write($"Ingresa el número {i}: ");
                                int numero = Convert.ToInt32(Console.ReadLine());
                                resultado -= numero;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Resultado: " + resultado);
                            Console.WriteLine("¿Quieres seguir restando? (1. Sí, 2. No)");
                            Console.Write("> ");
                            op2 = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine();
                        break;
                    }

                    case 3:
                    {
                        int op3 = 0;
                        while (op3 != 2)
                        {
                            Console.WriteLine();
                            Console.WriteLine("¿Cuántos números quieres multiplicar?");
                            Console.Write(">");
                            int num = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine();

                            if (num <= 0) { Console.WriteLine("Nada que multiplicar."); break; }

                            Console.Write("Ingresa el número 1: ");
                            int resultado = Convert.ToInt32(Console.ReadLine());

                            for (int i = 2; i <= num; i++)
                            {
                                Console.Write($"Ingresa el número {i}: ");
                                int numero = Convert.ToInt32(Console.ReadLine());
                                resultado *= numero;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Resultado: " + resultado);
                            Console.WriteLine("¿Quieres seguir multiplicando? (1. Sí, 2. No)");
                            Console.Write("> ");
                            op3 = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine();
                        break;
                    }

                    case 4:
                    {
                        int op4 = 0;
                        while (op4 != 2)
                        {
                            Console.WriteLine();
                            Console.WriteLine("¿Cuántos números quieres dividir?");
                            Console.Write(">");
                            int num = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine();

                            if (num <= 0) { Console.WriteLine("Nada que dividir."); break; }

                            Console.Write("Ingresa el número 1: ");
                            double resultado = Convert.ToDouble(Console.ReadLine());

                            for (int i = 2; i <= num; i++)
                            {
                                Console.Write($"Ingresa el número {i}: ");
                                double divisor = Convert.ToDouble(Console.ReadLine());
                                if (divisor == 0)
                                {
                                    Console.WriteLine("Error: división entre cero. Operación cancelada.");
                                    break;
                                }
                                resultado /= divisor;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Resultado: " + resultado);
                            Console.WriteLine("¿Quieres seguir dividiendo? (1. Sí, 2. No)");
                            Console.Write("> ");
                            op4 = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine();
                        break;
                    }

                    case 5:
                        Console.WriteLine("Has salido del programa.");
                        opI = 2;
                        break;

                    default:
                        Console.WriteLine("No es una opción correcta, vuélvelo a intentar.");
                        break;
                }
            }
        }
    }
}
