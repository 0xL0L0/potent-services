namespace Potency.Services.Runtime.Utils.Factories
{
    public interface IFactory<out T, in P>
    {
        T Create(P createParams);
    }
}