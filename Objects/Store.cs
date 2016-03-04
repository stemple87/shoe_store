using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ShoeStore
{
  public class Store
  {
    private int _id;
    private string _storeName;


    public Store(string StoreName, int Id = 0)
    {
      _id = Id;
      _storeName = StoreName;

    }

    public override bool Equals(System.Object otherStore)
    {
      if (!(otherStore is Store))
      {
        return false;
      }
      else {
        Store newStore = (Store) otherStore;
        bool IdEquality = this.GetId() == newStore.GetId();
        bool StoreNameEquality = this.GetStoreName() == newStore.GetStoreName();


        return (IdEquality && StoreNameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetStoreName()
    {
      return _storeName;
    }
    public void SetStoreName(string newName)
    {
      _storeName = newName;
    }

    public static List<Store> GetAll()
    {
      List<Store> AllStores = new List<Store>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int StoreId = rdr.GetInt32(0);
        string StoreStoreName = rdr.GetString(1);

        Store NewStore = new Store(StoreStoreName, StoreId);
        AllStores.Add(NewStore);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllStores;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores (name) OUTPUT INSERTED.id VALUES (@StoreName); INSERT INTO store_brand (store_id) OUTPUT INSERTED.id VALUES (@StoreId);", conn);

      SqlParameter storeNameParam = new SqlParameter();
      storeNameParam.ParameterName = "@StoreName";
      storeNameParam.Value = this.GetStoreName();
      cmd.Parameters.Add(storeNameParam);

      SqlParameter storeIdParam = new SqlParameter();
      storeIdParam.ParameterName = "@StoreId";
      storeIdParam.Value = this.GetId();
      cmd.Parameters.Add(storeIdParam);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Store Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores WHERE id = @StoreId", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(storeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStoreId = 0;
      string foundStoreName = null;


      while(rdr.Read())
      {
        foundStoreId = rdr.GetInt32(0);
        foundStoreName = rdr.GetString(1);

      }
      Store foundStore = new Store(foundStoreName, foundStoreId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStore;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM stores WHERE id = @StoreId; DELETE FROM store_brand WHERE store_id = @StoreId;", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(storeIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddBrand(Brand newBrand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO store_brand (brand_id, store_id) VALUES (@BrandId, @StoreId)", conn);  //needs more stuff - brand relationship
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = newBrand.GetId();
      cmd.Parameters.Add(brandIdParameter);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Brand> GetBrands()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT brands.* FROM stores JOIN store_brand ON (stores.id = store_brand.store_id) JOIN brands ON (store_brand.brand_id = brands.id) WHERE stores.id = @StoreId", conn);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      rdr = cmd.ExecuteReader();

      List<Brand> brands = new List<Brand> {};
      int brandId = 0;
      string brandTitle = null;


      while (rdr.Read())
      {
        brandId = rdr.GetInt32(0);
        brandTitle = rdr.GetString(1);
        Brand brand = new Brand(brandTitle, brandId);
        brands.Add(brand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      return brands;
    }
  }
}
