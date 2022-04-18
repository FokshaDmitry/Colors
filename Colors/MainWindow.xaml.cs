using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
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

namespace Colors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog1;
        private byte[]? Image;
        private BitmapImage imgsource;

        public MainWindow()
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf",
                Title = "Open image file"
            };
        }

        public void OpenFileDialogForm()
        {
            Dispatcher.Invoke(new Action(() => openFileDialog1.ShowDialog()));
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    Dispatcher.Invoke(new Action(() => ColorImage.Source = new BitmapImage(new Uri(openFileDialog1.FileName))));
                    Dispatcher.Invoke(new Action(() => link.Text = openFileDialog1.FileName));
                    Image = File.ReadAllBytes(openFileDialog1.FileName);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        public void Search()
        {
            ServerConnect server = new ServerConnect();
            server.onError += (msg) =>
            {
                MessageBox.Show(msg);
            };
            server.Connect();
            if (Dispatcher.Invoke(() => SearchName.Text) == "")
            {
                MessageBox.Show("Write Color Name");
                return;
            }
            if (Dispatcher.Invoke(() => SearchVName.Text) == "")
            {
                MessageBox.Show("Write Vendor Name");
                return;
            }
            //Lib.EntityContext.Entity entity = new Lib.EntityContext.Entity();
            server.newSearch(Dispatcher.Invoke(() => SearchName.Text), Dispatcher.Invoke(() => SearchVName.Text));
            server.waitResponse((res)=>
            {
                Lib.EntityContext.Entity entity = (Lib.EntityContext.Entity)res.data;
                Dispatcher.Invoke(new Action(() =>
                {
                    imgsource = new BitmapImage();
                    imgsource.BeginInit();
                    imgsource.StreamSource = new MemoryStream(entity.Image);
                    imgsource.EndInit();
                    ColorImage.Source = imgsource;
                    MessageBox.Show(res.StatusTxt);
                }));
            });
            
            //Dispatcher.Invoke(new Action(() => ColorImage.Source = imgsource));
        }
        public void Add()
        {
            ServerConnect server = new ServerConnect();
            server.onError += (msg) =>
            {
                MessageBox.Show(msg);
            };
            server.Connect();
            if (Dispatcher.Invoke(()=> AddName.Text) == "")
            {
                MessageBox.Show("Write Color Name");
                return;
            }
            if (Dispatcher.Invoke(() => AddVName.Text) == "")
            {
                MessageBox.Show("Write Vendor Name");
                return;
            }
            if (Image == null)
            {
                MessageBox.Show("Add Image");
                return;
            }
            server.newAdd(Dispatcher.Invoke(() => AddName.Text), Dispatcher.Invoke(() => AddVName.Text), Image);
            server.waitResponse((res) => MessageBox.Show(res.StatusTxt));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => OpenFileDialogForm());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> Add());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Search());
        }
    }
}
