/* 
Kaitlin May
Vena Ström
2025-10-27
VS Code 1.105.1
 */

namespace Nim;

class Program
{
  static void Main(string[] args)
  {
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();

    int leaderboardLength = 10;
    string[] leaderboardKeys = new string[leaderboardLength];
    int[] leaderboardValues = new int[leaderboardLength];

    Welcome();

    while (true)
    {
      // Console.Clear();
      string player1Name = GetPlayerName();
      string player2Name = GetPlayerName(player1Name);
      Console.WriteLine(" ");
      Console.WriteLine("NU KÖR VI \n");
      Thread.Sleep(1500);

      int[] gameState = [5, 5, 5];
      bool isPlayer1Turn = true;

      while (!IsGameOver(gameState))
      {
        DrawBoard(gameState);
        Console.WriteLine("");
        Thread.Sleep(500);

        if (isPlayer1Turn)
        {
          Console.WriteLine($"Nu är det {player1Name}s tur");
          if (player1Name == "AI:]" || player1Name == "AI:[")
          {
            Console.WriteLine("AI tänker ...");
            Thread.Sleep(2000);
            string aiPlay = AIPlayer(gameState);
            gameState = PlayerTurn(gameState, aiPlay);
            DrawBoard(gameState);
            Console.WriteLine($"AI spelade <{aiPlay}>");
            Thread.Sleep(2000);
          }
          else gameState = PlayerTurn(gameState);
        }
        else
        {
          Console.WriteLine($"Nu är det {player2Name}s tur");
          if (player2Name == "AI:]" || player2Name == "AI:[")
          {
            Console.WriteLine("AI tänker ...");
            Thread.Sleep(1999); // This AI is a faster thinker  
            string aiPlay = AIPlayer(gameState);
            gameState = PlayerTurn(gameState, aiPlay);
            DrawBoard(gameState);
            Console.WriteLine($"AI spelade <{aiPlay}>");
            Thread.Sleep(2000);
          }
          else gameState = PlayerTurn(gameState);
        }
        isPlayer1Turn = !isPlayer1Turn;
      }

      Console.WriteLine("");
      if (!isPlayer1Turn)
      {
        Console.WriteLine($"Grattis {player1Name}! Du vann! \n");
        SetPlayerScore(leaderboardKeys, leaderboardValues, player1Name, 1);
        SetPlayerScore(leaderboardKeys, leaderboardValues, player2Name, 0);
      }
      else
      {
        Console.WriteLine($"Grattis {player2Name}! Du vann! \n");
        SetPlayerScore(leaderboardKeys, leaderboardValues, player2Name, 1);
        SetPlayerScore(leaderboardKeys, leaderboardValues, player1Name, 0);
      }

      DrawScoreboard(leaderboardKeys, leaderboardValues);
      Console.WriteLine("");

      Console.WriteLine("Tack för att ni spelade! \n \nVill ni spela igen? [J/n] \n");
      string replayResponse = (Console.ReadLine() ?? "").Trim(); Console.WriteLine("");
      if (replayResponse == "n" || replayResponse == "nej" || replayResponse == "ne")
      {
        Console.WriteLine("Tack för att ni spelade! \nSpelet stänger...");
        break;
      }

    }
  }

  /// <summary>
  /// Welcomes players and displays the rules of the game.  
  /// </summary>
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

  public static string GetPlayerName(string reservedName = "")
  {
    for (int tries = 0; tries < 13; tries++)
    {
      Console.WriteLine("");
      Console.WriteLine("Vem ska spela? Skriv ditt namn eller \"AI\" för att skapa en AI-spelare");
      Console.WriteLine("");
      Console.ForegroundColor = ConsoleColor.DarkGray;
      string playerName = Console.ReadLine() ?? "ERROR";
      Console.ForegroundColor = ConsoleColor.Cyan;

      if (playerName.Trim() == "")
      {
        Console.WriteLine("Du behöver ange ett namn! \nFörsök igen!");
        continue;
      }

      if (playerName.Length > 4)
      {
        Console.WriteLine("Välj ett namn med mest fyra bokstaver \nFörsök igen!");
        continue;
      }

      if (playerName == reservedName || playerName == "AI:]" || playerName == "AI:[")
      {
        Console.WriteLine("Namnet är upptaget \nFörsök igen!");
        continue;
      }

      if (playerName.Trim().ToLower() == "ai")
      {
        if (reservedName == "AI:]") playerName = "AI:[";
        else playerName = "AI:]";
        Console.WriteLine("Du har skapat en AI-spelare!");
        Thread.Sleep(1000);
      }

      Console.WriteLine("");
      Console.WriteLine("Hej " + playerName + "!");
      Console.WriteLine("");

      return playerName;
    }
    return "Nimm"; // Fallback name if player does not give a name
  }

  public static void DrawBoard(int[] gameState)
  {
    Console.Clear();
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Black;
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.Write("\x1b[1m"); // Bold
    Console.WriteLine($"{" ",9}");
    for (int i = 0; i < gameState.Length; i++)
    {
      Console.WriteLine($"{" ",2}{new string('|', gameState[i]),-5}{" ",2}");
      Console.WriteLine($"{" ",9}");

    }
    Console.Write("\x1b[22m"); // Un-bold
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.BackgroundColor = ConsoleColor.Black;
  }

