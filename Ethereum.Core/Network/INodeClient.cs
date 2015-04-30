using System.Threading.Tasks;

namespace Ethereum.Network
{
    public interface INodeClient
    {
        Task Start();

        void Dispose();
    }
}