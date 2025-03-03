using System;
using System.IO;

namespace QuadtreeConsole
{
    internal class Quadtree
    {
        private InternalNode root;

        public Quadtree()
        {
            // Initialize the root node with the quadtree's space (in this case, -50 to 50 for both x and y)
            root = new InternalNode(new Rectangle(-50, -50, 100, 100));
        }

        public void Insert(Rectangle rect)
        {
            root.Insert(rect);
        }

        public bool Delete(double x, double y)
        {
            Rectangle rect = Find(x, y);
            if (rect != null)
            {
                root.Delete(rect);
                return true;
            }
            return false;
        }

        public Rectangle Find(double x, double y)
        {
            return FindInNode(root, x, y);
        }

        // Recursively searches for a rectangle in the quadtree
        private Rectangle FindInNode(Node node, double x, double y)
        {
            if (node is LeafNode leafNode)
            {
                foreach (var rect in leafNode.Objects)
                {
                    if (rect.X == x && rect.Y == y)
                    {
                        return rect;
                    }
                }
            }
            else if (node is InternalNode internalNode)
            {
                foreach (var child in internalNode.Children)
                {
                    if (child.Space.Contains(x, y))
                    {
                        return FindInNode(child, x, y);
                    }
                }
            }
            return null;
        }

        public bool Update(double x, double y, double newLength, double newWidth)
        {
            Rectangle rect = Find(x, y);
            if (rect != null)
            {
                rect.Length = newLength;
                rect.Width = newWidth;
                return true;
            }
            return false;
        }

        public void Dump()
        {
            root.Dump(0);
        }
    }
}
