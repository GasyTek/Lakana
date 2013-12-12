using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Transitions.Anim3D;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Base class for animated transitions.
    /// </summary>
    public abstract class Transition
    {
        #region Properties

        public Duration Duration { get; set; }

        /// <summary>
        /// Specifies if an animation is already in progress.
        /// Only one animation can be played at a time.
        /// </summary>
        public static bool IsRunning { get; private set; }

        #endregion

        public Task Run(Panel scene, HostControl backView, HostControl frontView, AnimationType animationType)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (IsRunning)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            IsRunning = true;

            //
            // Creates a namescope that will be associated to the scene panel
            // to work around the Wpf bug when using Storyboard.SetTarget, use a namescope + Storyboard.SetTargetName instead
            // cf. http://connect.microsoft.com/VisualStudio/feedback/details/723701/storyboard-settarget-only-works-on-uielements-but-throws-no-exception
            //

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

            // Storyboard completed handler
            var localBackView = backView;
            var localFrontView = frontView;
            var localTransitionInfo = transitionInfo;
            var storyboardCompletedAction = new Action(() =>
                                                {
                                                    // Commented out to avoid view flickering
                                                    //// Reset views
                                                    //if (localBackView != null) localBackView.Reset();
                                                    //if (localFrontView != null) localFrontView.Reset();

                                                    // Clean
                                                    OnRunTransitionCompleted(localTransitionInfo);

                                                    IsRunning = false;

                                                    tcs.SetResult(true);
                                                });

            // Execute this code asynchronously in the dispatcher with the priority specified
            // so that we can be sure, it will be executed after the view was rendered
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        OnRunTransitionStarted(transitionInfo);

                        var storyboard = CreateAnimation(transitionInfo);

                        // if the storyboard is empty then trigger storyboard completion manually
                        if (storyboard.Children.Count == 0) { storyboardCompletedAction(); }
                        else
                        {
                            // If it is a 3D animation then hide views
                            if (this is Transition3D) { EnsuresViewsAreHidden(backView, frontView); }
                            else { EnsuresViewsAreVisible(backView, frontView); }

                            storyboard.Completed += (sender, args) => storyboardCompletedAction();
                            storyboard.Begin(scene);
                        }

                    }), DispatcherPriority.Loaded);

            return tcs.Task;
        }

        private void EnsuresViewsAreVisible(HostControl backView, HostControl frontView)
        {
            if (backView != null) backView.Visibility = Visibility.Visible;
            if (frontView != null) frontView.Visibility = Visibility.Visible;
        }

        private void EnsuresViewsAreHidden(HostControl backView, HostControl frontView)
        {
            if (backView != null) backView.Visibility = Visibility.Hidden;
            if (frontView != null) frontView.Visibility = Visibility.Hidden;
        }

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
}
