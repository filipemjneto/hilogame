// See https://aka.ms/new-console-template for more information
using HiloClient;

Console.WriteLine("Starting Game Client");

using (GamePlay gp = GamePlay.GetGamePlay())
{
    await gp.GameLoop();

    Console.WriteLine(gp.GetResults());
}

Console.WriteLine("Game Over!");