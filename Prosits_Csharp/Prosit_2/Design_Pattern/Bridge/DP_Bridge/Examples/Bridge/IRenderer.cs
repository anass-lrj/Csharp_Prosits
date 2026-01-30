namespace DP.Examples.Bridge
{
    /// <summary>
    /// Implementor role in the Bridge pattern.
    /// Defines the low-level rendering operations that concrete
    /// implementors (e.g., VectorRenderer, RasterRenderer) provide.
    /// The Abstraction (Shape) delegates work to this interface.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>Describes the rendering approach (for demo output).</summary>
        string WhatToRenderAs { get; }

        /// <summary>Render a circle with the given radius.</summary>
        string RenderCircle(float radius);

        /// <summary>Render a square with the given side length.</summary>
        string RenderSquare(float side);
    }
}
