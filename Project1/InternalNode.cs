using System;

namespace QuadtreeConsole
{
    public class InternalNode : Node
    {
        public Node[] Children { get; set; } = new Node[4]; // Four quadrants

        public InternalNode(Rectangle space) : base(space) { }

        public override void Dump(int level)
        {
            string indent = new string('\t', level);
            Console.WriteLine($"{indent}Internal Node: {Space}");

            foreach (var child in Children)
            {
                child?.Dump(level + 1);
            }
        }
    }
}