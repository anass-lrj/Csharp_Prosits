using System;

namespace DP.Examples.Bridge
{
    public static class BridgeDemo    {
        public static void Run()
        {
            // Short demonstration that shows how the Abstraction (Shape)
            // can vary independently from the Implementor (IRenderer).
            Console.WriteLine("--- Bridge Pattern Demo ---");

            // Create two different implementors
            var vector = new VectorRenderer();
            var raster = new RasterRenderer();

            // Create a Circle that uses vector rendering
            var circle = new Circle(vector, 5);
            Console.WriteLine(circle.Draw());
            // Resize only affects the abstraction's state (radius)
            circle.Resize(2);
            Console.WriteLine(circle.Draw());

            // Create a Square using raster rendering
            var square = new Square(raster, 3);
            Console.WriteLine(square.Draw());
            // Change shape size without touching renderer implementation
            square.Resize(3);
            Console.WriteLine(square.Draw());

            Console.WriteLine("--- End Demo ---");
        }
    }
}