using System.Diagnostics.CodeAnalysis;
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
    Console.WriteLine("NU KÖR VI \n");
    Thread.Sleep(1000);

    int[] gameState = [5, 5, 5];
    bool isPlayer1Turn = true;

    while (!IsGameOver(gameState))
    {
      DrawBoard(gameState);
      Console.WriteLine("");

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
      isPlayer1Turn = !isPlayer1Turn;
    }

    if (!isPlayer1Turn)
    {
      Console.WriteLine($"Grattis {player1Name}! Du vann!");
    }
    else
    {
      Console.WriteLine($"Grattis {player2Name}! Du vann!");
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
    // Copy game state by creating another array referencing the same values in the original array so that the original game state is not changed by this method during gameplay
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
      string[] splitResponse = response.Trim().Split(' ');
      if (splitResponse.Length < 2)
      {
        Console.WriteLine("Du angav inte två siffror \nFörsök igen!");
        Thread.Sleep(500);
        continue;
      }
      string pileResponse = splitResponse[0];
      string countResponse = splitResponse[1];

      if (!int.TryParse(pileResponse, out int pile) || !int.TryParse(countResponse, out int count))
      {
        Console.WriteLine("Du angav inte två siffror \nFörsök igen!");
        Thread.Sleep(500);
        continue;
      }
      if (splitResponse.Length > 2)
      {
        Console.WriteLine("Du har inte följt instruktionerna, jag kommer ignorera allt förutom de två första siffrorna.");
        Thread.Sleep(500);
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
  public static bool IsGameOver(int[] gameState)
  {
    int sumOfSticks = 0;
    for (int i = 0; i < gameState.Length; i++) sumOfSticks += gameState[i];

    // If any pile is less than the sum of all piles then there must be sticks remaining in multiple piles, which means that the game continues
    for (int i = 0; i < gameState.Length; i++)
    {
      if (gameState[i] < sumOfSticks)
      {
        return false;
      }
    }

    // At this point, sumOfSticks is the stick count in the single remaining pile, which means that the game continues if there is more than one
    if (sumOfSticks > 1)
    {
      return false;
    }

    // By this point the single remaining pile has one or fewer sticks remaining, which means the game is over
    return true;
  }

}
