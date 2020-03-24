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
            if(txt_Age.Text.Length < 1 || txt_Name.Text.Length < 1 || cbox_Gender.Text.Length < 1 || cbox_Role.Text.Length < 1 || txt_Pass.Password.Length < 1)
            {
                MessageBox.Show("Please fill in your name, a password, age, gender and role.");
                return;
            }
            if (stack_interests.Children.Count > 0)
            {
                foreach (UC_Interst uc in stack_interests.Children)
                {
                    if (uc.txt_interest.Text.Length < 1)
                    {
                        MessageBox.Show("One or more of your interests is empty. Please make sure they are all filled in and remove empty ones.");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter at least 1 interest.");
                return;
            }

            // DO THINGS WITH INFORMATION HERE, SAVE IT SO IT CAN BE USED FOR LATER.

            HomePage home = new HomePage();
            home.Show();
            this.Close();
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
