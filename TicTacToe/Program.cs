using System;

namespace TicTacToe
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new TicTacToe();
            game.Run();
        }
    }
}
