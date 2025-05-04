namespace BasketballSimulator.Core.Interfaces.Player;

public interface IPlayerGenerator
{
    Models.Player.Player Generate(byte age);
}