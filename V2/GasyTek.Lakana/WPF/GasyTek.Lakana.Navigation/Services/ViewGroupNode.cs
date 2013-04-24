namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewGroupNode
    {
        #region Fields

        private ViewGroupNode _previous;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public View Value { get; private set; }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public ViewGroup List { get; internal set; }

        /// <summary>
        /// Gets the previous.
        /// </summary>
        /// <value>
        /// The previous.
        /// </value>
        public ViewGroupNode Previous
        {
            get { return _previous; }
            internal set
            {
                _previous = value;
                if (_previous != null)
                    _previous.Next = this;
            }
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <value>
        /// The next.
        /// </value>
        public ViewGroupNode Next
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroupNode" /> class.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="value">The value.</param>
        public ViewGroupNode(ViewGroup list, View value)
        {
            List = list;
            Value = value;
        }

        #endregion
    }
}