using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ReestrTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        bool licensact = false;
        long licensCode = 2645736289;

        public MainWindow()
        {
            InitializeComponent();
            licenseCheker();
        }
        
        public void licenseCheker()
        {
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"TrialTestProgramm\License");
            if (key1 != null)
            {
                string chek = key1.GetValue(@"LicenseKey").ToString();
                if (chek != licensCode.ToString())
                {
                    var key0 = Registry.CurrentUser.OpenSubKey("TrialTestProgramm");
                    if (key0 != null && licensact == false)
                        Timer_start();

                }
                else
                {
                    licensact = true;
                    TrialBTN.Visibility = Visibility.Hidden;
                    LicenseBTN.Visibility = Visibility.Hidden;
                    CheckKeyTB.Visibility = Visibility.Hidden;
                    TimerLB.Content = "Безгранично!";
                    MessageBox.Show("Лицензия активна!");
                }
            }
            else
                Timer_start();
        }

        public async void Timer_start()
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey("TrialTestProgramm",true);
            if (k != null)
            {
                int t = (int)k.GetValue("TimerR");
                TimerLB.Content = t;
                while (t > 0 && licensact == false)
                {
                    t--;
                    k.SetValue("TimerR", t);
                    TimerLB.Content = t;
                    await Task.Delay(1000);
                    if (t == 0)
                    {
                        MessageBox.Show("Время Триала истекло");
                    }
                }
            }
        }

        private void TrialBTN_Click(object sender, RoutedEventArgs e)
        {

            var key0 = Registry.CurrentUser.OpenSubKey("TrialTestProgramm");
            
            if(key0 == null )
            {

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("TrialTestProgramm"))
                {
                    key.SetValue("TimerR", 30);
                    MessageBox.Show("Триал активирован");
                }
                Timer_start();

            }
            else
            {
                MessageBox.Show("Триал уже активирован!!");
                Timer_start();
            }
           
        }

        private void LicenseBTN_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey key0 = Registry.CurrentUser.OpenSubKey("TrialTestProgramm\\License");
            if( key0 == null)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("TrialTestProgramm\\License", true))
                {
                    key.SetValue("LicenseKey", 0000000000);
                }
            }
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("TrialTestProgramm\\License\\", true);
            if (key1 != null)
            {
                if(CheckKeyTB.Text == licensCode.ToString())
                {
                    key1.SetValue("LicenseKey", licensCode);
                    licensact = true;
                    TrialBTN.Visibility = Visibility.Hidden;
                    LicenseBTN.Visibility = Visibility.Hidden;
                    CheckKeyTB.Visibility = Visibility.Hidden;
                    TimerLB.Content = "Безгранично!";
                    MessageBox.Show("Лицензия активна!");

                }
                else
                {
                    MessageBox.Show("Лицензионый ключь не верен!");

                }
            }
        }
    }
}
