using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.RandomlyInitialize();

            while (true)
            {
                Console.Write("Please enter the cell coordinate: ex. A1 or C9 ");
                var cell = Console.ReadLine();

                // convert the input field to cell coordinate, e.g. A10 becomes (0, 9).
                try
                {
                    int xCoordinate = game.ParseXCoordinate(cell);
                    int yCoordinate=  game.ParseYCoordinate(cell);
                    Console.WriteLine(game.FireAMissile(xCoordinate, yCoordinate));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}
