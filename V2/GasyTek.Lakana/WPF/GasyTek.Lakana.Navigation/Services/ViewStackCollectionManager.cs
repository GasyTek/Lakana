using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GasyTek.Lakana.Navigation.Services
{
    public class ViewStackCollectionManager
    {
        private readonly ViewStackCollection _viewStackCollection;
        private readonly ObservableCollection<ViewInfo> _viewCollection;
        private readonly ReadOnlyObservableCollection<ViewInfo> _readOnlyViewCollection;

        #region Properties

        public ReadOnlyObservableCollection<ViewInfo> ViewCollection
        {
            get
            {
                return _readOnlyViewCollection;
            }
        }

        public int NbViews
        {
            get { return _viewStackCollection.Sum(v => v.Count); }
        }

        public ViewStackCollection ViewStackCollection
        {
            get { return _viewStackCollection; }
        }

        #endregion

        #region Constructor

        internal ViewStackCollectionManager()
        {
            _viewStackCollection = new ViewStackCollection();
            _viewCollection = new ObservableCollection<ViewInfo>();
            _readOnlyViewCollection = new ReadOnlyObservableCollection<ViewInfo>(_viewCollection);
        }

        #endregion

        #region Public methods

        public ViewInfo GetActiveView()
        {
            var activeNode = GetActiveNode();
            return activeNode != null ? activeNode.Value : ViewInfo.Null;
        }

        public LinkedListNode<ViewInfo> GetActiveNode()
        {
            var activeStack = GetActiveStack();
            return activeStack != null ? activeStack.Last : null;
        }

        public LinkedList<ViewInfo> GetActiveStack()
        {
            if (!_viewStackCollection.Any()) return null;
            if (!_viewStackCollection.Last.Value.Any()) return null;
            return _viewStackCollection.Last.Value;
        }

        #endregion

        #region Internal methods

        internal void ActivateExistingNode(LinkedListNode<ViewInfo> node)
        {
            if (GetActiveView() != node.Value)
            {
                _viewStackCollection.Remove((ViewStack)node.List);
                _viewStackCollection.AddLast((ViewStack)node.List);
            }
        }

        internal void ActivateNewNode(LinkedListNode<ViewInfo> newNode, ViewStack ownerStack = null)
        {
            if (ownerStack != null)
                ownerStack.AddLast(newNode);
            else
            {
                var newStack = new ViewStack();
                newStack.AddLast(newNode);
                _viewStackCollection.AddLast(newStack);
            }

            if (!newNode.Value.IsMessageBox)
                _viewCollection.Add(newNode.Value);
        }

        internal LinkedListNode<ViewInfo> FindViewNode(string viewInstanceKey)
        {
            LinkedListNode<ViewInfo> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node;
            }
            throw new ViewInstanceNotFoundException(viewInstanceKey);
        }

        internal bool ContainsViewNode(string viewInstanceKey)
        {
            LinkedListNode<ViewInfo> node;
            return TryFindViewNode(viewInstanceKey, out node);
        }

        internal bool TryFindViewNode(string viewInstanceKey, out LinkedListNode<ViewInfo> node)
        {
            var viewInfo = new ViewInfo(viewInstanceKey);
            var q = (from stack in _viewStackCollection
                     from v in stack
                     where v == viewInfo
                     select stack.Find(v)).ToList();
            node = q.FirstOrDefault();
            return q.Any();
        }

        internal LinkedListNode<ViewInfo> RemoveViewNode(string viewInstanceKey)
        {
            LinkedListNode<ViewInfo> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                var stack = (ViewStack)node.List;
                stack.Remove(node);

                if (stack.Count == 0)
                    _viewStackCollection.Remove(stack);

                _viewCollection.Remove(node.Value);
            }

            return node;
        }

        internal bool IsTopMostView(string viewInstanceKey)
        {
            LinkedListNode<ViewInfo> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node.List.Last.Value == node.Value;
            }
            return false;
        }

        internal IEnumerable<ViewInfo> GetNotCloseableViews()
        {
            // retrieve views that are parent of top most message box views
            var q1 = (from vs in _viewStackCollection
                      let last = vs.Last
                      let lastPrevious = vs.Last.Previous
                      where last != null
                         && lastPrevious != null
                         && last.Value.IsMessageBox
                      select lastPrevious.Value).ToList();

            // retrieve top most views except message boxes
            var q2 = (from vs in _viewStackCollection
                      let last = vs.Last
                      where last != null
                      select last.Value).Except(q1);

            // retrieve views/viewmodels that are not closeable
            return from vi in q1.Concat(q2)
                   let v = vi.ViewInstance as ICloseable
                   let vm = vi.ViewInstance.DataContext as ICloseable
                   where (v != null && v.CanClose() == false) || (vm != null && vm.CanClose() == false)
                   select vi;
        }


        #endregion
    }

    public class ViewStack : LinkedList<ViewInfo>
    {

    }

    public class ViewStackCollection : LinkedList<ViewStack>
    {

    }
}
