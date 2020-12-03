// <copyright file="Spinner.xaml.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace APE.PostgreSQL.Teamwork.GUI.Loading
{
    /// <summary>
    /// Interaction logic for Spinner.xaml.
    /// </summary>
    public partial class Spinner : UserControl
    {
        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(Spinner), new PropertyMetadata(null));

        private static readonly List<Color> DefaultColors = new List<Color>()
        {
            (Color)ColorConverter.ConvertFromString("#4285F4"),
            (Color)ColorConverter.ConvertFromString("#DE3E35"),
            (Color)ColorConverter.ConvertFromString("#F7C223"),
            (Color)ColorConverter.ConvertFromString("#1B9A59"),
        };

        private int actualColor = 1;
        private ColorAnimation oldAnimation = null;

        public Spinner()
        {
            this.InitializeComponent();
            this.Loaded += this.SpinnerLoaded;
        }

        public Brush Color
        {
            get { return (Brush)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }

        private void SpinnerLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Color == null)
            {
                this.arc.Stroke = new SolidColorBrush(DefaultColors[0]);
                this.ColorAnimationCompleted();
            }
            else
            {
                this.arc.Stroke = this.Color;
            }
        }

        private void ColorAnimationCompleted(object sender = null, EventArgs e = null)
        {
            if (this.oldAnimation != null)
            {
                this.oldAnimation.Completed -= this.ColorAnimationCompleted;
                this.oldAnimation = null;
            }

            var colorAnimation = new ColorAnimation(DefaultColors[this.actualColor], new Duration(TimeSpan.FromSeconds(2)), FillBehavior.HoldEnd);
            colorAnimation.Completed += this.ColorAnimationCompleted;
            this.arc.Stroke.ApplyAnimationClock(SolidColorBrush.ColorProperty, colorAnimation.CreateClock(), HandoffBehavior.Compose);
            this.oldAnimation = colorAnimation;

            if (this.actualColor == DefaultColors.Count - 1)
            {
                this.actualColor = 0;
            }
            else
            {
                this.actualColor++;
            }
        }
    }
}