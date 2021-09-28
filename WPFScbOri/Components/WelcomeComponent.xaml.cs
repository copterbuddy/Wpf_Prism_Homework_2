using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFScbOri.Components
{
    /// <summary>
    /// Interaction logic for WelcomeComponent.xaml
    /// </summary>
    public partial class WelcomeComponent : UserControl
    {
        public WelcomeComponent()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty BackgroundImagePathProperty = 
            DependencyProperty.Register(nameof(BackgroundImagePath),
                typeof(string),
                typeof(WelcomeComponent));

        public string BackgroundImagePath
        {
            get { return (string)GetValue(BackgroundImagePathProperty); }
            set { SetValue(BackgroundImagePathProperty, value); }
        }

        public static readonly DependencyProperty ImageCenterPathProperty =
            DependencyProperty.Register(nameof(ImageCenterPath),
                typeof(string),
                typeof(WelcomeComponent));

        public string ImageCenterPath
        {
            get { return (string)GetValue(ImageCenterPathProperty); }
            set { SetValue(ImageCenterPathProperty, value); }
        }

    }
}
