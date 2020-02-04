﻿using System.Collections.Generic;
using ArchaicQuestII.GameLogic.Character;
using ArchaicQuestII.GameLogic.Core;
using ArchaicQuestII.GameLogic.World.Room;
using Moq;
using Xunit;

namespace ArchaicQuestII.GameLogic.Tests.Commands.Movement
{
    public class MovementTests
    {
        private GameLogic.World.Room.Room _room;
        private Player _player;
        private readonly Mock<IWriteToClient> _writer;
        private readonly Mock<ICache> _cache;

        public MovementTests()
        {
            _writer = new Mock<IWriteToClient>();
            _cache = new Mock<ICache>();

        }

        [Fact]
        public void Should_move_characters_position()
        {
            var player2 = new Player();
            player2.ConnectionId = "2";

            _player = new Player();
            _player.ConnectionId = "1";
            _player.Name = "Bob";

            _room = new Room()
            {
                AreaId = 1,
                Title = "Room 1",
                Description = "room 1",
                Exits = new ExitDirections()
                {
                    North = new Exit()
                    {
                        AreaId = 2,
                        Name = "North"
                    }
                },
                Players = new List<Player>()
                {
                    _player,
             
                    player2
                }
            };

            var room2 = new Room()
            {
                AreaId = 2,
                Title = "Room 2",
                Description = "room 2",
                Exits = new ExitDirections()
                {
                    South = new Exit()
                    {
                        AreaId = 1,
                        Name = "South"
                    }
                },
                Players = new List<Player>()
            };

            _cache.Setup(x => x.GetRoom(2)).Returns(room2);


            new GameLogic.Commands.Movement.Movement(_writer.Object, _cache.Object).Move(_room, _player, "North");

            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s.Contains("Bob walks north.")), "1"), Times.Never);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "Bob walks north."), "2"), Times.Once);
            Assert.Equal(2, _player.RoomId);
        }

       
    }
}