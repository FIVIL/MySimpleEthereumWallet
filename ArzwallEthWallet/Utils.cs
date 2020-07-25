using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ArzwallEthWallet
{
    public static class Utils
    {
        public static void Navigate(UserControl to)
        {
            var Window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var t = Window.Content as Grid;
            var g = t.Children[0] as Grid;
            var w = t.Children[1] as Grid;
            w.Visibility = Visibility.Hidden;
            w.Children.Clear();
            g.Children.Clear();
            g.Children.Add(to);
        }
    }
}
