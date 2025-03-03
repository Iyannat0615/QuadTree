namespace QuadtreeConsole
{
    public abstract class Node
    {
        public Rectangle Space { get; set; } // Represents the node's area

        public Node(Rectangle space)
        {
            Space = space;
        }

        public abstract void Dump(int level);
    }
}