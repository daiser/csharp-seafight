using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    class ShipBlueprint : IBlueprint
    {
        public int Size { get; private set; }
        public ShipBlueprint(in int size)
        {
            Size = size;
        }
    }
}
