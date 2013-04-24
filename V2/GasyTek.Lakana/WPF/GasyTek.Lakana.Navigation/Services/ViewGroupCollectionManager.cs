using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewGroupCollectionManager
    {
        private readonly ViewGroupCollection _viewGroupCollection;
        private readonly ObservableCollection<View> _viewCollection;
        private readonly ReadOnlyObservableCollection<View> _readOnlyViewCollection;

        #region Properties

        public ReadOnlyObservableCollection<View> ViewCollection
        {
            get { return _readOnlyViewCollection; }
        }

        public int NbViews
        {
            get { return _viewGroupCollection.Sum(v => v.Count); }
        }

        public ViewGroupCollection ViewGroupCollection
        {
            get { return _viewGroupCollection; }
        }

        #endregion

        #region Constructor

        internal ViewGroupCollectionManager()
        {
            _viewGroupCollection = new ViewGroupCollection();
            _viewCollection = new ObservableCollection<View>();
            _readOnlyViewCollection = new ReadOnlyObservableCollection<View>(_viewCollection);
        }

        #endregion

        #region Public methods

        public View GetActiveView()
        {
            var activeNode = GetActiveNode();
            return activeNode != null ? activeNode.Value : View.Null;
        }

        public ViewGroupNode GetActiveNode()
        {
            var activeViewGroup = GetActiveViewGroup();
            return activeViewGroup != null ? activeViewGroup.Peek() : null;
        }

        public ViewGroup GetActiveViewGroup()
        {
            if (!_viewGroupCollection.Any()) return null;
            if (!_viewGroupCollection.Last.Value.Any()) return null;
            return _viewGroupCollection.Last.Value;
        }

        #endregion

        #region Internal methods

        internal void ActivateExistingNode(ViewGroupNode node)
        {
            if (GetActiveView() != node.Value)
            {
                _viewGroupCollection.Remove(node.List);
                _viewGroupCollection.AddLast(node.List);
            }
        }

        internal void ActivateNewNode(ViewGroupNode newNode, ViewGroup ownerGroup = null)
        {
            if (ownerGroup != null)
                ownerGroup.Push(newNode);
            else
            {
                var newStack = new ViewGroup();
                newStack.Push(newNode);
                _viewGroupCollection.AddLast(newStack);
            }

            if (!newNode.Value.IsMessageBox)
                _viewCollection.Add(newNode.Value);
        }

        internal ViewGroupNode FindViewNode(string viewInstanceKey)
        {
            ViewGroupNode node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node;
            }
            throw new ViewInstanceNotFoundException(viewInstanceKey);
        }

        internal bool ContainsViewNode(string viewInstanceKey)
        {
            ViewGroupNode node;
            return TryFindViewNode(viewInstanceKey, out node);
        }

        internal bool TryFindViewNode(string viewInstanceKey, out ViewGroupNode node)
        {
            var viewInfo = new View(viewInstanceKey);
            var q = (from viewGroup in _viewGroupCollection
                     from view in viewGroup
                     where view == viewInfo
                     select viewGroup.Find(view)).ToList();
            node = q.FirstOrDefault();
            return q.Any();
        }

        internal ViewGroupNode RemoveViewNode(string viewInstanceKey)
        {
            ViewGroupNode node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                var viewGroup = node.List;
                var removedNode = viewGroup.Pop();
                removedNode.List = viewGroup;

                if (viewGroup.Count == 0)
                    _viewGroupCollection.Remove(viewGroup);

                _viewCollection.Remove(node.Value);

                return removedNode;
            }

            throw new ViewInstanceNotFoundException(viewInstanceKey);
        }

        internal bool IsTopMostView(string viewInstanceKey)
        {
            ViewGroupNode node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node.List.Peek() == node;
            }
            return false;
        }

        internal IEnumerable<View> GetNotCloseableViews()
        {
            // retrieve views that are parent of top most message box views
            var q1 = (from viewGroup in _viewGroupCollection
                      let last = viewGroup.Peek()
                      let lastPrevious = last.Previous
                      where last != null
                         && lastPrevious != null
                         && last.Value.IsMessageBox
                      select lastPrevious.Value).ToList();

            // retrieve top most views except message boxes
            var q2 = (from viewGroup in _viewGroupCollection
                      let last = viewGroup.Peek()
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
}
