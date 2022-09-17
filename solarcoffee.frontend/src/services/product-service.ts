import { IProduct } from "@/types/Product.d";
import axios, { AxiosResponse } from "axios";
import { Service } from "./service";

/**
 * Inventory Service
 * Proviced UI business logic associated with product inventory
 */
export class ProductService extends Service {
  public async archive(productId: number): Promise<any> {
    const result: AxiosResponse = await axios.patch(`${this.API_URL}/product/${productId}`);
    return result.data;
  }

  public async save(newProduct: IProduct): Promise<any> {
    const result: AxiosResponse = await axios.post(`${this.API_URL}/product`, newProduct);
    return result.data;
  }
}
