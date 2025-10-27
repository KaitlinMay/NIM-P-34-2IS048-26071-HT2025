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
    // leaderboardKeys[0] = "k";
    // leaderboardValues[0] = 112;
    // leaderboardKeys[3] = "v";
    // leaderboardValues[3] = 17;
    SetPlayerScore(leaderboardKeys, leaderboardValues, "Kate", 142);
    SetPlayerScore(leaderboardKeys, leaderboardValues, "V" );

    DrawLeaderboard(leaderboardKeys, leaderboardValues);
    Environment.Exit(0);

    Welcome();

    while (true)
    {
      Console.Clear();
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
        Console.WriteLine("");
        Console.WriteLine($"Grattis {player1Name}! Du vann! \n");
      }
      else
      {
        Console.WriteLine("");
        Console.WriteLine($"Grattis {player2Name}! Du vann! \n");
      }

      Console.WriteLine("Tack för att ni spelade! \n \nVill ni spela igen? [J/n] \n");
      string replayResponse = (Console.ReadLine() ?? "").Trim(); Console.WriteLine("");
      if (replayResponse == "n" || replayResponse == "nej" || replayResponse == "ne")
      {
        Console.WriteLine("Tack för att ni spelade! \nSpelet stänger...");
        break;
      }

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
    Console.ForegroundColor = ConsoleColor.DarkGray;
    string playerName = Console.ReadLine() ?? "ERROR";
    Console.ForegroundColor = ConsoleColor.Cyan;

    if (playerName.Trim() == "") playerName = "Nise"; // Cutting "Nisse" to 4 letters

    Console.WriteLine("");
    Console.WriteLine("Hej " + playerName + "!");
    Console.WriteLine("");

    return playerName;
  }

  public static void DrawBoard(int[] gameState)
  {
    Console.Clear();
    Console.WriteLine(" ");
    Console.ForegroundColor = ConsoleColor.Black;
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.WriteLine($"{" ",9}");
    for (int i = 0; i < gameState.Length; i++)
    {
      Console.WriteLine($"{" ",2}{new string('|', gameState[i]),-5}{" ",2}");
      Console.WriteLine($"{" ",9}");

    }
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.BackgroundColor = ConsoleColor.Black;
  }

  public static int[] PlayerTurn(int[] gameState)
  {
    // Copy game state by creating another array referencing the same values in the original array so that the original game state is not changed by this method during gameplay
    int[] gameStateCopy = [.. gameState];

    Console.WriteLine(" ");
    Console.WriteLine("Ange hög och antal i följande format: \n\"<hög> <antal>\" \n");

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

  public static void DrawLeaderboard(string[] leaderboardKeys, int[] leaderboardValues)
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
}
