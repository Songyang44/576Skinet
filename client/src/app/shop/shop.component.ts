import { Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?:ElementRef;
  products: Product[]=[];
  brands: Brand[]=[];
  types: Type[]=[];
  brandIdSelected=0;
  typeIdSelected=0;

  shopParams=new ShopParams();
  
  sortSelected = "name";
  sortOptions=
  [
    {name:'Alphabetical',value:'name'},
    {name:'Price: Low to High: ',value:'priceAsc'},
    {name:'Price: High to Low',value:'priceDesc'},
  ]

  totalCount=0;
  constructor(private shopService:ShopService){}
  ngOnInit(): void {
    this.getProduct();
    this.getBrands();
    this.getTypes();
  }

  getProduct(){
    this.shopService.getProduct(this.shopParams).subscribe
    ({
      next: response=> 
      {
        this.products=response.data,
        this.shopParams.pageIndex=response.pageIndex;
        this.shopParams.pageSize=response.pageSize;
        this.totalCount=response.count;
      },
      error: error=>console.log(error)
    })
  }

  getBrands(){
    this.shopService.getBrand().subscribe
    ({
      next:response=> this.brands=[{id:0,name:"All"}, ...response],
      error:error=>console.log(error)

    })
  }

  getTypes(){
    this.shopService.getType().subscribe
    ({
      next:response=> this.types=[{id:0,name:"All"}, ...response],
      error:error=>console.log(error)

    })
  }

  onBrandSelected(brandId:number)
  {
    const params = this.shopService.getShopParams();
    params.brandId=brandId;
    params.pageIndex=1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProduct();
  }

  onTypeSelected(typeId:number)
  {
    const params = this.shopService.getShopParams();
    params.typeId=typeId;
    params.pageIndex=1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProduct();
  }

  onSortSelected(event:any)
  {
    const params = this.shopService.getShopParams();
    params.sort=event.target.value;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProduct();
  }

  onPageChanged(event:any)
  {
    if(this.shopParams.pageIndex!==event)
    {
      this.shopParams.pageIndex=event;
      this.getProduct();
    }
    
  }

  onSearch()
  {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm?.nativeElement.value;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProduct();
  }

  onReset()
  {
    if(this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams=new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProduct();

  }

}
