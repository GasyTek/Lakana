using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GasyTek.Lakana.Navigation.Transitions.Anim3D
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Transition3D : Transition
    {
        private Viewport3D Viewport3D { get; set; }

        protected Camera Camera { get; private set; }
        protected ModelVisual3D Light { get; private set; }

        protected Visibility OriginalBackViewVisibility { get; private set; }
        protected Visibility OriginalFrontViewVisibility { get; private set; }

        protected sealed override void OnRunTransitionStarted(TransitionInfo transitionInfo)
        {
            // save original visibility states
            OriginalBackViewVisibility = transitionInfo.BackView.Visibility;
            OriginalFrontViewVisibility = transitionInfo.FrontView.Visibility;

            OnRunTransitionStartExt(transitionInfo);
            Setup3DScene(transitionInfo);
        }

        protected sealed override void OnRunTransitionCompleted(TransitionInfo transitionInfo)
        {
            // restores original visibility states
            transitionInfo.BackView.Visibility = OriginalBackViewVisibility;
            transitionInfo.FrontView.Visibility = OriginalFrontViewVisibility;

            // removes the 3D scene
            transitionInfo.Scene.Children.Remove(Viewport3D);

            OnRunTransitionCompletedExt(transitionInfo);
        }

        private void Setup3DScene(TransitionInfo transitionInfo)
        {
            // Create the viewport that will host the 3D animation
            Viewport3D = new Viewport3D { IsHitTestVisible = false, ClipToBounds = false };

            // Set up the camera
            Camera = CreateCamera(transitionInfo);
            Viewport3D.Camera = Camera;

            // Set up the light
            Light = CreateLight(transitionInfo);
            Viewport3D.Children.Add(Light);

            // Set up the 3D objects
            var objects = Create3DObjects(transitionInfo);
            objects.ForEach(m => Viewport3D.Children.Add(m));

            transitionInfo.Scene.Children.Add(Viewport3D);

            // Hides views
            transitionInfo.BackView.Visibility = Visibility.Hidden;
            transitionInfo.FrontView.Visibility = Visibility.Hidden;
        }

        protected virtual Camera CreateCamera(TransitionInfo transitionInfo)
        {
            return new PerspectiveCamera();
        }

        protected virtual ModelVisual3D CreateLight(TransitionInfo transitionInfo)
        {
            return new ModelVisual3D { Content = new AmbientLight(Colors.White) };
        }

        protected virtual List<ModelVisual3D> Create3DObjects(TransitionInfo transitionInfo)
        {
            return new List<ModelVisual3D>();
        }

        protected virtual void OnRunTransitionStartExt(TransitionInfo transitionInfo) { }
        protected virtual void OnRunTransitionCompletedExt(TransitionInfo transitionInfo) { }
    }
}
