using Nancy;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ShoeStore
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/brands"] = _ => {
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Get["/stores"] = _ => {
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Get["/brands/new"] = _ => {
        return View["brand_form.cshtml"];
      };

      Get["/stores/new"] = _ => {
        return View["store_form.cshtml"];
      };

      Post["brands/new"] = _ => {
        Brand newBrand = new Brand(Request.Form["name"]);
        newBrand.Save();
        return View["success.cshtml", Brand.GetAll()];
      };

      Post["stores/new"] = _ => {
        Store newStore = new Store(Request.Form["name"]);
        newStore.Save();
        return View["success.cshtml", Store.GetAll()];
      };
      //
      // Get["brands/{id}"] = parameters => {
      //   Brand newBrand = Brand.Find(parameters.id);
      //   List<Brand> AllBrands = new List<Brand>{};
      //   AllBrands.Add(newBrand);
      //   return View["brand.cshtml", AllBrands];
      // };

      Get["brands/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();

        Brand SelectedBrand = Brand.Find(parameters.id);
        List<Store> BrandStores = SelectedBrand.GetStores();
        List<Store> AllStores = Store.GetAll();

        model.Add("brand", SelectedBrand);
        model.Add("brandStores", BrandStores);
        model.Add("allStores", AllStores);

        return View["brand.cshtml", model];
      };

      Get["stores/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();

        Store SelectedStore = Store.Find(parameters.id);
        List<Brand> StoreBrands = SelectedStore.GetBrands();
        List<Brand> AllBrands = new List<Brand>{};

        model.Add("store", SelectedStore);
        model.Add("storeBrands", StoreBrands);
        model.Add("allBrands", AllBrands);

        return View["store.cshtml", model];
      };

      Post["brand/add_store"] = _ => {
        Store store = Store.Find(Request.Form["store-id"]);
        Brand brand = Brand.Find(Request.Form["brand-id"]);
        brand.AddStore(store);
        return View["success.cshtml"];
      };
      Post["store/add_brand"] = _ => {
        Store store = Store.Find(Request.Form["store-id"]);
        Brand brand = Brand.Find(Request.Form["brand-id"]);
        store.AddBrand(brand);
        return View["success.cshtml"];
      };

      // Get["stores/{id}"] = parameters => {
      //   Store newStore = Store.Find(parameters.id);
      //   List<Store> AllStores = new List<Store>{};
      //   AllStores.Add(newStore);
      //   return View["store.cshtml", AllStores];
      // };

    }
  }
}
