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
using System.Data.SqlClient;
using System.Data;

namespace webCRUD
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //LoadGrid();
        }

         

        /*private static void SubmitData(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(@"Data Source=DESKTOP-NF5DL1U\SQLEXPRESS;Initial Catalog=MiniProyecto;Persist Security Info=True"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "submitdata";

                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);

                    command.ExecuteNonQuery();
                }
            }
        }*/

        public void ClearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            city_txt.Clear();
        }

        public void LoadGrid()
        {
            /*SqlCommand cmd = new SqlCommand("SELECT * FROM Person", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;*/

        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        public void isValid()
        {
            if(name_txt.Text == string.Empty)
            {
                MessageBox.Show("The name");
            }
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Database.Connection con = new Database.Connection();
            DataSet ds = new DataSet();

            //con.AddParameters();

            ds = con.ExecuteQueryDS("SelectAllPerson", true, con.ConnectionStringCON_DBCRUD());




        }

    }
}
