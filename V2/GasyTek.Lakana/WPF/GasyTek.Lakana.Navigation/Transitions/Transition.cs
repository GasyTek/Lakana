using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;
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

        public static bool IsRunning { get; private set; }

        #endregion

        public void Run(Panel scene, HostControl backView, HostControl frontView, AnimationType animationType)
        {
            //
            // To work around the Wpf bug when using Storyboard.SetTarget, use a namescope + Storyboard.SetTargetName instead
            // cf. http://connect.microsoft.com/VisualStudio/feedback/details/723701/storyboard-settarget-only-works-on-uielements-but-throws-no-exception
            //

            if (IsRunning) return;

            IsRunning = true;

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
                BackView = backView,
                FrontView = frontView,
                AnimationType = animationType
            };

            RaiseTransitionStarted();
            OnRunTransitionStarted(transitionInfo);

            // Execute this code asynchronously in the dispatcher with the priority specified
            // so that we can be sure, it will be executed after the view was rendered
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        var storyboard = CreateAnimation(transitionInfo);
                        storyboard.FillBehavior = FillBehavior.Stop;
                        storyboard.Completed += (sender, args) =>
                        {
                            // Reset views
                            if (backView != null) backView.Reset();
                            if (frontView != null) frontView.Reset();

                            // Notify world
                            OnRunTransitionCompleted(transitionInfo);
                            RaiseTransitionCompleded();

                            IsRunning = false;
                        };

                        storyboard.Begin(scene);
                    }), DispatcherPriority.Loaded);
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
        public HostControl BackView { get; set; }
        public HostControl FrontView { get; set; }
        public AnimationType AnimationType { get; set; }

        public double SceneWidth
        {
            get { return Scene != null ? Scene.ActualWidth : 0; }
        }
    }

    /// <summary>
    /// Type of animation to execute. 
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Show the front view and animate this process. 
        /// </summary>
        ShowFrontView,

        /// <summary>
        /// Hide the front view and animate this process.
        /// </summary>
        HideFrontView
    }
}
