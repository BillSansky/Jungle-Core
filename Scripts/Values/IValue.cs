namespace Jungle.Values
{
    /// <summary>
    /// Provides access to a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value returned by the provider.</typeparam>
    public interface IValue<out T>
    {
        /// <summary>
        /// Gets the value represented by the provider.
        /// </summary>
        /// <returns>The value produced by the provider.</returns>
        T GetValue();
    }
}
