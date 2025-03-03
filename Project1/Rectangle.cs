namespace QuadtreeConsole
{
    public class Rectangle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        public Rectangle(double x, double y, double length, double width)
        {
            X = x;
            Y = y;
            Length = length;
            Width = width;
        }

        public override string ToString()
        {
            return $"{Length}x{Width} at ({X}, {Y})";
        }
    }
}