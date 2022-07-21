using SeaFight.Board;

namespace SeaFight
{
    interface ITarget
    {
        ShotEffect TakeShot(Point coord);
    }
}