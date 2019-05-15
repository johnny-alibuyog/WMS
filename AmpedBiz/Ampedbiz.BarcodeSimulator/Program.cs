using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using WindowsInput;

namespace Ampedbiz.BarcodeSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            var simulator = new InputSimulator();

            var barcodes = new List<string>()
            {
                "748485700045",
                "4800888600806",
                "4800575141100",
                "4800194116466",
                "4902430463713",
                "4808680652146",
                "4902430389556",
                "48032025",
                "748485800035",
                "4800110025995",
                "4800417000947",
                "4800024579997",
                "8993175539241",
                "4800024555533",
                "4800024562258",
                "4902430774024",
                "4800488958772",
                "4806018405655",
                "7622300637996",
                "4800086045560",
                "4902430583169",
                "4800086043641",
                "4800016787027",
                "4902430154154",
                "4800047841712",
                "4800888150202",
                "4800047841781",
                "4809012413046",
                "4800361383493",
                "4806018403859",
            };

            barcodes.ForEach(barcode =>
            {
                simulator.Keyboard.Sleep(2000).TextEntry(barcode);
            });

                //.ToObservable(Scheduler.Default)
                //.Throttle(
                //    dueTime: TimeSpan.FromSeconds(random.Next(1, 2)), 
                //    scheduler: Scheduler.Default
                //)
                //.Subscribe(barcode => simulator.Keyboard.Sleep(2000).TextEntry(barcode));


            //var observable = EndlessBarcodeScans().ToObservable(Scheduler.Default);

            //var observableThrottled = observable.Throttle(TimeSpan.FromSeconds(1), Scheduler.Default);

            //observableThrottled.Subscribe(i => Console.WriteLine($"{i}\nTime Received {DateTime.Now.ToString()}\n"));

            Console.WriteLine("\nPress ENTER to exit...\n");

            Console.ReadLine();
        }

        //private static SimulateBarcodeScan(string barcode)
        //{
        //    var simulator = new InputSimulator();


        //}

        //private static IEnumerable<string> EndlessBarcodeScans()
        //{
        //    var random = new Random();

        //    var barcodes = new List<string>()
        //    {
        //        "Email Msg from John ",
        //        "Email Msg from Bill ",
        //        "Email Msg from Marcy ",
        //        "Email Msg from Wes "
        //    };

        //    while (true)
        //    {
        //        yield return barcodes[random.Next(barcodes.Count)];
        //        Thread.Sleep(random.Next(1000));
        //    }
        //}
    }
}
