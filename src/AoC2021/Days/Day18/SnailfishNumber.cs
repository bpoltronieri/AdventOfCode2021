namespace AoC2021.Days.Day18Utils
{
    class SnailfishNumber
    {
        public int? Value { get; set; }
        public SnailfishNumber Left { get; set; }
        public SnailfishNumber Right { get; set; }

        public SnailfishNumber Parent { get; set; }

        // binary tree
        public SnailfishNumber(int value)
        {
            Value = value;
        }

        public SnailfishNumber(SnailfishNumber left, SnailfishNumber right)
        {
            Left = left;
            Right = right;
            left.Parent = this;
            right.Parent = this;
        }

        public SnailfishNumber(string s, int start_index)
        {
            if (s[start_index] != '[') // regular number
            {
                var i = start_index;
                while (char.IsDigit(s[i])) i++;
                Value = int.Parse(s.Substring(start_index, i - start_index));
            }
            else // pair of snailfish numbers
            {
                Left = new SnailfishNumber(s, start_index + 1);
                Left.Parent = this;

                // find right element after comma that isn't in another pair
                var i = start_index + 1;
                var depth = 0;
                while (!(s[i] == ',' && depth == 0))
                {
                    if (s[i] == '[') 
                        depth++;
                    if (s[i] == ']')
                        depth--;
                    i += 1;
                }
                Right = new SnailfishNumber(s, i + 1);
                Right.Parent = this;
            }
        }

        public static SnailfishNumber operator +(SnailfishNumber a, SnailfishNumber b)
        {
            // returns unreduced sum of two snailfish numbers
            // careful that nodes of the resulting sum will point to the same nodes as the original trees
            return new SnailfishNumber(a, b);
        }

        public bool IsRegularNumber()
        {
            return Left == null; // if left is null then so is right, and value isn't null
        }
        public bool IsPair()
        {
            return Left != null;
        }

        public bool IsPairOfValues()
        {
            return IsPair() 
                && Left.IsRegularNumber() 
                && Right.IsRegularNumber();
        }

        public int Magnitude()
        {
            if (this.IsRegularNumber())
                return (int)Value;
            
            return 3 * Left.Magnitude() + 2 * Right.Magnitude();
        }

        public void Reduce()
        {
            var done = false; // done if nothing to split or explode
            while (!done)
            {
                var splitOrExploded = false;
                // look left-to-right for pair that is enclosed in 4 or more pairs
                var (node, depth) = this.LeftmostNode();
                while (node != null)
                {
                    if (node.IsPairOfValues() && depth >= 4)
                    {
                        // explode
                        var leftNode = node.NextLeftValue();
                        while (leftNode != null && !leftNode.IsRegularNumber())
                            leftNode = leftNode.NextLeftValue();
                        if (leftNode != null)
                            leftNode.Value += node.Left.Value;

                        var (rightNode, dum) = node.NextRight();
                        while (rightNode != null && !rightNode.IsRegularNumber())
                            (rightNode, dum) = rightNode.NextRight();
                        if (rightNode != null)
                            rightNode.Value += node.Right.Value;

                        node.Left = node.Right = null;
                        node.Value = 0;

                        splitOrExploded = true;
                        break;
                    }
                    var depthChange = 0;
                    (node, depthChange) = node.NextRight();
                    depth += depthChange;
                }
                if (splitOrExploded) continue;

                // look left-to-right for value that is 10 or greater
                (node, depth) = this.LeftmostNode();
                while (node != null)
                {
                    if (node.IsRegularNumber() && node.Value >= 10)
                    {
                        // split
                        int halfRoundedDown = ((int)node.Value) / 2;
                        node.Left = new SnailfishNumber(halfRoundedDown);
                        node.Left.Parent = node;
                        node.Right = new SnailfishNumber((int)node.Value - halfRoundedDown);
                        node.Right.Parent = node;

                        splitOrExploded = true;
                        break;
                    }
                    (node, depth) = node.NextRight();
                }
                if (!splitOrExploded)
                    done = true;
            }
        }

        private (SnailfishNumber, int) LeftmostNode()
        {
            // returns left-most child node and its depth
            var node = this;
            var depth = 0;
            while (node.IsPair())
            {
                node = node.Left;
                depth += 1;
            }
            return (node, depth);
        }

        private (SnailfishNumber, int) NextRight()
        {
            // returns next node to the right, and the
            // depth change needed to reach it
            if (this.Parent == null)
                return (null, 0);

            if (this.IsRegularNumber() && this == this.Parent.Right)
                return (this.Parent, -1);
            
            var depthUp = 0;
            var node = this;
            while (node == node.Parent.Right)
            {
                node = node.Parent;
                depthUp += 1;
                if (node.Parent == null)
                    return (null, 0);
            }
            var (next, depthDown) = node.Parent.Right.LeftmostNode();
            return (next, depthDown - depthUp);
        }

        private SnailfishNumber RightmostNode()
        {
            // returns right-most child node
            var node = this;
            while (node.IsPair())
                node = node.Right;
            return node;
        }

        private SnailfishNumber NextLeftValue()
        {
            // returns next node to the left
            if (this.Parent == null)
                return null;

            var node = this;
            while (node == node.Parent.Left)
            {
                node = node.Parent;
                if (node.Parent == null)
                    return null;
            }
            return node.Parent.Left.RightmostNode();
        }

        public override string ToString()
        {
            if (this.IsRegularNumber())
                return Value.ToString();

            return "[" + this.Left.ToString() + "," + this.Right.ToString() + "]";
        }
       
    }
}