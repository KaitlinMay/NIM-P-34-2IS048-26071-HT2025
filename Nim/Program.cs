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
        Thread.Sleep(500);

        if (isPlayer1Turn)
        {
          Console.WriteLine($"Nu är det {player1Name}s tur \n");
          if (player1Name == "AI:]" || player1Name == "AI:[")
          {
            Console.WriteLine("AI tänker ...\n");
            Thread.Sleep(2000);
            string aiPlay = AIPlayer(gameState);
            gameState = PlayerTurn(gameState, aiPlay);
            DrawBoard(gameState);
            Console.WriteLine($"AI spelade <{aiPlay}>\n");
            Thread.Sleep(2000);
          }
          else gameState = PlayerTurn(gameState); DrawBoard(gameState);
        }
        else
        {
          Console.WriteLine($"Nu är det {player2Name}s tur \n");
          if (player2Name == "AI:]" || player2Name == "AI:[")
          {
            Console.WriteLine("AI tänker ...\n");
            Thread.Sleep(1999); // This AI is a faster thinker  
            string aiPlay = AIPlayer(gameState);
            gameState = PlayerTurn(gameState, aiPlay);
            DrawBoard(gameState);
            Console.WriteLine($"AI spelade <{aiPlay}>\n");
            Thread.Sleep(2000);
          }
          else gameState = PlayerTurn(gameState); DrawBoard(gameState);
        }
        isPlayer1Turn = !isPlayer1Turn;
      }

      if (!isPlayer1Turn)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Grattis {player1Name}! Du vann!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        SetPlayerScore(leaderboardKeys, leaderboardValues, player1Name, 1);
        SetPlayerScore(leaderboardKeys, leaderboardValues, player2Name, 0);
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Grattis {player2Name}! Du vann!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        SetPlayerScore(leaderboardKeys, leaderboardValues, player2Name, 1);
        SetPlayerScore(leaderboardKeys, leaderboardValues, player1Name, 0);
      }

      DrawScoreboard(leaderboardKeys, leaderboardValues);
      Console.WriteLine("");

      Console.WriteLine("Tack för att ni spelade! \n \nVill ni spela igen? [J/n] \n");
      Console.ForegroundColor = ConsoleColor.DarkGray;
      string replayResponse = (Console.ReadLine() ?? "").Trim(); 
      Console.ForegroundColor = ConsoleColor.Cyan;
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
    Console.WriteLine("Välkommen!\n");
    Console.WriteLine("Vi kommer spela NIM tillsammans\n");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("Spelreglarna är: \n");
    Console.WriteLine("Spelet börjar med att man placerar fem stickor i tre olika högar.\r\nDärefter turas spelarna om att plocka stickor från dem tills de är tomma.\r\nDen spelare som har plockat den sista stickan har vunnit spelet.");
    Console.ForegroundColor = ConsoleColor.Cyan;

  }

  /// <summary>
  /// Prompts the player to give a name and validate that it is defined and unique. Names may only be four characters long. 
  /// </summary>
  /// <param name="reservedName"> Prevent user from using this name. </param>
  /// <returns> A valid name </returns>
  public static string GetPlayerName(string reservedName = "")
  {
    for (int tries = 0; tries < 13; tries++)
    {
      Console.WriteLine("");
      Console.WriteLine("Vem ska spela?\n\nSkriv ditt namn eller \"AI\" för att skapa en AI-spelare");
      Console.WriteLine("");
      Console.ForegroundColor = ConsoleColor.DarkGray;
      string playerName = Console.ReadLine() ?? "ERROR";
      Console.ForegroundColor = ConsoleColor.Cyan;

      if (playerName.Trim() == "")
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du behöver ange ett namn!\n\nFörsök igen!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        continue;
      }

      if (playerName.Length > 4)
      {
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Välj ett namn med mest fyra bokstaver\n\nFörsök igen!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        continue;
      }

      if (playerName == reservedName || playerName == "AI:]" || playerName == "AI:[")
      {
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Namnet är upptaget\n\nFörsök igen!");
        Console.ForegroundColor = ConsoleColor.Cyan;
        continue;
      }

      if (playerName.Trim().ToLower() == "ai")
      {
        if (reservedName == "AI:]") playerName = "AI:[";
        else playerName = "AI:]";
        Console.WriteLine("");
        Console.WriteLine("Du har skapat en AI-spelare!");
        Thread.Sleep(1000);
      }
      Console.WriteLine("");
      Console.WriteLine("Hej " + playerName + "!");

      return playerName;
    }
    return "Nimm"; // Fallback name if player does not give a name
  }

  /// <summary>
  /// Renders game state.
  /// </summary>
  /// <param name="gameState"> Value at each index is an amount of sticks </param>
  public static void DrawBoard(int[] gameState)
  {
    Console.Clear();
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Black;
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.Write("\x1b[1m"); // Make the sticks bold
    Console.WriteLine($"{" ",9}");
    for (int i = 0; i < gameState.Length; i++)
    {
      Console.WriteLine($"{" ",2}{new string('|', gameState[i]),-5}{" ",2}");
      Console.WriteLine($"{" ",9}");

    }
    Console.Write("\x1b[22m"); // Un-bold
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.BackgroundColor = ConsoleColor.Black;
  }

  /// <summary>
  /// Prompts player to make a move and update gameState.
  /// </summary>
  /// <param name="gameState"> Value at each index is an amount of sticks </param>
  /// <param name="responseOverride"> When defined, bypass Console.ReadLine() and use this instead </param>
  /// <returns> updated gameState </returns>
  public static int[] PlayerTurn(int[] gameState, string? responseOverride = null)
  {
    // Copy game state by creating another array referencing the same values in the original array so that the original game state is not changed by this method during gameplay
    int[] gameStateCopy = [.. gameState];

    Console.WriteLine("Ange hög och antal i följande format:\n\n<hög> <antal>\n");

    for (int i = 0; i < 20; i++)
    {
      string response;
      Console.ForegroundColor = ConsoleColor.DarkGray;
      if (responseOverride != null) response = responseOverride;
      else response = Console.ReadLine() ?? "";
      Console.WriteLine("");
      Console.ForegroundColor = ConsoleColor.Cyan;


      if (response.Trim() == "")
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Försök igen!\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }
      string[] splitResponse = response.Trim().Split(' ');
      if (splitResponse.Length < 2)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du angav inte två siffror\n\nFörsök igen!\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }
      string pileResponse = splitResponse[0];
      string countResponse = splitResponse[1];

      if (!int.TryParse(pileResponse, out int pile) || !int.TryParse(countResponse, out int count))
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du angav inte två siffror\n\nFörsök igen!\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }
      if (splitResponse.Length > 2)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du har inte följt instruktionerna, jag kommer ignorera allt förutom de två första siffrorna.\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
      }
      int pileIndex = pile - 1;
      if (pileIndex < 0 || pileIndex > gameStateCopy.Length - 1)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du angav en ogiltig hög\n\nFörsök igen! \n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }
      if (gameStateCopy[pileIndex] <= 0)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Denna hög är töm\n\nFörsök igen! \n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }
      if (count < 1)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du måste plocka minst en sticka >:( \n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
        continue;
      }

      if (gameStateCopy[pileIndex] < count)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du tog för många stickor, vi löser det. \n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Thread.Sleep(1000);
      }
      int newPileCount = gameStateCopy[pileIndex] - count;
      if (newPileCount < 0)
      {
        newPileCount = 0;
      }
      gameStateCopy[pileIndex] = newPileCount;
      return gameStateCopy;

    }
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Vad håller du på med?");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Environment.Exit(0);
    return []; // Should never happen, makes code happy
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="gameState"></param>
  /// <returns></returns>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="leaderboardKeys"></param>
  /// <param name="leaderboardValues"></param>
  public static void DrawScoreboard(string[] leaderboardKeys, int[] leaderboardValues)
  {
    (string[] sortedLeaderboardKeys, int[] sortedLeaderboardValues) = SortLeaderboard(leaderboardKeys, leaderboardValues);

    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Blue;
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
    Console.ForegroundColor = ConsoleColor.Cyan;
  }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="leaderboardKeys"></param>
  /// <param name="leaderboardValues"></param>
  /// <param name="playerName"></param>
  /// <param name="score"></param>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="leaderboardKeys"></param>
  /// <param name="leaderboardValues"></param>
  /// <returns></returns>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="gameState"></param>
  /// <returns></returns>
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
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("AI gick sönder :O \nAvslutar programmet...");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Environment.Exit(0);
    return "";
  }
}
