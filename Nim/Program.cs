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
    else
    {
      Console.WriteLine($"Nu är det {player2Name}s tur");
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
    int[] gameStateCopy = [.. gameState];

    Console.WriteLine("Ange hög och antal i följande format: \n\"<hög> <antal>\"");

    for (int i = 0; i < 20; i++)
    {
      string response = Console.ReadLine() ?? "";

      if (response.Trim() == "")
      {
        Console.WriteLine("Försök igen!");
        Thread.Sleep(500);
        continue;
      }
      string[] splitResponse = response.Split(' ');
      string? pileResponse = splitResponse[0];
      string? countResponse = splitResponse[1];
      if (pileResponse == null || countResponse == null)
      {
        Console.WriteLine("Försök igen!");
        Thread.Sleep(500);
        continue;
      }
      if (splitResponse.Length > 2)
      {
        Console.WriteLine("Du har inte följt instruktionerna, jag kommer ignorera allt förutom de två första siffrorna.");
        Thread.Sleep(500);
      }
      if (!int.TryParse(pileResponse, out int pile) || !int.TryParse(countResponse, out int count))
      {
        Console.WriteLine("Du angav inte två siffror \nFörsök igen!");
        Thread.Sleep(500);
        continue;
      }
      int pileIndex = pile - 1;
      if (pileIndex < 0 || pileIndex > gameStateCopy.Length - 1)
      {
        Console.WriteLine("Du angav en ogiltig hög \nFörsök igen!");
        Thread.Sleep(500);
        continue;
      }
      if (gameStateCopy[pileIndex] <= 0)
      {
        Console.WriteLine("Denna hög är töm \nFörsök igen!");
        Thread.Sleep(500);
        continue;
      }
      if (count < 1)
      {
        Console.WriteLine("Du måste plocka minst en sticka >:(");
        Thread.Sleep(500);
        continue;
      }

      if (gameStateCopy[pileIndex] < count)
      {
        Console.WriteLine("Du tog för många stickor, vi löser det.");
        Thread.Sleep(500);
      }
      int newPileCount = gameStateCopy[pileIndex] - count;
      if (newPileCount < 0)
      {
        newPileCount = 0;
      }
      gameStateCopy[pileIndex] = newPileCount;
      return gameStateCopy;

    }
    Console.WriteLine("Vad håller du på med?");
    Environment.Exit(0);
    return []; // Should never happen, makes code happy
  }
}
