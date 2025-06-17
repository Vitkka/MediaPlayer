using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using WpfApp1.View.CustomControls;


namespace WpfApp1.Converters
{
    class ValueToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double sliderValue = (double)value;
            if (parameter is FrameworkElement slider)
            {
                CustomSlider customSlider = slider as CustomSlider;
                double trackLength = slider.ActualWidth; // Полная ширина слайдера
                double percentage = (sliderValue - customSlider.Minimum) / (customSlider.Maximum - customSlider.Minimum);
                return percentage * trackLength;
            }
            return 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
