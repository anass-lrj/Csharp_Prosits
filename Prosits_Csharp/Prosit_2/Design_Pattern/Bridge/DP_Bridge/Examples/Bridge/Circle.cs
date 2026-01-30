namespace DP.Examples.Bridge
{
    /// <summary>
    /// RefinedAbstraction: a concrete Shape that uses the implementor
    /// to perform rendering. The shape logic (e.g., resizing) is kept
    /// separate from the rendering details.
    /// </summary>
    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override string Draw() => renderer.RenderCircle(radius);

        public override void Resize(float factor) => radius *= factor;
    }
}
