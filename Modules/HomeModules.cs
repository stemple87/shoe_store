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
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Post["stores/new"] = _ => {
        Store newStore = new Store(Request.Form["name"]);
        newStore.Save();
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };


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
        List<Brand> AllBrands = Brand.GetAll();
        model.Add("store", SelectedStore);
        model.Add("storeBrands", StoreBrands);
        model.Add("allBrands", AllBrands);
        return View["store.cshtml", model];
      };

      Post["brand/add_store"] = _ => {
        Store store = Store.Find(Request.Form["store-id"]);
        Brand brand = Brand.Find(Request.Form["brand-id"]);
        brand.AddStore(store);
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Post["store/add_brand"] = _ => {
        Store store = Store.Find(Request.Form["store-id"]);
        Brand brand = Brand.Find(Request.Form["brand-id"]);
        store.AddBrand(brand);
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Delete["/brands/delete"] = _ => {
        Brand.DeleteAll();
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Delete["/stores/delete"] = _ => {
        Store.DeleteAll();
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Get["/stores/edit/{id}"] = parameters => {
        Store SelectedStore = Store.Find(parameters.id);
        return View["store_edit.cshtml", SelectedStore];
      };

      Patch["/stores/edit/{id}"] = parameters => {
        Store SelectedStore = Store.Find(parameters.id);
        SelectedStore.Update(Request.Form["store-name"]);
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Get["stores/delete/{id}"] = parameters => {
        Store SelectedStore = Store.Find(parameters.id);
        return View["store_delete.cshtml", SelectedStore];
      };
      Delete["stores/delete/{id}"] = parameters => {
        Store SelectedStore = Store.Find(parameters.id);
        SelectedStore.Delete();
        List<Store> AllStores = Store.GetAll();
        return View["stores.cshtml", AllStores];
      };

      Get["/brands/edit/{id}"] = parameters => {
        Brand SelectedBrand = Brand.Find(parameters.id);
        return View["brand_edit.cshtml", SelectedBrand];
      };

      Patch["/brands/edit/{id}"] = parameters => {
        Brand SelectedBrand = Brand.Find(parameters.id);
        SelectedBrand.Update(Request.Form["brand-name"]);
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };

      Get["brands/delete/{id}"] = parameters => {
        Brand SelectedBrand = Brand.Find(parameters.id);
        return View["brand_delete.cshtml", SelectedBrand];
      };
      Delete["brands/delete/{id}"] = parameters => {
        Brand SelectedBrand = Brand.Find(parameters.id);
        SelectedBrand.Delete();
        List<Brand> AllBrands = Brand.GetAll();
        return View["brands.cshtml", AllBrands];
      };


    }
  }
}
