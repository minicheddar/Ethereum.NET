using System.Threading.Tasks;

namespace Ethereum.Network
{
    public interface INodeServer
    {
        Task Start();

        void Dispose();
    }
}