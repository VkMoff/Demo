using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Test3
{
    /// <summary>
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public static CatalogWindow Instance;
        public static bool EditMode = false;
        public static List<Product> Products = new();
        public List<Product> AllProducts = new();
        Dictionary<int, string> suppliersSort = new();
        public CatalogWindow()
        {
            InitializeComponent();

            Instance = this;

            if (Session.CurrentUser == null)
            {
                NameLabel.Content = "Гость";
            }
            else
            {
                NameLabel.Content = Session.CurrentUser.Name;

                if ((Session.CurrentUser.RoleId == 1) || (Session.CurrentUser.RoleId == 2))
                {
                    AdminPanel.Visibility = Visibility.Visible;
                }
            }

            using (DemoContext db = new())
            {
                AllProducts = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Include(p => p.Producer)
                    .ToList();
                Products = AllProducts.ToList();
                ProductsControl.ItemsSource = Products;

                suppliersSort.Add(-1, "Все поставщики");
                foreach (var s in db.Suppliers.ToList())
                {
                    suppliersSort.Add(s.Id, s.Name);
                }

                SuppliersComboBox.ItemsSource = suppliersSort;
                SuppliersComboBox.SelectedValuePath = "Key";
                SuppliersComboBox.DisplayMemberPath = "Value";
                SuppliersComboBox.SelectedValue = -1;
            }

            SortAmountComboBox.ItemsSource = new string[3] { "По умолчанию", "По возрастанию", "По убыванию"};
            SortAmountComboBox.SelectedIndex = 0;
        }

        private void ToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;
            new MainWindow().Show();
            Close();
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (EditMode) return;
            
            new EditProductWindow((int)((Border)sender).Tag).Show();
            EditMode = true;
        }

        public void Update()
        {
            using (DemoContext db = new())
            {
                Products = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Include(p => p.Producer)
                    .ToList();
                ProductsControl.ItemsSource = Products;
            }
        }

        public void Sort()
        {
            Products = AllProducts;

            if ((int)SuppliersComboBox.SelectedValue != -1)
            {
                Products = Products.Where(p => p.SupplierId == (int)(SuppliersComboBox.SelectedValue)).ToList();
            }

            if (SortAmountComboBox.SelectedIndex == 1)
            {
                Products = Products.OrderBy(p => p.Amount).ToList();
            }
            else if (SortAmountComboBox.SelectedIndex == 2)
            {
                Products = Products.OrderByDescending(p => p.Amount).ToList();
            }

            if (!SearchTextBox.Text.IsWhiteSpace())
            {
                string search = SearchTextBox.Text;
                Products = Products.Where(
                    p => p.Name.ToLower().Contains(search.ToLower()) ||
                    p.Article.ToLower().Contains(search.ToLower()) ||
                    p.Category.Name.ToLower().Contains(search.ToLower()) ||
                    p.Measurement.ToLower().Contains(search.ToLower()) ||
                    p.Supplier.Name.ToLower().Contains(search.ToLower()) ||
                    p.Producer.Name.ToLower().Contains(search.ToLower()) ||
                    p.Description.ToLower().Contains(search.ToLower())
                    ).ToList();
            }
            ProductsControl.ItemsSource = Products;
        }

        private void Sort(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }
    }
}
