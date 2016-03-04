using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ShoeStore
{
  public class Brand
  {
    private int _id;
    private string _brandName;


    public Brand(string BrandName, int Id = 0)
    {
      _id = Id;
      _brandName = BrandName;
    }

    public override bool Equals(System.Object otherBrand)
    {
      if (!(otherBrand is Brand))
      {
        return false;
      }
      else
      {
        Brand newBrand = (Brand) otherBrand;
        bool idEquality = this.GetId() == newBrand.GetId();
        bool brandNameEquality = this.GetBrandName() == newBrand.GetBrandName();
        return (idEquality && brandNameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetBrandName()
    {
    return _brandName;
    }
    public void SetBrandName(string newBrandName)
    {
      _brandName = newBrandName;
    }

    public static List<Brand> GetAll()
    {
      List<Brand> allBrands = new List<Brand>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int brandId = rdr.GetInt32(0);
        string brandBrandName = rdr.GetString(1);

        Brand newBrand = new Brand(brandBrandName, brandId);
        allBrands.Add(newBrand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allBrands;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brands (name) OUTPUT INSERTED.id VALUES (@BrandName); INSERT INTO store_brand (brand_id) OUTPUT INSERTED.id VALUES (@BrandId)", conn);

      SqlParameter brandNameParameter = new SqlParameter();
      brandNameParameter.ParameterName = "@BrandName";
      brandNameParameter.Value = this.GetBrandName();
      cmd.Parameters.Add(brandNameParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      rdr = cmd.ExecuteReader();
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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Brand Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands WHERE id = @BrandID;", conn);
      SqlParameter BrandIDParemeter = new SqlParameter();
      BrandIDParemeter.ParameterName = "@BrandId";
      BrandIDParemeter.Value = id.ToString();
      cmd.Parameters.Add(BrandIDParemeter);
      rdr = cmd.ExecuteReader();

      int foundBrandId = 0;
      string foundBrandBrandName = null;

      while(rdr.Read())
      {
        foundBrandId = rdr.GetInt32(0);
        foundBrandBrandName = rdr.GetString(1);

      }
      Brand foundBrand = new Brand(foundBrandBrandName, foundBrandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBrand;
    }

    public void AddBrandStore(Store newStore)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO store_brand (brand_id, store_id) VALUES (@BrandId, @StoreId)", conn);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = newStore.GetId();
      cmd.Parameters.Add(storeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Store> GetStores()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT stores.* FROM brands JOIN store_brand ON (brands.id = store_brand.brand_id) JOIN stores ON (store_brand.store_id = stores.id) WHERE brands.id = @BrandId", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      rdr = cmd.ExecuteReader();

      List<Store> stores = new List<Store> {};
      int storeId = 0;
      string storeStoreName = null;


      while(rdr.Read())
      {
        storeId = rdr.GetInt32(0);
        storeStoreName = rdr.GetString(1);

        Store store = new Store(storeStoreName, storeId);
        stores.Add(store);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return stores;
    }

    public void Delete()
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("DELETE FROM brands WHERE id = @BrandId; DELETE FROM store_brand WHERE brand_id = @BrandId;", conn);
       SqlParameter brandIdParameter = new SqlParameter();
       brandIdParameter.ParameterName = "@BrandId";
       brandIdParameter.Value = this.GetId();

       cmd.Parameters.Add(brandIdParameter);
       cmd.ExecuteNonQuery();

       if (conn != null)
       {
         conn.Close();
       }
     }

     public void AddStore(Store newStore)
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("INSERT INTO store_brand (brand_id, store_id) VALUES (@BrandId, @StoreId)", conn);
       SqlParameter storeIdParameter = new SqlParameter();
       storeIdParameter.ParameterName = "@StoreId";
       storeIdParameter.Value = newStore.GetId();
       cmd.Parameters.Add(storeIdParameter);

       SqlParameter brandIdParameter = new SqlParameter();
       brandIdParameter.ParameterName = "@BrandId";
       brandIdParameter.Value = this.GetId();
       cmd.Parameters.Add(brandIdParameter);

       cmd.ExecuteNonQuery();

       if (conn != null)
       {
         conn.Close();
       }
     }

     public void Update(string newName)
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr;
       conn.Open();

       SqlCommand cmd = new SqlCommand("UPDATE brands SET name = @NewName OUTPUT INSERTED.name WHERE id = @brandId;", conn);

       SqlParameter newNameParameter = new SqlParameter();
       newNameParameter.ParameterName = "@NewName";
       newNameParameter.Value = newName;
       cmd.Parameters.Add(newNameParameter);


       SqlParameter brandIdParameter = new SqlParameter();
       brandIdParameter.ParameterName = "@brandId";
       brandIdParameter.Value = this.GetId();
       cmd.Parameters.Add(brandIdParameter);
       rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         this._brandName = rdr.GetString(0);
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

    public override int GetHashCode()
    {
      return 0;
    }

  }
}
