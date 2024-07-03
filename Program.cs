using TicTacToe.game;

namespace Program {
    internal class Program {
        private static void Main(string[] args) {
            GameManager gameManager = new GameManager(3);

            gameManager.StartGame();
        }
    }
}