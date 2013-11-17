using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using GasyTek.Lakana.Navigation.Controls;

namespace GasyTek.Lakana.Navigation.Transitions.Anim3D
{
    public class Cube3DTransition : Transition3D
    {
        private const string AnimatedObjectName = "AF50FE302444D8A7EF51E944534D41";

        public IEasingFunction EasingFunction { get; set; }

        #region Constructor
        
        public Cube3DTransition()
        {
            Duration = new Duration(TimeSpan.FromSeconds(2));
            EasingFunction = new BounceEase();
        }

        #endregion

        #region Overriden methods

        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();
            
            var cameraRotationAnimation = new DoubleAnimation(0, 90, Duration) { EasingFunction = EasingFunction };

            Storyboard.SetTargetName(cameraRotationAnimation, AnimatedObjectName);
            Storyboard.SetTargetProperty(cameraRotationAnimation, new PropertyPath(AxisAngleRotation3D.AngleProperty));

            storyboard.Children.Add(cameraRotationAnimation);

            return storyboard;
        }

        protected override Camera CreateCamera(TransitionInfo transitionInfo)
        {
            var axisAngle = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);

            transitionInfo.SceneNameScope.RegisterName(AnimatedObjectName, axisAngle);
            
            var camera = new PerspectiveCamera
                          {
                              LookDirection = new Vector3D(0, 0, -1),
                              Transform = new RotateTransform3D(axisAngle)
                          };

            // Compute camera position
            const double fieldOfView = 30d;
            const double fieldOfViewRadian = (fieldOfView / 2d) * (Math.PI / 180f);

            var oppositeSideLength = transitionInfo.SceneWidth / 2d;
            var computedOppositeSideLength = oppositeSideLength * Math.Tan(fieldOfViewRadian) + oppositeSideLength;
            var cameraDistance = computedOppositeSideLength / Math.Tan(fieldOfViewRadian);

            camera.FieldOfView = fieldOfView;
            camera.Position = new Point3D(0, 0, cameraDistance);

            return camera;
        }

        protected override List<ModelVisual3D> Create3DObjects(TransitionInfo transitionInfo)
        {
            return new List<ModelVisual3D>
                       {
                           new ModelVisual3D
                               {
                                   Content = new GeometryModel3D
                                                 {
                                                     Material = CreateOldItemFaceMaterial(transitionInfo.OldItem),
                                                     Geometry = CreateOldItemFaceMesh(transitionInfo.OldItem)
                                                 }
                               },
                           new ModelVisual3D
                               {
                                   Content = new GeometryModel3D
                                                 {
                                                     Material = CreateNewItemFaceMaterial(transitionInfo.NewItem),
                                                     Geometry = CreateNewItemFaceMesh(transitionInfo.NewItem)
                                                 }
                               }
                       };
        }

        protected override void OnRunTransitionCompletedExt(TransitionInfo transitionInfo)
        {
            transitionInfo.SceneNameScope.UnregisterName(AnimatedObjectName);
        }

        #endregion

        #region Private methods

        private MeshGeometry3D CreateOldItemFaceMesh(ViewHostControl oldItem)
        {
            var geometry = new MeshGeometry3D
                               {
                                   Positions = new Point3DCollection(
                                       new[]
                                           {
                                               new Point3D(-1, -1, 1),
                                               new Point3D(1, -1, 1),
                                               new Point3D(-1, 1, 1),
                                               new Point3D(1, 1, 1)
                                           }),
                                   TextureCoordinates = new PointCollection(
                                       new[]
                                           {
                                               new Point(0, 1),
                                               new Point(1, 1),
                                               new Point(0, 0),
                                               new Point(1, 0)
                                           }),
                                   TriangleIndices = new Int32Collection(new[] { 0, 1, 2, 1, 3, 2 })
                               };

            geometry.Positions =
                new Point3DCollection(
                    geometry.Positions.Select(
                        p =>
                        new Point3D(p.X * (oldItem.ActualWidth / 2d), p.Y * (oldItem.ActualHeight / 2d),
                                    p.Z * (oldItem.ActualWidth / 2d))));

            return geometry;
        }

        private Material CreateOldItemFaceMaterial(ViewHostControl oldItem)
        {
            return new DiffuseMaterial(oldItem.TakeSnapshot());
        }

        private MeshGeometry3D CreateNewItemFaceMesh(ViewHostControl newItem)
        {
            var geometry = new MeshGeometry3D
                               {
                                   Positions = new Point3DCollection(
                                       new[]
                                           {
                                               new Point3D(1, -1, 1),
                                               new Point3D(1, -1, -1),
                                               new Point3D(1, 1, 1),
                                               new Point3D(1, 1, -1)
                                           }),
                                   TextureCoordinates = new PointCollection(
                                       new[]
                                           {
                                               new Point(0, 1),
                                               new Point(1, 1),
                                               new Point(0, 0),
                                               new Point(1, 0)
                                           }),
                                   TriangleIndices = new Int32Collection(new[] { 0, 1, 2, 1, 3, 2 })
                               };

            geometry.Positions =
                new Point3DCollection(
                    geometry.Positions.Select(
                        p =>
                        new Point3D(p.X * (newItem.ActualWidth / 2d), p.Y * (newItem.ActualHeight / 2d),
                                    p.Z * (newItem.ActualWidth / 2d))));

            return geometry;
        }

        private Material CreateNewItemFaceMaterial(ViewHostControl newItem)
        {
            return new DiffuseMaterial(newItem.TakeSnapshot());
        }

        #endregion
    }
}