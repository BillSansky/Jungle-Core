using UnityEngine;

/// <summary>
/// Represents a context source capable of providing a <see cref="GameObject"/> reference.
/// </summary>
public interface IGameObjectContext
{
    /// <summary>
    /// Retrieves the <see cref="GameObject"/> associated with the context.
    /// </summary>
    /// <returns>The resolved <see cref="GameObject"/>, or <c>null</c> when unavailable.</returns>
    GameObject GetGameObject();
}
