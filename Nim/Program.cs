using System.Threading.Tasks.Dataflow;

namespace Nim;

class Program
{

  static void Main(string[] args)
  {
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();



    Welcome();
    string player1Name = GetPlayerName();
    string player2Name = GetPlayerName();
    Console.WriteLine(" ");
    Console.WriteLine("NU KÖR VI");
    int[] gameState = [5, 5, 5];
    DrawBoard(gameState);
    Console.WriteLine("");

    bool isPlayer1Turn = true;
    if (isPlayer1Turn)
    {
      Console.WriteLine($"Nu är det {player1Name}s tur");
      gameState = PlayerTurn(gameState);
    }
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
    
  }

  public static string GetPlayerName()
  {
    Console.WriteLine("");
    Console.WriteLine("Vem ska spela? Skriv ditt namn");
    Console.WriteLine("");
    string playerName = Console.ReadLine() ?? "ERROR";
    Console.WriteLine("");
    Console.WriteLine("Hej " + playerName + "!");
    Console.WriteLine("");
    return playerName; 
  }
  public static void DrawBoard(int[] gameState)
  {
    for (int i = 0; i < gameState.Length; i++)
    {
      Console.WriteLine(new string('|', gameState[i]));

    }

  }

  public static int[] PlayerTurn(int[] gameState)
  {


  }
}
