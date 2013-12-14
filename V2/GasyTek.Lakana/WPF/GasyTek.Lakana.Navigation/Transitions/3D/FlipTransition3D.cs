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
    public class FlipTransition3D : Transition3D
    {
        public enum FlipDirection
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }

        private Point3D _cameraInitialPosition;
        private Point3D _cameraIntermediatePosition;

        private const string CameraRotationObjectName = "AF50FE302444D8A7EF51E944534D41";
        private const string CameraObjectName = "BF50FE302444D8A7EF51C944534D41";

        public FlipDirection Direction { get; set; }

        #region Constructor

        public FlipTransition3D()
        {
            Direction = FlipDirection.BottomToTop;
            Duration = new Duration(TimeSpan.FromSeconds(1));
        }

        #endregion

        #region Overriden methods

        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            Point3DAnimationUsingKeyFrames cameraPositionAnimation;
            DoubleAnimationUsingKeyFrames cameraRotationAnimation;

            // Camera distance animation
            cameraPositionAnimation = new Point3DAnimationUsingKeyFrames { Duration = Duration };
            cameraPositionAnimation.KeyFrames.Add(new EasingPoint3DKeyFrame(_cameraInitialPosition, KeyTime.FromPercent(0)));
            cameraPositionAnimation.KeyFrames.Add(new EasingPoint3DKeyFrame(_cameraIntermediatePosition, KeyTime.FromPercent(0.25)));
            cameraPositionAnimation.KeyFrames.Add(new EasingPoint3DKeyFrame(_cameraIntermediatePosition, KeyTime.FromPercent(0.75)));
            cameraPositionAnimation.KeyFrames.Add(new EasingPoint3DKeyFrame(_cameraInitialPosition, KeyTime.FromPercent(1)));

            Storyboard.SetTargetName(cameraPositionAnimation, CameraObjectName);
            Storyboard.SetTargetProperty(cameraPositionAnimation, new PropertyPath(ProjectionCamera.PositionProperty));

            // Flip animation

            switch (Direction)
            {
                case FlipDirection.LeftToRight:
                case FlipDirection.BottomToTop:
                    cameraRotationAnimation = new DoubleAnimationUsingKeyFrames { Duration = Duration };
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.25)));
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(-180, KeyTime.FromPercent(0.75)));
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(-180, KeyTime.FromPercent(1)));
                    break;

                default:
                    cameraRotationAnimation = new DoubleAnimationUsingKeyFrames { Duration = Duration };
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromPercent(0.25)));
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(180, KeyTime.FromPercent(0.75)));
                    cameraRotationAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(180, KeyTime.FromPercent(1)));
                    break;
            }

            Storyboard.SetTargetName(cameraRotationAnimation, CameraRotationObjectName);
            Storyboard.SetTargetProperty(cameraRotationAnimation, new PropertyPath(AxisAngleRotation3D.AngleProperty));

            storyboard.Children.Add(cameraPositionAnimation);
            storyboard.Children.Add(cameraRotationAnimation);

            return storyboard;
        }

        protected override Camera CreateCamera(TransitionInfo transitionInfo)
        {
            Rotation3D axisAngle;

            switch (Direction)
            {
                case FlipDirection.TopToBottom:
                case FlipDirection.BottomToTop:
                    axisAngle = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
                    break;

                default:
                    axisAngle = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
                    break;
            }

            var camera = new PerspectiveCamera
            {
                LookDirection = new Vector3D(0, 0, -1),
                Transform = new RotateTransform3D(axisAngle)
            };

            transitionInfo.SceneNameScope.RegisterName(CameraRotationObjectName, axisAngle);
            transitionInfo.SceneNameScope.RegisterName(CameraObjectName, camera);

            // Compute camera position
            const double fieldOfView = 30d;
            const double fieldOfViewRadian = (fieldOfView / 2d) * (Math.PI / 180f);

            var oppositeSideLength = transitionInfo.SceneWidth / 2d;
            var computedOppositeSideLength = oppositeSideLength * Math.Tan(fieldOfViewRadian) + oppositeSideLength;

            _cameraInitialPosition = new Point3D(0, 0, oppositeSideLength / Math.Tan(fieldOfViewRadian));
            _cameraIntermediatePosition = new Point3D(0, 0, computedOppositeSideLength / Math.Tan(fieldOfViewRadian));

            camera.FieldOfView = fieldOfView;
            camera.Position = _cameraInitialPosition;

            return camera;
        }

        protected override List<ModelVisual3D> Create3DObjects(TransitionInfo transitionInfo)
        {
            var initiallyVisibleView = transitionInfo.AnimationType == AnimationType.ShowFrontView
                                           ? transitionInfo.BackView
                                           : transitionInfo.FrontView;

            var initiallyInvisibleView = transitionInfo.AnimationType == AnimationType.ShowFrontView
                                           ? transitionInfo.FrontView
                                           : transitionInfo.BackView;

            return new List<ModelVisual3D>
                       {
                           new ModelVisual3D
                               {
                                   Content = new GeometryModel3D
                                                 {
                                                     Material = CreateInitiallyVisibleFaceMaterial(initiallyVisibleView),
                                                     Geometry = CreateInitiallyVisibleFaceMesh(initiallyVisibleView)
                                                 }
                               },
                           new ModelVisual3D
                               {
                                   Content = new GeometryModel3D
                                                 {
                                                     Material = CreateInitiallyInvisibleFaceMaterial(initiallyInvisibleView),
                                                     Geometry = CreateInitiallyInvisibleFaceMesh(initiallyInvisibleView)
                                                 }
                               }
                       };
        }

        protected override void OnRunTransitionCompletedExt(TransitionInfo transitionInfo)
        {
            transitionInfo.SceneNameScope.UnregisterName(CameraObjectName);
            transitionInfo.SceneNameScope.UnregisterName(CameraRotationObjectName);
        }

        #endregion

        #region Private methods

        private MeshGeometry3D CreateInitiallyVisibleFaceMesh(HostControl view)
        {
            var geometry = new MeshGeometry3D
            {
                Positions = new Point3DCollection(
                    new[]
                                           {
                                               new Point3D(-1, -1, 0),
                                               new Point3D(1, -1, 0),
                                               new Point3D(-1, 1, 0),
                                               new Point3D(1, 1, 0)
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

            var actualWidth = view != null ? view.ActualWidth : 1;
            var actualHeight = view != null ? view.ActualHeight : 1;

            geometry.Positions =
                new Point3DCollection(
                    geometry.Positions.Select(
                        p =>
                        new Point3D(p.X * (actualWidth / 2d), p.Y * (actualHeight / 2d),
                                    p.Z * (actualWidth / 2d))));

            return geometry;
        }

        private Material CreateInitiallyVisibleFaceMaterial(HostControl view)
        {
            return view != null ? new DiffuseMaterial(view.TakeSnapshot()) : new DiffuseMaterial(Brushes.Transparent);
        }

        private MeshGeometry3D CreateInitiallyInvisibleFaceMesh(HostControl view)
        {
            PointCollection textureCoordinates;

            switch (Direction)
            {
                case FlipDirection.TopToBottom:
                case FlipDirection.BottomToTop:
                    textureCoordinates = new PointCollection(
                        new[]
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(0, 1),
                            new Point(1, 1)
                        });
                    break;

                default:
                    textureCoordinates = new PointCollection(
                        new[]
                        {
                            new Point(1, 1),
                            new Point(0, 1),
                            new Point(1, 0),
                            new Point(0, 0)
                        });
                    break;
            }

            var geometry = new MeshGeometry3D
            {
                Positions = new Point3DCollection(
                    new[]
                                           {
                                               new Point3D(-1, -1, 0),
                                               new Point3D(1, -1, 0),
                                               new Point3D(-1, 1, 0),
                                               new Point3D(1, 1, 0)
                                           }),
                TextureCoordinates = textureCoordinates,
                TriangleIndices = new Int32Collection(new[] { 0, 2, 1, 1, 2, 3 })
            };

            var actualWidth = view != null ? view.ActualWidth : 1;
            var actualHeight = view != null ? view.ActualHeight : 1;

            geometry.Positions =
                new Point3DCollection(
                    geometry.Positions.Select(
                        p =>
                        new Point3D(p.X * (actualWidth / 2d), p.Y * (actualHeight / 2d),
                                    p.Z * (actualWidth / 2d))));

            return geometry;
        }

        private Material CreateInitiallyInvisibleFaceMaterial(HostControl view)
        {
            return view != null ? new DiffuseMaterial(view.TakeSnapshot()) : new DiffuseMaterial(Brushes.Transparent);
        }

        #endregion
    }
}