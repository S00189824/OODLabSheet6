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

namespace LabSheet6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ex1Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Categories
                        join p in db.Products on c.CategoryName equals p.Category.CategoryName
                        orderby c.CategoryName
                        select new { Category = c.CategoryName, product = p.ProductName };
            var results = query.ToList();

            Ex1GdDisplay.ItemsSource = results;
            Ex1TblkCount.Text = results.Count.ToString();
        }

        private void Ex2Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        orderby p.Category.CategoryName, p.ProductName
                        select new { Category = p.Category.CategoryName, Product = p.ProductName };

            var results = query.ToList();
            Ex2GdDisplay.ItemsSource = results;
            Ex2TblkCount.Text = results.Count.ToString();
        }

        private void Ex3Button_Click(object sender, RoutedEventArgs e)
        {
            var query1 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail);

            var query2 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail.UnitPrice * detail.Quantity);

            int numberOfOrders = query1.Count();
            decimal totalValue = query2.Sum();
            decimal averageValue = query2.Average();

            Ex3TblkCount.Text = string.Format(
                "Total number of orders {0}\nvalue of orders {1:C}\nAverage order Value {2:C}",
                numberOfOrders, totalValue, averageValue);
        }

        private void Ex4Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count >= 20
                        select new
                        {
                            Name = customer.CompanyName,
                            OrderCount = customer.Orders.Count
                        };

            Ex4GdDisplay.ItemsSource = query.ToList();
        }

        private void Ex5Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count < 3
                        select new
                        {
                            Company = customer.CompanyName,
                            City = customer.City,
                            Region = customer.Region,
                            OrderCount = customer.Orders.Count
                        };

            Ex5GdDisplay.ItemsSource = query.ToList();
        }

        private void Ex6Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        orderby customer.CompanyName
                        select customer.CompanyName;

            Ex6LbxEx6Customers.ItemsSource = query.ToList();
        }

        private void Ex6LbxEx6Customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string company = (string)Ex6LbxEx6Customers.SelectedItem;

            if(company != null)
            {
                var query = (from detail in db.Order_Details
                             where detail.Order.Customer.CompanyName == company
                             select detail.UnitPrice * detail.Quantity).Sum();

                Ex6TblkCount.Text = string.Format("total for supplier {0}\n\n{1:C}", company, query);
            }
        }

        private void Ex7Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        group p by p.Category.CategoryName into g
                        orderby g.Count() descending
                        select new
                        {
                            Category = g.Key,
                            Count = g.Count()
                        };

            Ex7GdDisplay.ItemsSource = query.ToList();
        }
    }
}
