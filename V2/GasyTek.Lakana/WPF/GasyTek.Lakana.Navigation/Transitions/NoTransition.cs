namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Do not animate transition.
    /// </summary>
    public static class NoTransition
    {
        public static TransitionAnimation Create()
        {
            return new TransitionAnimation();
        }
    }
}
