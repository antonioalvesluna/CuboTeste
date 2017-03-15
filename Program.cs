using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace CuboTeste
{
    class Program
    {
        public static List<Piloto> pilotos = new List<Piloto>();
        public static List<Volta> voltas = new List<Volta>();     

        static void Main(string[] args)
        {
            var result = lerGrid().OrderByDescending(p => p.voltas.Count).OrderBy(t => t.tempo);

            foreach (var res in result)
            {
                Console.WriteLine("{0} {1} {2} {3}", res.codigo, res.nome, res.voltas.Count(), res.tempo );
               
            }

            Console.WriteLine("");
            Console.WriteLine("Melhores Voltas:");

            foreach (var MelhorVolta in result)
            {
                
                Console.WriteLine("{0} {1}", getMelhorVolta(MelhorVolta.codigo), MelhorVolta.nome );

            }
            
            Console.ReadKey();
        }


        public static List<Piloto> lerGrid()
        {
            List<Piloto> pl = new List<Piloto>();

            using (StreamReader sr = new StreamReader(@"Arqs\Grid.txt"))
            {
                
                while (!sr.EndOfStream )
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    var vetor = sr.ReadLine().Split(';');

                    string tempo = "0:0:"+vetor[4].ToString();

                    pilotos.Add(new Piloto { codigo = Convert.ToInt32(vetor[1]), nome = vetor[2].ToString() });
                    voltas.Add(new Volta { codPiloto = Convert.ToInt32(vetor[1]), numVolta = Convert.ToInt32( vetor[3]), tempoVolta = TimeSpan.Parse( tempo ), velocidadeMediaVolta = Convert.ToDecimal(vetor[5]) } );
                   
                }
            }


            var distPiloto = pilotos.Select(m => m.codigo).Distinct().ToList();

            foreach (var piloto in distPiloto)
            {
                var laps = voltas.Where(p => p.codPiloto == piloto).ToList();
                var x = pilotos.Where(p => p.codigo == piloto).FirstOrDefault();
                x.voltas = laps;
                x.tempo = laps.Sum(p => p.tempoVolta.Ticks );
                pl.Add(x);

            }

            return pl;
        }

        public static TimeSpan getMelhorVolta(int codigo)
        {
            var piloto = pilotos.Where(p => p.codigo == codigo).FirstOrDefault();
            var betterLap = piloto.voltas.Select(p => p.tempoVolta).Min();
            return betterLap ;
        }
    }

    public class Piloto
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public List<Volta> voltas { get; set; }
        public Int64 tempo { get; set; }

    }

    public class Volta
    {
        public int codPiloto { get; set; }
        public int numVolta { get; set; }
        public TimeSpan tempoVolta { get; set; }
        public Decimal velocidadeMediaVolta { get; set; }
    }
}
