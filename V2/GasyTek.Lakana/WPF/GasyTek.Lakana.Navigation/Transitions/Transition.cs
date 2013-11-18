using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using GasyTek.Lakana.Navigation.Controls;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Base class for animated transitions.
    /// </summary>
    public abstract class Transition
    {
        #region Events

        public event EventHandler TransitionStarted;
        public event EventHandler TransitionCompleded;

        #endregion

        #region Properties

        public Duration Duration { get; set; }

        /// <summary>
        /// Indicates if at least one transition is running.
        /// </summary>
        public static bool IsRunning { get; private set; }

        #endregion

        public void Run(Panel scene, ViewHostControl oldItem, ViewHostControl newItem)
        {
            //
            // To work around the Wpf bug when using Storyboard.SetTarget, use a namescope + Storyboard.SetTargetName instead
            // cf. http://connect.microsoft.com/VisualStudio/feedback/details/723701/storyboard-settarget-only-works-on-uielements-but-throws-no-exception
            //

            if (IsRunning) return;

            IsRunning = true;

            // set items visibility
            oldItem.Visibility = Visibility.Visible;
            newItem.Visibility = Visibility.Visible;

            // Creates a namescope that will be associated to the scene panel
            var sceneNameScope = NameScope.GetNameScope(scene);
            if (sceneNameScope == null)
            {
                sceneNameScope = new NameScope();
                NameScope.SetNameScope(scene, sceneNameScope);
            }

            var transitionInfo = new TransitionInfo
                                     {
                                         Scene = scene,
                                         SceneNameScope = sceneNameScope,
                                         OldItem = oldItem,
                                         NewItem = newItem
                                     };

            RaiseTransitionStarted();
            OnRunTransitionStarted(transitionInfo);

            var storyboard = CreateAnimation(transitionInfo);
            storyboard.FillBehavior = FillBehavior.Stop;
            storyboard.Completed += (sender, args) =>
                                        {
                                            OnRunTransitionCompleted(transitionInfo);
                                            RaiseTransitionCompleded();

                                            // set items visibility
                                            oldItem.Visibility = Visibility.Visible;
                                            newItem.Visibility = Visibility.Hidden;

                                            IsRunning = false;
                                        };
            storyboard.Begin(scene);
        }

        #region Protected methods

        protected void RaiseTransitionStarted()
        {
            var handler = TransitionStarted;
            if (handler != null) handler(this, new EventArgs());
        }

        protected void RaiseTransitionCompleded()
        {
            var handler = TransitionCompleded;
            if (handler != null) handler(this, new EventArgs());
        }

        #endregion

        #region Overridable methods

        protected virtual void OnRunTransitionStarted(TransitionInfo transitionInfo)
        {
        }

        protected virtual void OnRunTransitionCompleted(TransitionInfo transitionInfo)
        {
        }

        protected abstract Storyboard CreateAnimation(TransitionInfo transitionInfo);

        #endregion
    }

    /// <summary>
    /// Used to share the transition animation necessary infos.
    /// </summary>
    public class TransitionInfo
    {
        public Panel Scene { get; set; }
        public INameScope SceneNameScope { get; set; }
        public ViewHostControl OldItem { get; set; }
        public ViewHostControl NewItem { get; set; }

        public double SceneWidth
        {
            get { return Scene != null ? Scene.ActualWidth : 0; }
        }
    }
}
