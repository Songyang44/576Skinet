import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl='https://localhost:5002/api/';
  shopParams= new ShopParams();
  constructor(private http : HttpClient) { }

  getProduct(ShopParams:ShopParams)
  {
    let params=new HttpParams();
    if(ShopParams.brandId) params=params.append('brandId',ShopParams.brandId);
    if(ShopParams.typeId) params=params.append('typeId',ShopParams.typeId);
    params=params.append('sort',ShopParams.sort);
    params=params.append('pageIndex',ShopParams.pageIndex);
    params=params.append('pageSize',ShopParams.pageSize);

    if(ShopParams.search) params=params.append('search',ShopParams.search);
    return this.http.get<Pagination<Product[]>>(this.baseUrl+"products",{params});
  }

  getProductById(id:number)
  {
    return this.http.get<Product>(this.baseUrl+"products/"+id);
  }

  getBrand()
  {
    return this.http.get<Brand[]>(this.baseUrl+"products/brands")
  }

  getType()
  {
    return this.http.get<Type[]>(this.baseUrl+"products/types")
  }

  setShopParams(params: ShopParams)
  {
    this.shopParams = params;
  }

  getShopParams()
  {
    return this.shopParams;
  }
}
