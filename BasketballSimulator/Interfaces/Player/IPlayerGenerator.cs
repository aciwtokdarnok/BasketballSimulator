using BasketballSimulator.Core.Enums.Players;
using BasketballSimulator.Core.Models.Players;

namespace BasketballSimulator.Core.Interfaces.Players;

public interface IPlayerGenerator
{
    Player Generate(int age, Archetype? targetArchetype);
}