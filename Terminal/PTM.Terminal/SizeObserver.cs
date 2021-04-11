using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tesseract.Common;

namespace PTM.Terminal
{
    /// <summary>
    /// Pozwala rejestrować zmiany w rozmiarze kontrolki
    /// https://stackoverflow.com/questions/1083224/pushing-read-only-gui-properties-back-into-viewmodel
    /// </summary>
    public static class SizeObserver
    {
        /// <summary>
        /// Property przechowujące obserwowaną wartość
        /// </summary>
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(SizeObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        /// <summary>
        /// Property przechowujące obserwowaną szerokość kontrolki
        /// </summary>
        public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
            "ObservedWidth",
            typeof(double),
            typeof(SizeObserver));

        /// <summary>
        /// Property przechowujące obserwowaną wysokość kontrolki
        /// </summary>
        public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
            "ObservedHeight",
            typeof(double),
            typeof(SizeObserver));

        /// <summary>
        /// Pobiera obserwowaną wartość
        /// </summary>
        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        /// <summary>
        /// Ustawia obserwowaną wartość
        /// </summary>
        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        /// <summary>
        /// Pobiera obserwowaną szerokość kontrolki
        /// </summary>
        public static double GetObservedWidth(FrameworkElement frameworkElement)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            return (double)frameworkElement.GetValue(ObservedWidthProperty);
        }

        /// <summary>
        /// Ustawia obserwowaną szerokość kontrolki
        /// </summary>
        public static void SetObservedWidth(FrameworkElement frameworkElement, double observedWidth)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            frameworkElement.SetValue(ObservedWidthProperty, observedWidth);
        }

        /// <summary>
        /// Pobiera obserwowaną wysokość kontrolki
        /// </summary>
        public static double GetObservedHeight(FrameworkElement frameworkElement)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            return (double)frameworkElement.GetValue(ObservedHeightProperty);
        }

        /// <summary>
        /// Ustawia obserwowaną wysokość kontrolki
        /// </summary>
        public static void SetObservedHeight(FrameworkElement frameworkElement, double observedHeight)
        {
            Ensure.ParamNotNull(frameworkElement, nameof(frameworkElement));
            frameworkElement.SetValue(ObservedHeightProperty, observedHeight);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
            }
        }

        private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }

        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {
            // WPF 4.0 onwards
            frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
            frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);

            // WPF 3.5 and prior
            ////SetObservedWidth(frameworkElement, frameworkElement.ActualWidth);
            ////SetObservedHeight(frameworkElement, frameworkElement.ActualHeight);
        }
    }
}
