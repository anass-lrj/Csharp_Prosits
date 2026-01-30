namespace DP.Examples.Bridge
{
    /// <summary>
    /// Abstraction role in the Bridge pattern.
    /// Keeps a reference to an <see cref="IRenderer" /> (the implementor)
    /// and delegates primitive operations to it. Concrete shapes extend
    /// this class and provide higher-level behaviour built on top of the
    /// implementor operations.
    /// </summary>
    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        /// <summary>Draw the shape by delegating to the implementor.</summary>
        public abstract string Draw();

        /// <summary>Change shape size; implemented by refined abstractions.</summary>
        public abstract void Resize(float factor);
    }
}
