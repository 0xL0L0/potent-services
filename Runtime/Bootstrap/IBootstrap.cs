using System.Threading.Tasks;

namespace Potency.Services.Runtime.Bootstrap
{
    /// <summary>
    /// Entry point of the game.
    /// Object inheriting from this should be placed on the bootstrapping scene, and handle instantiating
    /// services, loading configs, and setting up the state machine
    /// </summary>
    public interface IBootstrap
    {
        /// <summary>
        /// Initializes the game with necessary services, configs, and state machine
        /// </summary>
        Task Bootstrap();
    }
}
