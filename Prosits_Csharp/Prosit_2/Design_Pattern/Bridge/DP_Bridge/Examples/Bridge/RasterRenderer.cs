namespace DP.Examples.Bridge
{
    /// <summary>
    /// Concrete implementor that performs raster (pixel) rendering.
    /// Shows how different implementors can be swapped without
    /// changing the Abstraction (Shape) hierarchy.
    /// </summary>
    public class RasterRenderer : IRenderer
    {
        public string WhatToRenderAs => "Raster";

        public string RenderCircle(float radius) => $"Drawing a {WhatToRenderAs} circle of radius {radius} (pixels)";

        public string RenderSquare(float side) => $"Drawing a {WhatToRenderAs} square of side {side} (pixels)";
    }
}
