using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.View.CustomControls
{

    public class CustomSlider : RangeBase, INotifyPropertyChanged
    {
        static CustomSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomSlider), new FrameworkPropertyMetadata(typeof(CustomSlider))); // Указываем стиль по умолчанию
        }
            
        // Dependency Properties
        public static readonly DependencyProperty TrackBarColorProperty =
            DependencyProperty.Register(
                "TrackBarColor",
                typeof(Brush),
                typeof(CustomSlider),
                new PropertyMetadata(Brushes.Red));

        public Brush TrackBarColor
        {
            get { return (Brush)GetValue(TrackBarColorProperty); }
            set { SetValue(TrackBarColorProperty, value); }
        }

        public static readonly DependencyProperty TrackColorProperty =
            DependencyProperty.Register(
                "TrackColor",
                typeof(Brush),
                typeof(CustomSlider),
                new PropertyMetadata(Brushes.Gray));


        public Brush TrackColor
        {
            get { return (Brush)GetValue(TrackColorProperty); }
            set { SetValue(TrackColorProperty, value); }
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            OnPropertyChanged(nameof(Value));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

