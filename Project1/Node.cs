using System;
using System.Collections.Generic;

namespace QuadtreeConsole
{
    public abstract class Node
    {
        public Rectangle Space { get; set; } // Represents the node's area

        public Node(Rectangle space)
        {
            Space = space;
        }

        public abstract void Dump(int level); // Abstract method to dump node info
    }

    public class LeafNode : Node
    {
        public List<Rectangle> Objects { get; set; }

        public LeafNode(Rectangle space) : base(space)
        {
            Objects = new List<Rectangle>();
        }

        // Insert a rectangle into this leaf node
        public void Insert(Rectangle rect)
        {
            if (Space.Contains(rect))
            {
                Objects.Add(rect);
            }
        }

        // Delete a rectangle from this leaf node
        public void Delete(Rectangle rect)
        {
            Objects.Remove(rect);
        }

        // Dump the leaf node's content
        public override void Dump(int level)
        {
            Console.WriteLine(new string(' ', level * 2) + $"LeafNode - ({Space.X}, {Space.Y}) - {Space.Width}x{Space.Height}");
            foreach (var obj in Objects)
            {
                Console.WriteLine(new string(' ', (level + 1) * 2) + $"rectangle - ({obj.X}, {obj.Y}) - {obj.Width}x{obj.Height}");
            }
        }
    }

    public class InternalNode : Node
    {
        public Node[] Children { get; set; }

        public InternalNode(Rectangle space) : base(space)
        {
            Children = new Node[4]; // 4 quadrants (top-left, top-right, bottom-left, bottom-right)
        }

        // Split the internal node into 4 child nodes
        public void Split()
        {
            float halfWidth = Space.Width / 2;
            float halfHeight = Space.Height / 2;

            Children[0] = new LeafNode(new Rectangle(Space.X, Space.Y, halfWidth, halfHeight)); // Top-left
            Children[1] = new LeafNode(new Rectangle(Space.X + halfWidth, Space.Y, halfWidth, halfHeight)); // Top-right
            Children[2] = new LeafNode(new Rectangle(Space.X, Space.Y + halfHeight, halfWidth, halfHeight)); // Bottom-left
            Children[3] = new LeafNode(new Rectangle(Space.X + halfWidth, Space.Y + halfHeight, halfWidth, halfHeight)); // Bottom-right
        }

        // Insert a rectangle into the internal node or its children
        public void Insert(Rectangle rect)
        {
            if (Children[0] == null) // If the node isn't split, split it
            {
                Split();
            }

            foreach (var child in Children)
            {
                if (child.Space.Contains(rect))
                {
                    if (child is LeafNode leafNode)
                    {
                        leafNode.Insert(rect);
                    }
                    else
                    {
                        ((InternalNode)child).Insert(rect);
                    }
                }
            }
        }

        // Delete a rectangle from the internal node or its children
        public void Delete(Rectangle rect)
        {
            foreach (var child in Children)
            {
                if (child.Space.Contains(rect))
                {
                    if (child is LeafNode leafNode)
                    {
                        leafNode.Delete(rect);
                    }
                    else
                    {
                        ((InternalNode)child).Delete(rect);
                    }
                }
            }
        }

        // Dump the internal node's content and all child nodes
        public override void Dump(int level)
        {
            Console.WriteLine(new string(' ', level * 2) + $"InternalNode - ({Space.X}, {Space.Y}) - {Space.Width}x{Space.Height}");

            foreach (var child in Children)
            {
                child.Dump(level + 1);
            }
        }
    }
}
