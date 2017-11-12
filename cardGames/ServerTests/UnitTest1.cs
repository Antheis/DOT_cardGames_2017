using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cardGame_Server;
using Protocol;

namespace ServerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Game_RemoveInvalidCl()
        {
            Game game = new Game(0);

            Assert.AreEqual(game.RemoveClient(null), false, "Impossible-to-remove Client not handled");
        }

        [TestMethod]
        public void Game_AddInvalidCl()
        {
            Game game = new Game(0);
            game.AddClient(null);
            game.AddClient(null);

            Assert.AreEqual(game.IsFull(), false, "Impossible-to-add Client not handled");
            Assert.AreEqual(game.nbPlayers(), 0, "Impossible-to-add Client not handled");
            Assert.AreEqual(game.IsRunning(), false, "Impossible-to-add Client not handled");
            Assert.AreEqual(game.IsPlaying(), false, "Impossible-to-add Client not handled");

            game.PrepareGame();

            Assert.AreEqual(game.IsPlaying(), false, "Impossible-to-add Client not handled");
        }

        public void Deck_GiveTests()
        {
            Deck deck = new Deck();

            for (int i = 0; i < 52; ++i)
                Assert.AreNotEqual(deck.GetNextCard(), Cards.None, "Deck gestion not handled");
            for (int i = 0; i < 10; ++i)
                Assert.AreEqual(deck.GetNextCard(), Cards.None, "Deck overflow not handled");
        }
    }
}
