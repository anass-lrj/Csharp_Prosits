namespace PizzeriaMVC.Models
{
    public class Pizza
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public Pizza(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public override string ToString() => $"[{Id}] {Name} - {Price:C}";
    }
}
