using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoeStore
{
  public class StoreTest : IDisposable
  {
    public StoreTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void DatabaseEmptyAtFirst()
    {
      //Arange, Act
      int result = Store.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Store firstStore = new Store("Bob Marley");
      Store secondStore = new Store("Bob Marley");

      //Assert
      Assert.Equal(firstStore, secondStore);
    }

    [Fact]
    public void Save()
    {
      //Arrange
      Store testStore = new Store("Peter Tosh");
      testStore.Save();

      //Act
      List<Store> result = Store.GetAll();
      List<Store> testList = new List<Store>{testStore};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void SaveAssignsIdToObject()
    {
      //Arrange
      Store testStore = new Store("Bill Clinton");
      testStore.Save();

      //Act
      Store savedStore = Store.GetAll()[0];

      int result = savedStore.GetId();
      int testId = testStore.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void FindFindsStoreInDatabase()
    {
      //Arrange
      Store testStore = new Store("Peter Griffin");
      testStore.Save();

      //Act
      Store result = Store.Find(testStore.GetId());

      //Assert
      Assert.Equal(testStore, result);
    }

    [Fact]
    public void AddBrand_AddsBrandToStore()
    {
      //Arrange
      Brand testBrand = new Brand("Evening TV");
      testBrand.Save();

      Store testStore = new Store("Brian Griffin");
      testStore.Save();

      //Act
      testStore.AddBrand(testBrand);

      List<Brand> result = testStore.GetBrands();
      List<Brand> testList = new List<Brand>{testBrand};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetBrands_ReturnsAllStoreBrands()
    {
      //Arrange
      Store testStore = new Store("Mr. Ed");
      testStore.Save();

      Brand testBrand1 = new Brand("Mr. Magoo - Into the 22nd Century");
      testBrand1.Save();

      Brand testBrand2 = new Brand("Dreaming With Genie");
      testBrand2.Save();

      //Act
      testStore.AddBrand(testBrand1);
      testStore.AddBrand(testBrand2);
      List<Brand> result = testStore.GetBrands();
      List<Brand> testList = new List<Brand> {testBrand1, testBrand2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesStoreBrandStoresFromDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Bees and their Stingers");
      testBrand.Save();

      Store testStore = new Store("Farmer John");
      testStore.Save();

      //Act
      testStore.AddBrand(testBrand);
      testStore.Delete();

      List<Store> resultBrandStores = testBrand.GetStores();
      List<Store> testBrandStores = new List<Store> {};

      //Assert
      Assert.Equal(testBrandStores, resultBrandStores);
    }

    public void Dispose()
    {
      Store.DeleteAll();
      Brand.DeleteAll();
    }
  }
}
