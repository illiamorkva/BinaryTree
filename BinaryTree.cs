namespace Tree
{
    public class Node<TValue>
    {
        public TValue value;
        public Node<TValue> left;
        public Node<TValue> right;
 
        public Node(TValue initial)
        {
            value = initial;
            left = null;
            right = null;
        }
    }
 
    public class Tree<T> : ICollection<T>
    {
        protected IComparer<T> comparer;
        Node<T> top;
 
        public Tree()
            : this(Comparer<T>.Default)
        {
            top = null;
        }
 
        public Tree(T initial)
            : this(Comparer<T>.Default)
        {
            top = new Node<T>(initial);
        }
 
        public Tree(IComparer<T> defaultComparer)
        {
            if (defaultComparer == null)
                throw new ArgumentNullException("Comparer NULL");
            comparer = defaultComparer;
        }
 
        public void Add(T value)
        {
            if (top == null)
            {
                Node<T> NewNode = new Node<T>(value);
                top = NewNode;
                Count++;
                return;
            }
            Node<T> currentNode = top;
 
            bool added = false;
            do
            {
                if (comparer.Compare(value, currentNode.value) < 0)
                {
                    // go left
                    if (currentNode.left == null)
                    {
                        // add element
                        Node<T> NewNode = new Node<T>(value);
                        currentNode.left = NewNode;
                        added = true;
                    }
                    else
                    {
                        currentNode = currentNode.left;
                    }
                }
                if (comparer.Compare(value, currentNode.value) >= 0)
                {
                    if (currentNode.right == null)
                    {
                        Node<T> NewNode = new Node<T>(value);
                        currentNode.right = NewNode;
                        added = true;
                    }
                    else
                    {
                        currentNode = currentNode.right;
                    }
                }
            }
            while (!added);
            Count++;
        }
 
        public void AddRc(T value)
        {
            // add recurssion
            AddR(ref top, value);
 
        }
 
        private void AddR(ref Node<T> N, T value)
        {
            // private recursive search for where to add the new node
            if (N == null)
            {
                Node<T> NewNode = new Node<T>(value);
                N = NewNode;
                Count++;
                return;
            }
            if (comparer.Compare(value, N.value) < 0)
            {
                AddR(ref N.left, value);
                return;
            }
            if (comparer.Compare(value, N.value) >= 0)
            {
                AddR(ref N.right, value);
                return;
            }
        }
 
        public void Print(Node<T> N, ref string s)
        {
            if (N == null)
            {
                N = top;
            }
            if (N.left != null)
            {
                Print(N.left, ref s);
                s = s + N.value.ToString().PadLeft(3);
            }
            else
            {
                s = s + N.value.ToString().PadLeft(3);
            }
            if (N.right != null)
            {
                Print(N.right, ref s);
            }
        }
        public bool Remove(T value)
        {
            if (top == null)
                return false;
            Node<T> currentNode = top, parent = null;
            do
            {
                if (comparer.Compare(value, currentNode.value) < 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.left;
                }
                else if (comparer.Compare(value, currentNode.value) > 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.right;
                }
                if (currentNode == null)
                    return false;
            }
            while (comparer.Compare(value, currentNode.value) != 0);
 
            if (currentNode.right == null)
            {
                if (currentNode == top)
                    top = currentNode.left;
                else
                {
                    if (comparer.Compare(parent.value, currentNode.value) > 0)
                        parent.left = currentNode.left;
                    else
                        parent.right = currentNode.left;
                }
            }
            else if (currentNode.right.left == null)
            {
                currentNode.right.left = currentNode.left;
                if (currentNode == top)
                    top = currentNode.right;
                else
                {
                    if (comparer.Compare(parent.value, currentNode.value) > 0)
                        parent.left = currentNode.right;
                    else
                        parent.right = currentNode.right;
                }
            }
            else
            {
                Node<T> min = currentNode.right.left, prev = currentNode.right;
                while (min.left != null)
                {
                    prev = min;
                    min = min.left;
                }
                prev.left = min.right;
                min.left = currentNode.left;
                min.right = currentNode.right;
 
                if (currentNode == top)
                    top = min;
                else
                {
                    if (comparer.Compare(parent.value, currentNode.value) > 0)
                        parent.left = min;
                    else
                        parent.right = min;
                }
            }
            Count--;
            return true;
        }
 
        public T Min()
        {
            if (top == null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }
            else
            {
                Node<T> currentNode = top;
                while (currentNode.left != null)
                {
                    currentNode = currentNode.left;
                }
 
                return currentNode.value;
            }
        }
        public void Clear()
        {
            top = null;
            Count = 0;
        }
 
        public bool Contains(T value)
        {
            Node<T> currentvalue = top;
 
            while (currentvalue != null)
            {
                if (comparer.Compare(value, currentvalue.value) == 0)
                {
                    return true;
                }
                else if (comparer.Compare(value, currentvalue.value) < 0)
                {
                    currentvalue = currentvalue.left;
                }
                else
                {
                    currentvalue = currentvalue.left;
                }
            }
            return false;
        }
        public T Max()
        {
            if (top == null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }
            else
            {
                Node<T> currentNode = top;
                while (currentNode.right != null)
                {
                    currentNode = currentNode.right;
                }
                return currentNode.value;
            }
        }
 
        public int Count
        {
            get;
            protected set;
        }
        public IEnumerable<T> Inorder()
        {
            if (top == null)
            {
                yield break;
            }
 
            var stack = new Stack<Node<T>>();
            var node = top;
 
            while (stack.Count > 0 || node != null)
            {
                if (node == null)
                {
                    node = stack.Pop();
                    yield return node.value;
                    node = node.right;
                }
                else
                {
                    stack.Push(node);
                    node = node.left;
                }
            }
        }
 
        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
 
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var value in this)
                array[arrayIndex++] = value;
        }
 
        public IEnumerator<T> GetEnumerator()
        {
            return Inorder().GetEnumerator();
        }
        // Äëÿ ICollection
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}