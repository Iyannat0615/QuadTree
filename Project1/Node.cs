using System;
using System.Collections.Generic;

namespace QuadtreeConsole
{
    public class Quadtree
    {
        private Node root;
        private const int InitialSize = 100;

        public Quadtree()
        {
            // Initial space of 100x100 centered on 0,0
            root = new LeafNode(new Rectangle(-InitialSize / 2.0, -InitialSize / 2.0, InitialSize, InitialSize));
        }

        public void Insert(Rectangle rect)
        {
            Insert(root, rect);
        }

        private void Insert(Node node, Rectangle rect)
        {
            if (node is LeafNode leaf)
            {
                if (leaf.IsFull)
                {
                    Split(leaf);
                    Insert(node, rect); // Retry insertion after splitting
                }
                else
                {
                    leaf.Rectangles.Add(rect);
                } 
            }
            else if (node is InternalNode internalNode)
            {
                // Determine which quadrant the rectangle belongs to
                int quadrant = GetQuadrant(internalNode, rect);
                if (quadrant == -1) return; // Rectangle does not fit in any quadrant

                Insert(internalNode.Children[quadrant], rect);
            }
        }

        private void Split(LeafNode leaf)
        {
            Rectangle space = leaf.Space;
            double subWidth = space.Width / 2.0;
            double subLength = space.Length / 2.0;

            InternalNode internalNode = new InternalNode(space);

            // Create four new quadrants
            internalNode.Children[0] = new LeafNode(new Rectangle(space.X, space.Y, subLength, subWidth)); // Bottom-Left
            internalNode.Children[1] = new LeafNode(new Rectangle(space.X + subLength, space.Y, subLength, subWidth)); // Bottom-Right
            internalNode.Children[2] = new LeafNode(new Rectangle(space.X, space.Y + subWidth, subLength, subWidth)); // Top-Left
            internalNode.Children[3] = new LeafNode(new Rectangle(space.X + subLength, space.Y + subWidth, subLength, subWidth)); // Top-Right

            // Redistribute existing rectangles
            foreach (var rect in leaf.Rectangles)
            {
                int quadrant = GetQuadrant(internalNode, rect);
                if (quadrant != -1)
                {
                    ((LeafNode)internalNode.Children[quadrant]).Rectangles.Add(rect);
                }
            }

            leaf.Rectangles.Clear(); // Clear rectangles from the original leaf

            // Replace the leaf node with the internal node
            if (root == leaf)
            {
                root = internalNode;
            }
            else
            {
                // Find the parent of the leaf node and update its child reference
                // This part requires you to maintain parent references or search for the parent
                // For simplicity, we assume the root is always the parent (for now)
                root = internalNode; // Replace root with internal node
            }
        }

        private int GetQuadrant(InternalNode node, Rectangle rect)
        {
            double midX = node.Space.X + node.Space.Length / 2.0;
            double midY = node.Space.Y + node.Space.Width / 2.0;

            if (rect.X < midX && rect.Y < midY) return 0; // Bottom-Left
            if (rect.X >= midX && rect.Y < midY) return 1; // Bottom-Right
            if (rect.X < midX && rect.Y >= midY) return 2; // Top-Left
            if (rect.X >= midX && rect.Y >= midY) return 3; // Top-Right

            return -1; // Doesn't fit in any quadrant
        }

        public Rectangle Find(double x, double y)
        {
            return Find(root, x, y);
        }

        private Rectangle Find(Node node, double x, double y)
        {
            if (node is LeafNode leaf)
            {
                return leaf.Rectangles.Find(r => r.X == x && r.Y == y);
            }
            else if (node is InternalNode internalNode)
            {
                double midX = internalNode.Space.X + internalNode.Space.Length / 2.0;
                double midY = internalNode.Space.Y + internalNode.Space.Width / 2.0;

                if (x < midX && y < midY) return Find(internalNode.Children[0], x, y); // Bottom-Left
                if (x >= midX && y < midY) return Find(internalNode.Children[1], x, y); // Bottom-Right
                if (x < midX && y >= midY) return Find(internalNode.Children[2], x, y); // Top-Left
                if (x >= midX && y >= midY) return Find(internalNode.Children[3], x, y); // Top-Right
            }

            return null;
        }

        public bool Delete(double x, double y)
        {
            return Delete(root, x, y);
        }

        private bool Delete(Node node, double x, double y)
        {
            if (node is LeafNode leaf)
            {
                return leaf.Rectangles.RemoveAll(r => r.X == x && r.Y == y) > 0;
            }
            else if (node is InternalNode internalNode)
            {
                double midX = internalNode.Space.X + internalNode.Space.Length / 2.0;
                double midY = internalNode.Space.Y + internalNode.Space.Width / 2.0;

                if (x < midX && y < midY) return Delete(internalNode.Children[0], x, y); // Bottom-Left
                if (x >= midX && y < midY) return Delete(internalNode.Children[1], x, y); // Bottom-Right
                if (x < midX && y >= midY) return Delete(internalNode.Children[2], x, y); // Top-Left
                if (x >= midX && y >= midY) return Delete(internalNode.Children[3], x, y); // Top-Right
            }

            return false;
        }

        public bool Update(double x, double y, double newLength, double newWidth)
        {
            return Update(root, x, y, newLength, newWidth);
        }

        private bool Update(Node node, double x, double y, double newLength, double newWidth)
        {
            if (node is LeafNode leaf)
            {
                Rectangle rect = leaf.Rectangles.Find(r => r.X == x && r.Y == y);
                if (rect != null)
                {
                    rect.Length = newLength;
                    rect.Width = newWidth;
                    return true;
                }
                return false;
            }
            else if (node is InternalNode internalNode)
            {
                double midX = internalNode.Space.X + internalNode.Space.Length / 2.0;
                double midY = internalNode.Space.Y + internalNode.Space.Width / 2.0;

                if (x < midX && y < midY) return Update(internalNode.Children[0], x, y, newLength, newWidth); // Bottom-Left
                if (x >= midX && y < midY) return Update(internalNode.Children[1], x, y, newLength, newWidth); // Bottom-Right
                if (x < midX && y >= midY) return Update(internalNode.Children[2], x, y, newLength, newWidth); // Top-Left
                if (x >= midX && y >= midY) return Update(internalNode.Children[3], x, y, newLength, newWidth); // Top-Right
            }

            return false;
        }

        public void Dump()
        {
            Dump(root, 0);
        }

        private void Dump(Node node, int level)
        {
            node.Dump(level);
        }
    }
}