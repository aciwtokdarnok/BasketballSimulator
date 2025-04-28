using BasketballSimulator.Core.Models;

namespace BasketballSimulator.Core.Interfaces;

public interface IPlayerGenerator
{
    Player Generate(byte age);
}