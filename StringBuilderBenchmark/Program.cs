// See https://aka.ms/new-console-template for more information

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace StringBuilderBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Factory>();
        }
    }
    
    public class Artikel
    {
        public string Bezeichnung { get; set; }
    }
    
    public class Lagerbewegung
    {
        public double Menge { get; set; }
    }
    
    public class KommissionierlaufPosition
    {
        public int SaniLieferscheinPosNr { get; set; }
        public Artikel? Artikel { get; init; }
        public Lagerbewegung? Lagerbewegung { get; set; }
        public double Menge { get; set; }
    }
    
    [MemoryDiagnoser]
    public class Factory
    {
        private readonly KommissionierlaufPosition _kommissionierlaufPosition;
        private readonly double? _manuelleEntnahmeMenge;

        public Factory()
        {
            _kommissionierlaufPosition = new KommissionierlaufPosition
            {
                SaniLieferscheinPosNr = 1,
                Artikel = new Artikel() {Bezeichnung = "Testartikel"},
                Lagerbewegung = new Lagerbewegung() {Menge = 2},
                Menge = 2
            };
            _manuelleEntnahmeMenge = 0;
        }
        
        [Benchmark]
        public string CreateWithStringConcat() => CreateWithStringConcat(_kommissionierlaufPosition, _manuelleEntnahmeMenge);
        
        [Benchmark]
        public string CreateWithStringBuilder() => CreateWithStringBuilder(_kommissionierlaufPosition, _manuelleEntnahmeMenge);
        
        private static string CreateWithStringConcat(KommissionierlaufPosition kommissionierlaufPosition, double? manuelleEntnahmeMenge)
        {
            return $"Pos: {kommissionierlaufPosition.SaniLieferscheinPosNr}{Environment.NewLine}" +
                   $"Artikel: {kommissionierlaufPosition.Artikel.Bezeichnung}{Environment.NewLine}" +
                   CreateMengeLine(kommissionierlaufPosition, manuelleEntnahmeMenge);
        }

        private static string CreateWithStringBuilder(KommissionierlaufPosition kommissionierlaufPosition, double? manuelleEntnahmeMenge)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Pos: {kommissionierlaufPosition.SaniLieferscheinPosNr}");
            stringBuilder.AppendLine($"Artikel: {kommissionierlaufPosition.Artikel.Bezeichnung}");
            stringBuilder.AppendLine(CreateMengeLine(kommissionierlaufPosition, manuelleEntnahmeMenge));
            return stringBuilder.ToString();
        }
        
        private static string CreateMengeLine(KommissionierlaufPosition kommissionierlaufPosition, double? manuelleEntnahmeMenge)
        {
            if (manuelleEntnahmeMenge != null)
                return $"Menge: {manuelleEntnahmeMenge} von {kommissionierlaufPosition.Menge}"; 
            
            return $"Menge: {Math.Abs(kommissionierlaufPosition.Lagerbewegung.Menge)} von {kommissionierlaufPosition.Menge}";
        }
    }
}