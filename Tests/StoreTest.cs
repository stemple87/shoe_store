using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoeStore
{
  public class BrandTest : IDisposable
  {
    public BrandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_BrandsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Brand.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Brand firstBrand = new Brand("Life of the Past: Oregon");
      Brand secondBrand = new Brand("Life of the Past: Oregon");

      //Assert
      Assert.Equal(firstBrand, secondBrand);
    }

    [Fact]
    public void Save_SavesBrandToDatabase()
    {
       //Arrange
       Brand testBrand = new Brand("Linguistical Problem Solving Programs");
       testBrand.Save();

       //Act
       List<Brand> result = Brand.GetAll();
       List<Brand> testList = new List<Brand>{testBrand};

       //Assert
       Assert.Equal(testList, result);
    }
//
    [Fact]
    public void Save_AssignsIdToBrandObject()
    {
      //Arrange
      Brand testBrand = new Brand("Enterin' SQL Commands");
      testBrand.Save();

      //Act
      Brand savedBrand = Brand.GetAll()[0];

      int result = savedBrand.GetId();
      int testId = testBrand.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsBrandInDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Screwin' Up Da'bases With Chris");
      testBrand.Save();

      //Act
      Brand foundBrand = Brand.Find(testBrand.GetId());

      //Assert
      Assert.Equal(testBrand, foundBrand);
    }

    [Fact]
    public void Delete_DeletesBrandFromDatabase()
    {
      List<Brand> resultBrands = Brand.GetAll();
      //Arrange
      Brand testBrand = new Brand("Best Brand EVAH");
      testBrand.Save();
      testBrand.Delete();

      List<Brand> otherResultBrands = Brand.GetAll();

      //Assert
      Assert.Equal(otherResultBrands, resultBrands);
    }

    [Fact]
    public void Delete_DeletesBrandBrandAndStoresFromDatabase()
    {
      //Arrange
      Store testStore = new Store("Perilous Dungeons");
      testStore.Save();


      Brand testBrand = new Brand("Wet doggie noses");
      testBrand.Save();

      //Act
      testBrand.AddBrandStore(testStore);
      testBrand.Delete();

      List<Brand> resultStoreBrands = testStore.GetBrands();
      List<Brand> testStoreBrands = new List<Brand> {};

      //Assert
      Assert.Equal(testStoreBrands, resultStoreBrands);
    }


    [Fact]
    public void AddBrandStore_AddsStoreToBrand()
    {
      //Arrange
      Brand testBrand = new Brand("David Copperfield");
      testBrand.Save();

      Store testStore = new Store ("Harry Henderson");
      testStore.Save();

      Store testStore2 = new Store ("Sally Henderson");
      testStore2.Save();

      //Act
      testBrand.AddBrandStore(testStore);
      testBrand.AddBrandStore(testStore2);

      List<Store> result = testBrand.GetStores();
      List<Store> testList = new List<Store>{testStore, testStore2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetStores_RetrievesAllStoresWithBrand()
    {
      Brand testBrand = new Brand("How It Be");
      testBrand.Save();

      Store firstStore = new Store("Pippi Longstocking");
      firstStore.Save();
      testBrand.AddBrandStore(firstStore);

      Store secondStore = new Store("Matilda");
      secondStore.Save();
      testBrand.AddBrandStore(secondStore);

      List<Store> testStoreList = new List<Store> {firstStore, secondStore};
      List<Store> resultStoreList = testBrand.GetStores();

      Assert.Equal(testStoreList, resultStoreList);
    }

    public void Dispose()
    {
      Store.DeleteAll();
      Brand.DeleteAll();
    }
  }
}
