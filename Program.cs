using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cocaro
{
    internal class Program
    {
        static string player = "o";
        static bool timedown = true;
        static int Khoitao()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Wellcome to CARO chess");
            Console.ResetColor();
            Thread.Sleep(1500);
            Console.WriteLine($"\n2 cach dieu khien");
            Thread.Sleep(1000);
            Console.WriteLine("Cach 1: W_(len) A_(trai) S_(xuong) D_(phai) va space_(danh)");
            Console.WriteLine("Cach 2: cac phim len-xuong-trai-phai va enter(danh) trong ban phim");
            Thread.Sleep(1000);
            Console.WriteLine($"\nMoi nguoi choi co 60s de danh");
            Thread.Sleep(3000);
            bool kt_size = true;
            Console.Clear();
            Console.WriteLine("Nhap kich thuoc ban co NxN (10=<N=<30)");
            int n = int.Parse(Console.ReadLine());
            do
            {
                kt_size = true;
                if (n < 10 || n > 30)
                {
                    Console.WriteLine("Ngoai gio han cho phep! Moi nhap lai");
                    n = int.Parse(Console.ReadLine());
                    Console.Clear();
                    kt_size = false;
                }
            } while (kt_size == false);
            return n;
        }
        static int n = Khoitao();
        static int dong = 3 + n / 2;
        static int cot = 3 + n / 2;
        static string[,] banco = new string[n + 8, n + 8];       
        static void Main(string[] args)
        {
            bool chonOX = true;
            bool playagain = true;
            do
            {
                timedown = true;
                Bancocaro();
                dong = 3 + n / 2;
                cot = 3 + n / 2;
                do
                {
                    chonOX = true;
                    Console.WriteLine($"\nChon quan X hay O di truoc");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("1. O");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("2. X");
                    Console.ResetColor();
                    string choice = Console.ReadLine();

                    if (choice.Trim().Equals("1"))
                    {
                        player = "o";
                    }
                    else if (choice.Trim().Equals("2"))
                    {
                        player = "x";
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("ERROR!");
                        Console.WriteLine("Moi Ban Nhap Lai!");
                        chonOX = false;
                    }
                } while (chonOX == false);
                Thread t = new Thread(Threading_time);
                int tam = 1;
                while (true)
                {
                    Console.Clear();
                    Print_banco();
                    if (tam == 1) t.Start();
                    tam = 0;
                    if (timedown == false)
                    {
                        t.Abort();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nEnd Game!!! Vi qua thoi gian quy dinh");
                        Console.ForegroundColor = ConsoleColor.Red;
                        player = (player == "o") ? "x" : "o";
                        Console.WriteLine($"\nNguoi choi {player.ToUpper()} Win");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                        {
                            dong--;
                            if (dong < 4) dong = n + 3;
                        }
                        else
                        if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                        {
                            dong++;
                            if (dong > n + 3) dong = 4;
                        }
                        else
                        if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                        {
                            cot++;
                            if (cot > n + 3) cot = 4;
                        }
                        else
                        if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                        {
                            cot--;
                            if (cot < 4) cot = n + 3;
                        }
                        else
                        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                        {
                            if (timedown != false)
                                if (banco[dong, cot] == "-")
                                {
                                    t.Abort();
                                    t = new Thread(Threading_time);
                                    t.Start();
                                    banco[dong, cot] = player;
                                    if (Kiemtrawin() == 2)
                                    {
                                        t.Abort();
                                        Console.Clear();
                                        Print_banco();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("End Game!!!");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"\nNguoi choi {player.ToUpper()} Win");
                                        Console.ResetColor();
                                        break;
                                    }
                                    else if (Kiemtrawin() == 1)
                                    {
                                        t.Abort();
                                        Console.Clear();
                                        Print_banco();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine($"\nEnd Game!!!");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"\n2 Players Draw");
                                        Console.ResetColor();
                                        break;
                                    }
                                    player = (player == "o") ? "x" : "o";
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.SetCursorPosition(0, n + 5);
                                    Console.WriteLine("Can danh vao o trong");
                                    Console.ResetColor();
                                    Thread.Sleep(2000);
                                }
                        }
                    }
                }
                Thread.Sleep(3000);
                Console.WriteLine($"\nAn phim bat ki de tiep tuc");
                Console.ReadKey();
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Ban co muon choi lai khong?");
                Console.WriteLine("1. Co");
                Console.WriteLine("2. Khong");
                bool kt = true;
                do
                {
                    kt = true;
                    string again = Console.ReadLine();
                    if (again.Equals("2")) playagain = false;
                    else if (again.Equals("1")) Console.Clear();
                    else
                    {
                        Console.WriteLine("ERROR! Moi Nhap Lai!");
                        kt = false;
                    }
                } while (kt == false);
            }
            while (playagain == true);
        }
        static void Bancocaro()
        {
            for (int i = 0; i < n + 8; i++)
                for (int j = 0; j < n + 8; j++)
                    banco[i, j] = "-";
        }
        static void Print_banco()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Luot cua " + player.ToUpper());
            Console.ResetColor();
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Time:");
            Console.SetCursorPosition(0, 4);
            for (int i = 4; i < n + 4; i++)
            {
                for (int j = 4; j < n + 4; j++)
                {
                    if (i == dong && j == cot)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(banco[i, j] + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (banco[i, j] == "o")
                            Console.ForegroundColor = ConsoleColor.Blue;
                        else
                            if (banco[i, j] == "x")
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(banco[i, j] + " ");
                        Console.ResetColor();
                    }

                }
                Console.WriteLine();
            }
        }
        static int Kiemtrawin()
        {
            // dong
            for (int i = 4; i >= 0; i--)
            {
                int dem = 0;
                for (int j = 0; j <= 4; j++)
                {
                    if (banco[dong, cot] == banco[dong, cot - i + j]) dem++;
                }
                if (dem == 5) return 2;
            }
            //cot
            for (int i = 4; i >= 0; i--)
            {
                int dem = 0;
                for (int j = 0; j <= 4; j++)
                {
                    if (banco[dong, cot] == banco[dong - i + j, cot]) dem++;
                }
                if (dem == 5) return 2;
            }
            //cheo
            for (int i = 4; i >= 0; i--)
            {
                int dem = 0;
                for (int j = 0; j <= 4; j++)
                {
                    if (banco[dong, cot] == banco[dong - i + j, cot - i + j]) dem++;
                }
                if (dem == 5) return 2;
            }
            for (int i = 4; i >= 0; i--)
            {
                int dem = 0;
                for (int j = 0; j <= 4; j++)
                {
                    if (banco[dong, cot] == banco[dong + i - j, cot - i + j]) dem++;
                }
                if (dem == 5) return 2;
            }
            //hoa
            if (Kt_o_chua_danh() == false)
            {
                return 1;
            }
            else
                return 0;
        }
        static bool Kt_o_chua_danh()
        {
            bool kt_o_chua_danh = false;
            for (int i = 4; i < n + 4; i++)
                for (int j = 4; j < n + 4; j++)
                    if (banco[i, j] == "-")
                    {
                        kt_o_chua_danh = true;
                        goto breakOut;
                    }
                breakOut:
            return kt_o_chua_danh;
        }
        static void Threading_time()
        {
            for (int i = 60; i >= 0; i--)
            {
                Console.SetCursorPosition(7, 2);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(i + " ");
                Console.ResetColor();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if (i == 0) timedown = false;
            }

        }

    }
}