  public static int[] PlayerTurn(int[] gameState, string? responseOverride = null)
  {
    // Copy game state by creating another array referencing the same values in the original array so that the original game state is not changed by this method during gameplay
    int[] gameStateCopy = [.. gameState];

    Console.WriteLine(" ");
    Console.WriteLine("Ange hög och antal i följande format: \n<hög> <antal> \n");

    for (int i = 0; i < 20; i++)
    {
      string response;
      if (responseOverride != null) response = responseOverride;
      else response = Console.ReadLine() ?? "";

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

  public static void DrawScoreboard(string[] leaderboardKeys, int[] leaderboardValues)
  {
    (string[] sortedLeaderboardKeys, int[] sortedLeaderboardValues) = SortLeaderboard(leaderboardKeys, leaderboardValues);

    Console.WriteLine("");
    Console.WriteLine(" ________________________ ");
    Console.WriteLine("|    Vinststatistik      |");
    Console.WriteLine("| plats | namn | vinster |");
    Console.WriteLine("|-------|------|---------|");
    for (int i = 0; i < sortedLeaderboardKeys.Length; i++)
    {
      if (sortedLeaderboardKeys[i] == null || sortedLeaderboardKeys[i] == "") continue;

      // Apparently ",N" is the same as PadLeft within a template string :shrug:
      Console.WriteLine($"| {i + 1,-5} | {sortedLeaderboardKeys[i],4} | {sortedLeaderboardValues[i],7} |");
    }
    Console.WriteLine("|_______|______|_________|\n");
  }
  public static void SetPlayerScore(string[] leaderboardKeys, int[] leaderboardValues, string playerName, int score = 0)
  {
    // Try to find existing player by name
    // If they exist, ++ score,
    // else 
    // if leaderboard has space, append new entry
    // else replace lowest score entry

    int leaderboardCount = 0;

    for (int i = 0; i < leaderboardKeys.Length; i++)
    {
      if (leaderboardKeys[i] != null && leaderboardKeys[i] != "") leaderboardCount++; // for later use

      if (leaderboardKeys[i] == playerName)
      {
        leaderboardValues[i] += score;
        return;
      }
    }
    if (leaderboardCount < leaderboardKeys.Length)
    {
      for (int i = 0; i < leaderboardKeys.Length; i++)
      {
        if (leaderboardKeys[i] == null || leaderboardKeys[i] == "")
        {
          leaderboardKeys[i] = playerName;
          leaderboardValues[i] = score;
          return;
        }
      }
    }
    (string[] sortedLeaderboardKeys, int[] sortedLeaderboardValues) = SortLeaderboard(leaderboardKeys, leaderboardValues);
    sortedLeaderboardKeys[sortedLeaderboardKeys.Length - 1] = playerName;
    sortedLeaderboardValues[sortedLeaderboardValues.Length - 1] = score;
    for (int i = 0; i < leaderboardKeys.Length; i++)
    {
      leaderboardKeys[i] = sortedLeaderboardKeys[i];
      leaderboardValues[i] = sortedLeaderboardValues[i];
    }


  }

  public static (string[], int[]) SortLeaderboard(string[] leaderboardKeys, int[] leaderboardValues)
  {
    int leaderboardLength = leaderboardKeys.Length;

    string[] dict = new string[leaderboardLength];
    for (int i = 0; i < leaderboardLength; i++)
    {
      dict[i] = leaderboardKeys[i] + "|" + leaderboardValues[i];
    }

    // Bubble sort
    for (int pass = 0; pass < leaderboardLength; pass++)
    {
      for (int i = 0; i < leaderboardLength - 1; i++)
      {
        int scoreA = int.Parse(dict[i].Split("|")[1]);
        int scoreB = int.Parse(dict[i + 1].Split("|")[1]);
        if (scoreA < scoreB)
        {
          // Swap A and B
          (dict[i + 1], dict[i]) = (dict[i], dict[i + 1]);
        }
      }
    }

    string[] sortedKeys = new string[leaderboardLength];
    int[] sortedValues = new int[leaderboardLength];
    for (int i = 0; i < leaderboardLength; i++)
    {
      string[] nameAndScore = dict[i].Split("|");
      sortedKeys[i] = nameAndScore[0];
      sortedValues[i] = int.Parse(nameAndScore[1]);
    }

    return (sortedKeys, sortedValues);
  }
  public static string AIPlayer(int[] gameState)
  {
    Random random = new();
    for (int i = 0; i < 100; i++)
    {
      int pileIndex = random.Next(0, gameState.Length);
      int pileCount = gameState[pileIndex];
      if (pileCount <= 0)
      {
        continue;
      }
      int pick = random.Next(1, pileCount);
      return $"{pileIndex + 1} {pick}";
    }
    Console.WriteLine("AI gick sönder :O \nAvslutar programmet...");
    Environment.Exit(0);
    return "";
  }
}
