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

namespace Kassa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    //Looge kassarakendus kasutades WPF raamistikku.

    //Müüja saab lisada ise tooteid.Tootel peab olema nimetus, hind ja kogus.
    //Müüja saab lisada olemasolevaid tooteid ostukorvi mille alusel arvutatakse välja summa mida klient maksma peab.
    public partial class MainWindow : Window
    {
        List<Product> ExistingProducts;

        public MainWindow()
        {
            InitializeComponent();
            InitializeExistingProducts();
        }

        void InitializeExistingProducts()
        {
            Product curProduct;
            ExistingProducts = new List<Product>();

            string[] allLines = System.IO.File.ReadAllLines("../../products.txt");

            foreach (string curLine in allLines)
            {
                if (curLine.Contains('#'))
                    continue;

                string[] splitLine = curLine.Split();

                curProduct = new Product();

                curProduct.Name = splitLine[0];
                curProduct.Price = Convert.ToDecimal(splitLine[1]);
                curProduct.Quantity = Convert.ToInt32(splitLine[2]);

                ExistingProducts.Add(curProduct);
            }
            productListBox.ItemsSource = ExistingProducts;
            
        }

        void CalculateFullPrice()
        {

        }

        private void addProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
