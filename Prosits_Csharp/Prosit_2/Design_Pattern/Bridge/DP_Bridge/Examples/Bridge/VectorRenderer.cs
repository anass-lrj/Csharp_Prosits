namespace DP.Examples.Bridge
{
    /// <summary>
    /// Concrete implementor that performs vector-style rendering.
    /// In the Bridge pattern this class provides a specific
    /// implementation of the low-level rendering operations.
    /// </summary>
    public class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "Vector";

        public string RenderCircle(float radius) => $"Drawing a {WhatToRenderAs} circle of radius {radius}";

        public string RenderSquare(float side) => $"Drawing a {WhatToRenderAs} square of side {side}";
    }
}
