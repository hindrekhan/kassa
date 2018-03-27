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
        List<Product> ExistingProducts = new List<Product>();
        List<Product> SelectedProducts = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeExistingProducts();
            addQuantity.Text = "1";
        }

        void InitializeExistingProducts()
        {
            Product curProduct;

            string[] allLines = System.IO.File.ReadAllLines("../../products.txt");
            
            foreach (string curLine in allLines)
            {
                if (curLine.Contains('#'))
                    continue;

                string[] splitLine = curLine.Split();

                curProduct = new Product();

                curProduct.Name = splitLine[0];
                curProduct.Price = Convert.ToDecimal(splitLine[1]);
                curProduct.Quantity = 0;

                ExistingProducts.Add(curProduct);
            }

            productListBox.ItemsSource = ExistingProducts;
        }

        void CalculateFullPrice()
        {
            Decimal curPrice = 0;

            foreach (Product curProduct in SelectedProducts)
                curPrice += curProduct.Quantity * curProduct.Price;
        }

        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            bool exit = false;
            var a = curProductName.BorderBrush;

            if (curProductName.Text == "")
            {
                curProductName.BorderBrush = Brushes.Red;

                exit = true;
            }

            if (curProductPrice.Text == "")
            {
                curProductPrice.BorderBrush = Brushes.Red;

                exit = true;
            }

            if (curProductQuantity.Text == "")
            {
                curProductQuantity.BorderBrush = Brushes.Red;

                exit = true;
            }

            if (exit)
                return;

            Product newProduct = new Product();

            newProduct.Name = curProductName.Text;
            newProduct.Price = Convert.ToDecimal(curProductPrice.Text);
            newProduct.Quantity = Convert.ToInt32(curProductQuantity.Text);
            List<Product> bufferList = new List<Product>(ExistingProducts);

            ExistingProducts.Add(newProduct);
            bufferList.Add(newProduct);
            int x = newProduct.Quantity;
            for (x = newProduct.Quantity; x == newProduct.Quantity; x++)
            {
                bufferList.Add(newProduct);
            }
            productListBox.ItemsSource = bufferList;
            productListBox.SelectedItem = newProduct;
            curProductName.Text = null;
            curProductPrice.Text = null;
            curProductQuantity.Text = null;
        }

        private void curProductName_TextChanged(object sender, TextChangedEventArgs e)
        {
            curProductName.BorderBrush = Brushes.SlateGray;
        }

        private void curProductPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            curProductPrice.BorderBrush = Brushes.SlateGray;
        }

        private void curProductQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            curProductQuantity.BorderBrush = Brushes.SlateGray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Selected = (TextBlock)productListBox.SelectedItem;

            foreach (var curItem in ExistingProducts)
            {
                if (curItem.Name == Selected.Text)
                {
                    SelectedProducts.Add(curItem);
                }
            }

            var bufferList = new List<Product>(SelectedProducts);

            BasketItems.ItemsSource = bufferList;
        }

        private void addToBasket_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addQuantityFocus(object sender, RoutedEventArgs e)
        {
            addQuantity.Text = null;
        }

    }
}
