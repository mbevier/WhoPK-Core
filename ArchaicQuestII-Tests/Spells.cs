using ArchaicQuestII.Engine.Core.Events;
using System;
using System.Buffers;
using System.Collections.Generic;
using ArchaicQuestII.Engine.Character.Model;
using ArchaicQuestII.Engine.Effect;
using ArchaicQuestII.Engine.Item;
using ArchaicQuestII.Engine.Skill;
using ArchaicQuestII.Engine.Spell;
using ArchaicQuestII.Engine.Spell.Interface;
using ArchaicQuestII.Engine.Spell.Model;
using ArchaicQuestII.Engine.World.Room.Model;
using Moq;
using Xunit;
 

namespace ArchaicQuestII_Tests
{
    public class SpellTests
    {
        private readonly Spell _Spells;
        private readonly Player _player;
        private readonly Player _target;
        private readonly Room _room;
        private readonly Spells _spell;
        private readonly Mock<IWriteToClient> _writer;

        public SpellTests()
        {
            _Spells = new Spell()
            { 
                Name = "Ogre strength",
                Effect = new Effect()
                {
                    Modifier = new EffectModifer()
                    {
                        Value = 10,
                        PositiveEffect = true
                    },
                    Location = EffectLocation.Strength,
                    Name = "OgreStrength",
                    Duration = new EffectModifer()
                    {
                        Value = 10,
                        PositiveEffect = true,
                    },
                    Accumulate = true,
                    Id = 1
                },
                Damage = new Dice()
                {
                    DiceRoll = 1,
                    DiceMinSize = 1,
                    DiceMaxSize = 10
                },
                Description = "Makes you strong as an ogre",
                SkillAction = new List<Messages>
                {
                    new Messages
                    {
                        ToPlayer = "x",
                        ToRoom = "y",
                        ToTarget = "z"
                    }
                },
                Rounds = 1,
                SkillStart = new Messages()
                {
                    ToPlayer = "start spell",
                    ToRoom = "sy",
                    ToTarget = "sz"
                },
                Requirements = new Requirements()
                {
                    Evil = true
                },
                Type = SpellType.Affect,
                Cost = Cost.HitPoints,
                LevelBasedMessages = new LevelBasedMessages(),
                SkillEnd = new Messages()
                {
                    ToPlayer = "sx",
                    ToRoom = "sy",
                    ToTarget = "sz"
                },
                SkillFailure = new Messages()
                {
                    ToPlayer = "sx",
                    ToRoom = "sy",
                    ToTarget = "sz"
                },
                SpellGroup = new Sphere()

            };
            _player = new Player();
            _target = new Player()
            {
                Attributes = new Attributes()
                {
                   Attribute = new Dictionary<EffectLocation, int>
                   {
                       { EffectLocation.Strength, 10 }
                   }
                }

            };
            _room = new Room();
            _writer = new Mock<IWriteToClient>();
            _spell = new Spells(_writer.Object);
        }
        [Fact]
        public void SpellAffectStrength()
        {
            _spell.DoSpell(_Spells, _player, _target, _room);

            Assert.True(_target.Attributes.Attribute[EffectLocation.Strength] > 10, "The strength attribute was not greater than ten");
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "start spell")), Times.Once);
        }

        [Fact]
        public void SpellWontFireIfRoundIsHigherThanOne()
        {
            _Spells.Rounds = 2;
    
            _spell.DoSpell(_Spells, _player, _target, _room);
            Assert.True(_Spells.Rounds == 1, "Rounds remaining should equal 1");

          
        }

        [Fact]
        public void Should_show_level_based_message_for_level_ten()
        {
            _player.Level = 1;
            _Spells.LevelBasedMessages = new LevelBasedMessages()
            {
                HasLevelBasedMessages = true,
                Ten = new Messages()
                {
                    ToPlayer = "level ten"
                }
            };

            _spell.DoSpell(_Spells, _player, _target, _room);

            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level ten")), Times.Once);

        }

        [Fact]
        public void Should_show_level_based_message_for_level_twenty()
        {
            _player.Level = 15;
            _Spells.LevelBasedMessages = new LevelBasedMessages()
            {
                HasLevelBasedMessages = true,
                Twenty = new Messages()
                {
                    ToPlayer = "level twenty",
                    ToTarget = "level twenty target",
                    ToRoom = "level twenty room"
                }
            };

            _spell.DoSpell(_Spells, _player, _target, _room);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level twenty")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level twenty target")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level twenty room")), Times.Once);


        }

        [Fact]
        public void Should_show_level_based_message_for_level_Thirty()
        {
            _player.Level = 25;
            _Spells.LevelBasedMessages = new LevelBasedMessages()
            {
                HasLevelBasedMessages = true,
                Thirty = new Messages()
                {
                    ToPlayer = "level Thirty",
                    ToTarget = "level Thirty target",
                    ToRoom = "level Thirty room"
                }
            };

            _spell.DoSpell(_Spells, _player, _target, _room);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Thirty")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Thirty target")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Thirty room")), Times.Once);
        }
        [Fact]
        public void Should_show_level_based_message_for_level_Forty()
        {
            _player.Level = 35;
            _Spells.LevelBasedMessages = new LevelBasedMessages()
            {
                HasLevelBasedMessages = true,
                Forty = new Messages()
                {
                    ToPlayer = "level forty",
                    ToTarget = "level forty target",
                    ToRoom = "level forty room"
                }
            };

            _spell.DoSpell(_Spells, _player, _target, _room);

            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level forty")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level forty target")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level forty room")), Times.Once);
        }

        [Fact]
        public void Should_show_level_based_message_for_level_Fifty()
        {
            _player.Level = 45;
            _Spells.LevelBasedMessages = new LevelBasedMessages()
            {
                HasLevelBasedMessages = true,
                Fifty = new Messages()
                {
                    ToPlayer = "level Fifty",
                    ToTarget = "level Fifty target",
                    ToRoom = "level Fifty room"
                }
            };

            _spell.DoSpell(_Spells, _player, _target, _room);

            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Fifty")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Fifty target")), Times.Once);
            _writer.Verify(w => w.WriteLine(It.Is<string>(s => s == "level Fifty room")), Times.Once);
        }
    }
}
