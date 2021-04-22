using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace Command_Line_Task_Manager
{
    class Program
    {
        private static string x;
        static void list_processes()
        {
            Process[] processes = Process.GetProcesses();

            DateTime[] lastTime = new DateTime[processes.Length];
            TimeSpan[] lastTotalProcessorTime = new TimeSpan[processes.Length];

            DateTime[] curTime = new DateTime[processes.Length];
            TimeSpan[] curTotalProcessorTime = new TimeSpan[processes.Length];

            bool[] check = new bool[processes.Length];

            //Caculate Lasttime, and lastTotal processor for each process...
            for (int i = 0; i < processes.Length; i++)
            {
                lastTime[i] = DateTime.Now;
                try
                {
                    lastTotalProcessorTime[i] = processes[i].TotalProcessorTime;
                    check[i] = true;
                }
                catch (Exception)
                {
                    check[i] = false;
                }
            }
            Console.WriteLine("Wait 3 seconds to calculate CPU Usage...");
            Thread.Sleep(3000);

            //Caculate Curtime, and curTotal processor for each process...
            for (int i = 0; i < processes.Length; i++)
            {
                curTime[i] = DateTime.Now;
                try
                {
                    curTotalProcessorTime[i] = processes[i].TotalProcessorTime;
                    check[i] = true;
                }
                catch (Exception)
                {
                    check[i] = false;
                }
            }

            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("|    processName                |       ProcessID   |          Cpu Usage   |");

            for (int i = 0; i < processes.Length; i++)
            {
                double CPUUsage = 0.0;

                if (check[i])
                {
                    CPUUsage = (curTotalProcessorTime[i].TotalMilliseconds - lastTotalProcessorTime[i].TotalMilliseconds) / curTime[i].Subtract(lastTime[i]).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);

                    Console.WriteLine("----------------------------------------------------------------------------");
                    Console.WriteLine("|    {0}                |       {1}     |          {2:0.0}%    |", processes[i].ProcessName, processes[i].Id, CPUUsage * 100);
                }
                else
                {
                    Console.WriteLine("----------------------------------------------------------------------------");
                    Console.WriteLine("|    {0}                |       {1}     |          Access is denied   |", processes[i].ProcessName, processes[i].Id);

                }
            }
            Console.WriteLine("----------------------------------------------------------------------------");
        }
        static void kill_process(int id)
        {
            Process p;
            try
            {
                p = Process.GetProcessById(id);
            }
            catch (Exception)
            {
                Console.WriteLine("process with  an ID of {0} is not running, or Accesss to it is denied.", id);
                return;
            }
            try
            {
                p.Kill();
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, process with  an ID  of {0} is not killed ,may be access to it is denied.", id);
                return;
            }
            Console.WriteLine("process with  an ID  of {0} is killed right now ...", id);
        }

        static void change_priority(int id)
        {
            Process pprocess;
            try
            {
                pprocess = Process.GetProcessById(id);
            }
            catch (Exception)
            {
                Console.WriteLine("process with  an ID of {0} is not running, or Accesss to it is denied.", id);
                return;
            }
            bool b = true;
            while (b)
            {
                Console.Write("Enter priority type: ");
                string s = Console.ReadLine().Trim();
                switch (s)
                {
                    case "idle":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.Idle;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }
                        Console.WriteLine("Priority has been changed to idle right now ...");
                        break;
                    case "normal":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.Normal;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }
                        Console.WriteLine("Priority has been changed to Normal right now ...");
                        break;
                    case "high":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.High;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }
                        Console.WriteLine("Priority has been changed to High right now ...");
                        break;
                    case "realtime":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.RealTime;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }
                        Console.WriteLine("Priority has been changed to RealTime right now ...");
                        break;
                    case "abovenormal":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.AboveNormal;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }

                        Console.WriteLine("Priority has been changed to AboveNormal right now ...");
                        break;
                    case "belownormal":
                        b = false;
                        try
                        {
                            pprocess.PriorityClass = ProcessPriorityClass.BelowNormal;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Access is denied");
                            break;
                        }
                        Console.WriteLine("Priority has been changed to BelowNormal right now ...");
                        break;
                    default:
                        Console.WriteLine("The system cannot accept the priority type entered.");
                        break;
                }
            }


        }
        static void help()
        {
            Console.WriteLine("list processes       Display current running processes.");
            Console.WriteLine("kill process         kill specified process with id.");
            Console.WriteLine("change process       change priority of a specified process with id.");
            Console.WriteLine("idle                 to set process priority to idle.");
            Console.WriteLine("high                 to set process priority to high.");
            Console.WriteLine("normal               to set process priority to normal.");
            Console.WriteLine("realtime             to set process priority to realtime.");
            Console.WriteLine("abovenoraml          to set process priority to abovenoraml.");
            Console.WriteLine("belownormal          to set process priority to belownormal.");
            Console.WriteLine("Exit                 To Exit out of The task manager.");
        }
        static void Main(string[] args)
        {
            var v = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.WriteLine("Welcome to Task Manager CLI (~_~)");
            do
            {
                Console.Write("{0}{1}", v, ">");
                x = Console.ReadLine().Trim();
                switch (x)
                {
                    case "list processes":
                        list_processes();
                        break;
                    case "kill process":
                        while (true)
                        {
                            int t;
                            Console.Write("Enter process id: ");
                            try
                            {
                                t = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("The system cannot accept the id number entered.");
                                continue;
                            }
                            kill_process(t);
                            break;
                        }
                        break;
                    case "change priority":
                        while (true)
                        {
                            int tt;
                            Console.Write("Enter process id: ");
                            try
                            {
                                tt = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("The system cannot accept the id number entered.");
                                continue;
                            }
                            change_priority(tt);
                            break;
                        }
                        break;
                    case "help":
                        help();
                        break;
                    default:
                        if (x != "Exit" && x != "")
                            Console.WriteLine("\'{0}\' is not recognized as an internal or external command,operable program or batch file.", x);
                        break;
                }
            } while (x != "Exit");
        }
    }
}
