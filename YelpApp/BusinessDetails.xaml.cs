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
using Npgsql;

namespace YelpApp
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        public class Reviews
        {
            public string review_date { get; set; }
            public string name { get; set; }
            public double stars { get; set; }
            public string review_text { get; set; }
            public int funny { get; set; }
            public int useful { get; set; }
            public int cool { get; set; }
            public static double rating_option { get; set; }
        }

        public class FriendReviews
        {
            public string friend_name { get; set; }
            public string friend_review_date { get; set; }
            public string friend_review_text { get; set; }
        }

        // Reviews by users on review page
        private void addColumns5Grid()
        {
            // table Review
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("review_date");
            col1.Header = "Date";
            col1.Width = 80;
            reviewDG.Columns.Add(col1);

            // table yelpUser
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("name");
            col2.Header = "User Name";
            col2.Width = 100;
            reviewDG.Columns.Add(col2);

            // table Review
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("stars");
            col3.Header = "Stars";
            col3.Width = 30;
            reviewDG.Columns.Add(col3);

            // table Review
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("review_text");
            col4.Header = "Text";
            col4.Width = 700;
            reviewDG.Columns.Add(col4);

            // Review
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("funny");
            col5.Header = "Funny";
            col5.Width = 30;
            reviewDG.Columns.Add(col5);
            
            // Review
            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("useful");
            col6.Header = "Useful";
            col6.Width = 30;
            reviewDG.Columns.Add(col6);
            
            // Review
            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("cool");
            col7.Header = "Cool";
            col7.Width = 30;
            reviewDG.Columns.Add(col7);
        }

        // friends who reviewed this business on review page
        private void addColumns7Grid()
        {
            // table Friendship
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("friend_name");
            col1.Header = "User Name";
            col1.Width = 90;
            friendRevDG.Columns.Add(col1);

            // table Review
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("friend_review_date");
            col2.Header = "Date";
            col2.Width = 150;
            friendRevDG.Columns.Add(col2);

            // table Review
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("friend_review_text");
            col3.Header = "Text";
            col3.Width = 900;
            friendRevDG.Columns.Add(col3);
        }

        private string bid = "";
        public BusinessDetails(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            addColumns5Grid();
            addColumns7Grid();
            populateReviews(bid);
            populateFriendReviews(bid);
            loadBusinessDetails();
            loadBusinessNumbers();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password=1234";     // add password
        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();  //always returns single value because bid are unique
                        myf(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addReviewGridRow(NpgsqlDataReader R)
        {
            while(R.Read())
            reviewDG.Items.Add(new Reviews()
            {
                review_date = R.GetDate(0).ToString(),
                name = R.GetString(1),
                stars = R.GetDouble(2),
                review_text = R.GetString(3),
                funny = R.GetInt32(4),
                useful = R.GetInt32(5),
                cool = R.GetInt32(6),
            });
        }

        private void populateReviews(string bid)
        {
            reviewDG.Items.Clear();
            string sqlStr = "SELECT review.review_date, yelpUser.name, review.stars, review.review_text, review.funny, review.useful, review.cool " +
                            "FROM Business, Review, yelpUser " +
                            "WHERE business.business_id = review.business_id AND yelpUser.user_id = review.user_id AND review.business_id = '" + bid + "'; ";
                            //"' ORDER BY review.review_date DESC;";
            executeQuery(sqlStr, addReviewGridRow);
        }

        private void addFriendReviewGridRow(NpgsqlDataReader R)
        {
            if (R.HasRows)
            {
                while (R.Read())
                    friendRevDG.Items.Add(new FriendReviews()
                    {
                        friend_name = R.GetString(0),
                        friend_review_date = R.GetDate(1).ToString(),
                        friend_review_text = R.GetString(2),
                    });
            }
        }

        private void populateFriendReviews(string bid)
        {
            friendRevDG.Items.Clear();
            string sqlStr = "SELECT name, review_date, review_text, stars, useful, funny, cool " +
                            "FROM Review as R " +
                            "INNER JOIN(SELECT user_id, name FROM yelpUser) as U ON U.user_id = R.user_id " +
                            "WHERE business_id = '" + bid +
                            "' AND R.user_id IN(SELECT users_friend_id FROM Friendship WHERE user_id = '" + MainWindow.YelpUser.current_user_id + "');";
            executeQuery(sqlStr, addFriendReviewGridRow);
        }

        private void setBusinessDetails(NpgsqlDataReader R)
        {
            bname.Text = R.GetString(0);
            state.Text = R.GetString(1);
            city.Text = R.GetString(2);
            currentUserTB.Text = MainWindow.YelpUser.current_user_id;
        }

        private void setNumInState(NpgsqlDataReader R)
        {
            numInState.Content = R.GetInt16(0).ToString();
        }

        private void setNumInCity(NpgsqlDataReader R)
        {
            numInCity.Content = R.GetInt16(0).ToString();
        }

        private void loadBusinessNumbers()
        {
            string sqlStr1 ="SELECT count(*) from business " +
                            "WHERE state = (" +
                                "SELECT state " +
                                "FROM business " +
                                "WHERE business_id = '" + this.bid + "');";
            executeQuery(sqlStr1, setNumInState);
            string sqlStr2 = "SELECT count(*) from business WHERE city = (SELECT city FROM business WHERE business_id = '" + this.bid + "');";
            executeQuery(sqlStr2, setNumInCity);
        }

        private void loadBusinessDetails()
        {
            string sqlStr = "SELECT name, state, city FROM business WHERE business_id = '" + this.bid + "';";
            executeQuery(sqlStr, setBusinessDetails);
            string sqlStr2 = "SELECT review_text FROM review WHERE business_id = '" + this.bid + "';";
            executeQuery(sqlStr, setBusinessDetails);
        }

        private void addReview(NpgsqlDataReader R)
        {

        }

        private void addReviewButton_Click(object sender, RoutedEventArgs e)
        {
            string rand_review_id = System.Guid.NewGuid().ToString().Substring(0, 22);
            string date = DateTime.Now.ToString();

            string sqlStr = "INSERT INTO Review(review_id, user_id, business_id, review_date, review_text, stars, useful, funny,cool) " +
                            "VALUES ('" + rand_review_id + "','" + MainWindow.YelpUser.current_user_id + "','" + this.bid + "','" + date + "','" + addReviewTB.Text + "','" + Reviews.rating_option + "', 0,0,0);";
            executeQuery(sqlStr, addReview);
            populateReviews(this.bid);
        }

        // populate rating combobox
        private void addRatingCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addRatingCB.SelectedIndex > -1)
            {
                if (addRatingCB.SelectedIndex.Equals(0))
                    Reviews.rating_option = 0;
                else if (addRatingCB.SelectedIndex.Equals(1))
                    Reviews.rating_option = 1;
                else if (addRatingCB.SelectedIndex.Equals(2))
                    Reviews.rating_option = 2;
                else if (addRatingCB.SelectedIndex.Equals(3))
                    Reviews.rating_option = 3;
                else if (addRatingCB.SelectedIndex.Equals(4))
                    Reviews.rating_option = 4;
                else if (addRatingCB.SelectedIndex.Equals(5))
                    Reviews.rating_option = 5;
            }
        }
    }
}
