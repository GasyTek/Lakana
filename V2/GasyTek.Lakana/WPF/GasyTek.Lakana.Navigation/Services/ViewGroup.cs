using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewGroup : IEnumerable<View>
    {
        #region Fields

        private readonly Stack<ViewGroupNode> _internalStack;

        #endregion

        #region Properties

        public int Count
        {
            get { return _internalStack.Count; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroup" /> class.
        /// </summary>
        public ViewGroup()
        {
            _internalStack = new Stack<ViewGroupNode>();
        }

        #endregion

        #region Public methods

        public ViewGroupNode Push(ViewGroupNode newNode)
        {
            if (_internalStack.Count > 0)
            {
                var previousNode = _internalStack.Peek();
                newNode.Previous = previousNode;
            }
            newNode.List = this;
            _internalStack.Push(newNode);
            return newNode;
        }

        public ViewGroupNode Push(View view)
        {
            var newNode = new ViewGroupNode(this, view);
            return Push(newNode);
        }

        public ViewGroupNode Peek()
        {
            return _internalStack.Peek();
        }

        public ViewGroupNode Pop()
        {
            var lastNode = _internalStack.Pop();
            if (lastNode != null) { lastNode.Previous = null; }
            return lastNode;
        }

        public ViewGroupNode Find(View view)
        {
            return _internalStack.FirstOrDefault(vg => vg.Value == view);
        }

        public void Clear()
        {
            _internalStack.Clear();
        }

        #endregion

        #region IEnumerable implementations

        public IEnumerator<View> GetEnumerator()
        {
            return new ViewGroupEnumerator(_internalStack.Reverse().GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private class ViewGroupEnumerator

        private class ViewGroupEnumerator : IEnumerator<View>
        {
            private readonly IEnumerator<ViewGroupNode> _ownerEnumerator;

            public ViewGroupEnumerator(IEnumerator<ViewGroupNode> ownerEnumerator)
            {
                _ownerEnumerator = ownerEnumerator;
            }

            public void Dispose()
            {
                _ownerEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _ownerEnumerator.MoveNext();
            }

            public void Reset()
            {
                _ownerEnumerator.Reset();
            }

            public View Current
            {
                get { return _ownerEnumerator.Current.Value; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        #endregion
    }
}