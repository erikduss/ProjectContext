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
using System.Windows.Shapes;

namespace ProjectContextTest
{
    /// <summary>
    /// Interaction logic for CreateProfile.xaml
    /// </summary>
    public partial class CreateProfile : Window
    {
        List<UC_Interst> interests = new List<UC_Interst>();

        public CreateProfile()
        {
            InitializeComponent();
        }

        private void btn_AddInterest_Click(object sender, RoutedEventArgs e)
        {
            stack_interests.Children.Add(new UC_Interst());
            /*stack_interests.Children.Add(new TextBox { 
                FontSize = 24, 
                Height = 50,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 300
            });*/
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ClearEmpty_Click(object sender, RoutedEventArgs e)
        {
            /*foreach(TextBox tb in stack_interests.Children)
            {
                if(tb.Text.Length < 1)
                {
                    stack_interests.Children.Remove(tb);
                }
            }*/

            List<UC_Interst> toRemove = new List<UC_Interst>();

            foreach (UC_Interst uc in stack_interests.Children)
            {
                    if (uc.txt_interest.Text.Length < 1)
                    {
                        toRemove.Add(uc);
                        
                    }
            }

            foreach (UC_Interst uc in toRemove)
            {
                stack_interests.Children.Remove(uc);
            }
        }
    }
}
