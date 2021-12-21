using System.IO;

namespace AoC2021.Days.Day21Utils
{
    struct GameState
    {
        public int Player1Square;
        public int Player2Square;
        public int Player1Score;
        public int Player2Score;
        public bool Is1Turn; // whether it's player1's turn

        public GameState(int player1Square, int player2Square, int player1Score, int player2Score, bool is1Turn)
        {
            Player1Square = player1Square;
            Player2Square = player2Square;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Is1Turn = is1Turn;
        }

        public bool GameOver()
        {
            return Player1Score >= 21 || Player2Score >= 21;
        }

        public int Winner()
        {
            if (Player1Score >= 21 && Player2Score >= 21)
                throw new InvalidDataException();

            if (Player1Score >= 21)
                return 1;
            if (Player2Score >= 21)
                return 2;
                
            return -1;
        }
    }
}