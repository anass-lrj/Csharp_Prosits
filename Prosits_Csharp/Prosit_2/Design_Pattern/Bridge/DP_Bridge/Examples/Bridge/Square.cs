namespace DP.Examples.Bridge
{
    /// <summary>
    /// RefinedAbstraction: a concrete Shape representing a square.
    /// Demonstrates separation of geometric behaviour from rendering.
    /// </summary>
    public class Square : Shape
    {
        private float side;

        public Square(IRenderer renderer, float side) : base(renderer)
        {
            this.side = side;
        }

        public override string Draw() => renderer.RenderSquare(side);

        public override void Resize(float factor) => side *= factor;
    }
}
