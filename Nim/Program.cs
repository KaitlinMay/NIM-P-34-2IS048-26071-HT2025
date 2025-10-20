namespace Nim;

class Program
{
  static void Main(string[] args)
  {
    Console.BackgroundColor = ConsoleColor.DarkGray;
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkBlue;

    Welcome();
  }

  public static void Welcome()
{
    Console.Clear();
    Console.WriteLine("Välkommen!");
    Console.WriteLine("");
    Console.WriteLine("Vi kommer spela NIM tillsammans");
    Console.WriteLine("");
    Console.WriteLine("Spelreglarna är: ");
    Console.WriteLine("Spelet börjar med att man placerar fem stickor i tre olika högar.\r\nDärefter turas spelarna om att plocka stickor från dem tills de är tomma.\r\nDen spelare som har plockat den sista stickan har vunnit spelet.");
    Console.WriteLine("");
    Console.WriteLine("Vem ska spela? Skriv ditt namn");
    Console.WriteLine("");
    string? playerName = Console.ReadLine();
    Console.WriteLine("");
    Console.WriteLine("Hej " + playerName +"!");
    Console.WriteLine("");
    Console.WriteLine("Nu kör vi");
    Console.WriteLine("");
    

}
}
