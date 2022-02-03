// <copyright file="Arc.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace APE.PostgreSQL.Teamwork.GUI.Loading
{
    /// <summary>
    /// Displays a circle outline with the given angles.
    /// </summary>
    public sealed class Arc : Shape
    {
        // Using a DependencyProperty as the backing store for Center.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(Arc), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new FrameworkPropertyMetadata(Math.PI / 2.0, FrameworkPropertyMetadataOptions.AffectsRender));

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Arc), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        // Using a DependencyProperty as the backing store for SmallAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallAngleProperty = DependencyProperty.Register("SmallAngle", typeof(bool), typeof(Arc), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static Arc() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));

        public Point Center
        {
            get => (Point)this.GetValue(CenterProperty);
            set => this.SetValue(CenterProperty, value);
        }

        public double StartAngle
        {
            get => (double)this.GetValue(StartAngleProperty);
            set => this.SetValue(StartAngleProperty, value);
        }

        public double EndAngle
        {
            get => (double)this.GetValue(EndAngleProperty);
            set => this.SetValue(EndAngleProperty, value);
        }

        public double Radius
        {
            get => (double)this.GetValue(RadiusProperty);
            set => this.SetValue(RadiusProperty, value);
        }

        public bool SmallAngle
        {
            get => (bool)this.GetValue(SmallAngleProperty);
            set => this.SetValue(SmallAngleProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var a0 = this.StartAngle < 0
                    ? this.StartAngle + (2 * Math.PI)
                    : this.StartAngle;
                var a1 = this.EndAngle < 0
                    ? this.EndAngle + (2 * Math.PI)
                    : this.EndAngle;

                if (a1 < a0)
                {
                    a1 += Math.PI * 2;
                }

                var d = SweepDirection.Counterclockwise;
                bool large;

                if (this.SmallAngle)
                {
                    large = false;
                    var t = a1;
                    if (a1 - a0 > Math.PI)
                    {
                        d = SweepDirection.Counterclockwise;
                    }
                    else
                    {
                        d = SweepDirection.Clockwise;
                    }
                }
                else
                {
                    large = Math.Abs(a1 - a0) < Math.PI;
                }

                var p0 = this.Center + (new Vector(Math.Cos(a0), Math.Sin(a0)) * this.Radius);
                var p1 = this.Center + (new Vector(Math.Cos(a1), Math.Sin(a1)) * this.Radius);

                var segments = new List<PathSegment>(1)
                {
                    new ArcSegment(p1, new Size(this.Radius, this.Radius), 0.0, large, d, true),
                };
                var figures = new List<PathFigure>(1);
                var pf = new PathFigure(p0, segments, true)
                {
                    IsClosed = false,
                };
                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
                return g;
            }
        }
    }
}