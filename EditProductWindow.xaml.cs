using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        Product currentProduct;

        bool newProductMode = false;

        public List<Supplier> Suppliers;
        public List<Producer> Producers;
        public List<Category> Categories;
        public EditProductWindow(int id)
        {
            InitializeComponent();

            using (DemoContext db = new())
            {
                Suppliers = db.Suppliers.ToList();
                Producers = db.Producers.ToList();
                Categories = db.Categories.ToList();
                
                currentProduct = db.Products.Where(p => p.Id == id).FirstOrDefault();


            }

            if (currentProduct is null)
            {
                currentProduct = new();
                newProductMode = true;
            }

            SupplierComboBox.ItemsSource = Suppliers;
            SupplierComboBox.DisplayMemberPath = "Name";
            SupplierComboBox.SelectedValuePath = "Id";
            SupplierComboBox.SelectedValue = currentProduct.SupplierId;

            ProducerComboBox.ItemsSource = Producers;
            ProducerComboBox.DisplayMemberPath = "Name";
            ProducerComboBox.SelectedValuePath = "Id";
            ProducerComboBox.SelectedValue = currentProduct.ProducerId;

            CategoryComboBox.ItemsSource = Categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
            CategoryComboBox.SelectedValue = currentProduct.CategoryId;

            DataContext = currentProduct;

            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CatalogWindow.EditMode = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            return;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            currentProduct.CategoryId = (int)CategoryComboBox.SelectedValue;
            currentProduct.SupplierId = (int)SupplierComboBox.SelectedValue;
            currentProduct.ProducerId = (int)ProducerComboBox.SelectedValue;
            using (DemoContext db = new())
            {
                if (newProductMode)
                {
                    CatalogWindow.Products.Add(currentProduct);
                    db.Products.Add(currentProduct);
                }
                else
                {
                    db.Products.Update(currentProduct);
                }
                db.SaveChanges();
            }
            CatalogWindow.Instance.Update();
            Close();
        }
    }
}
