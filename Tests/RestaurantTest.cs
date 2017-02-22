using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantListApp
{
    public class RestaurantTest : IDisposable
    {
        public RestaurantTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=food_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_DatabaseEmptyAtFirst()
        {
            //Arrange, Act
            int result = Restaurant.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfParametersAreTheSame()
        {
            //Arrange, Act
            Restaurant firstRestaurant = new Restaurant("Tony's", 5, "Sicily");
            Restaurant secondRestaurant = new Restaurant("Tony's", 5, "Sicily");

            //Assert
            Assert.Equal(firstRestaurant, secondRestaurant);
        }

        [Fact]
        public void Test_Save_SavesToDatabase()
        {
            //Arrange
            Restaurant testRestaurant = new Restaurant("Tony's", 5, "Sicily");

            //Act
            testRestaurant.Save();
            List<Restaurant> result = Restaurant.GetAll();
            List<Restaurant> testList = new List<Restaurant>{testRestaurant};

            //Assert
            Assert.Equal(testList, result);
        }

        public void Dispose()
        {
            Restaurant.DeleteAll();
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
            //Arrange
            Restaurant testRestaurant = new Restaurant("Tony's", 5, "Sicily");

            //Act
            testRestaurant.Save();
            Restaurant savedRestaurant = Restaurant.GetAll()[0];

            int result = savedRestaurant.GetId();
            int testId = testRestaurant.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_Find_FindsRestaurantInDatabase()
        {
            //Arrange
            Restaurant testRestaurant = new Restaurant("Tony's", 5, "Sicily");
            testRestaurant.Save();

            //Act
            Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

            //Assert
            Assert.Equal(testRestaurant, foundRestaurant);
        }
    }
}
