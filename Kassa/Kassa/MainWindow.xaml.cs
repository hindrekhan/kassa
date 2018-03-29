using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

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
            removeQuantity.Text = "1";
        }

        void InitializeExistingProducts()
        {
            Product curProduct;

            string[] allLines = File.ReadAllLines("../../products.txt");
            
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

            if (exit)
                return;

            Product newProduct = new Product();

            newProduct.Name = curProductName.Text;
            newProduct.Price = Convert.ToDecimal(curProductPrice.Text);
            List<Product> bufferList = new List<Product>(ExistingProducts);

            ExistingProducts.Add(newProduct);
            bufferList.Add(newProduct);
            productListBox.ItemsSource = bufferList;
            productListBox.SelectedItem = newProduct;
            curProductName.Text = null;
            curProductPrice.Text = null;
            productListBox.ScrollIntoView(newProduct);
            SaveProducts();
        }

        private void curProductName_TextChanged(object sender, TextChangedEventArgs e)
        {
            curProductName.BorderBrush = Brushes.SlateGray;
        }

        private void curProductPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            curProductPrice.BorderBrush = Brushes.SlateGray;
        }

        private void addToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (productListBox.SelectedItems.Count == 0)
                return;

            var selectedIndex = productListBox.SelectedIndex;
            var newProduct = ExistingProducts.ElementAt(selectedIndex);

            if (SelectedProducts.Contains(newProduct))
            {
                var indOf = SelectedProducts.IndexOf(newProduct);
                newProduct.Quantity += Int32.Parse(addQuantity.Text);
                SelectedProducts[indOf] = newProduct;
            }

            else
            {
                newProduct.Quantity = Int32.Parse(addQuantity.Text);
                SelectedProducts.Add(newProduct);
            }

            var bufferList = new List<Product>(SelectedProducts);

            BasketItems.ItemsSource = bufferList;
            decimal TotalPrice = 0;
            for (int i = 0; i < bufferList.Count; i++)
            {
                TotalPrice += Convert.ToDecimal(bufferList[i].Price * bufferList[i].Quantity);
            }

            lblTotalPrice.Content = TotalPrice;
        }

        private void removeFromBasket_Click(object sender, RoutedEventArgs e)
        {

            var selectedIndex = BasketItems.SelectedIndex;

            if (selectedIndex == -1)
                return;

            var newProduct = SelectedProducts.ElementAt(selectedIndex);

            if (SelectedProducts[selectedIndex].Quantity <= Int32.Parse(removeQuantity.Text))
            {
                SelectedProducts.RemoveAt(selectedIndex);
            }

            else
            {
                SelectedProducts[selectedIndex].Quantity -= Int32.Parse(removeQuantity.Text);
            }

            List<Product> bufferList = new List<Product>(SelectedProducts);
            BasketItems.ItemsSource = bufferList;

        }

        private void addQuantityFocus(object sender, RoutedEventArgs e)
        {
            addQuantity.Text = null;
        }

        private void removeQuantityFocus(object sender, RoutedEventArgs e)
        {
            removeQuantity.Text = null;
        }

        private void productListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void SaveProducts()
        {
            string[] newData = new string[ExistingProducts.Count];

            for (int i = 0; i < ExistingProducts.Count; i++)
            {
                newData[i] = "\n" + ExistingProducts[i].Name + " " + ExistingProducts[i].Price;
            }

            System.IO.File.WriteAllLines("../../products.txt", newData);
        }
    }
}
