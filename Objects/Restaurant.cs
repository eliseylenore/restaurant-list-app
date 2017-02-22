using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantListApp
{
    public class Restaurant
    {
        private int _id;
        private string _name;
        private int _rating;
        private string _city;

        public Restaurant(string Name, int Rating, string City, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _rating = Rating;
            _city = City;
        }
        public override bool Equals(System.Object otherRestaurant)
        {
            if (!(otherRestaurant is Restaurant))
            {
                return false;
            }
            else
            {
                Restaurant newRestaurant = (Restaurant) otherRestaurant;
                bool idEquality = this.GetId() == newRestaurant.GetId();
                bool nameEquality = (this.GetName() == newRestaurant.GetName());
                bool ratingEquality = (this.GetRating() == newRestaurant.GetRating());
                bool cityEquality = (this.GetCity() == newRestaurant.GetCity());
                return (idEquality && nameEquality && ratingEquality && cityEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }
        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public int GetRating()
        {
            return _rating;
        }

        public void SetRating(int newRating)
        {
            _rating = newRating;
        }

        public string GetCity()
        {
            return _city;
        }

        public void SetCity(string newCity)
        {
            _city = newCity;
        }
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, rating, city) OUTPUT INSERTED.id VALUES (@RestaurantName, @RestaurantRating, @RestaurantCity);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@RestaurantName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);

            SqlParameter ratingParameter = new SqlParameter();
            ratingParameter.ParameterName = "@RestaurantRating";
            ratingParameter.Value = this.GetRating();
            cmd.Parameters.Add(ratingParameter);

            SqlParameter cityParameter = new SqlParameter();
            cityParameter.ParameterName = "@RestaurantCity";
            cityParameter.Value = this.GetCity();
            cmd.Parameters.Add(cityParameter);


            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
        public static List<Restaurant> GetAll()
        {
            List<Restaurant> allRestaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int restaurantId = rdr.GetInt32(0);
                string restaurantName = rdr.GetString(1);
                int restaurantRating = rdr.GetInt32(2);
                string restaurantCity = rdr.GetString(3);
                Restaurant newRestaurant = new Restaurant(restaurantName, restaurantRating, restaurantCity, restaurantId);
                allRestaurants.Add(newRestaurant);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allRestaurants;
        }

    }
}
