using System;
using System.Collections.Generic;

namespace QuadtreeConsole
{
    public class LeafNode : Node
    {
        private const int MaxRectangles = 5;
        public List<Rectangle> Rectangles { get; set; } = new List<Rectangle>();

        public LeafNode(Rectangle space) : base(space) { }

        public bool IsFull => Rectangles.Count >= MaxRectangles;

        public override void Dump(int level)
        {
            string indent = new string('\t', level);
            Console.WriteLine($"{indent}Leaf Node: {Space}");

            foreach (var rect in Rectangles)
            {
                Console.WriteLine($"{indent}\tRectangle: {rect}");
            }
        }
    }
}