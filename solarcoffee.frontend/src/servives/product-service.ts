import { IProduct } from "@/types/Product.d";
import axios, { AxiosResponse } from "axios";

/**
 * Inventory Service
 * Proviced UI business logic associated with product inventory
 */
export class ProductService {
  API_URL = process.env.VUE_APP_API_URL_HTTPS;

  public async archive(productId: number): Promise<any> {
    const result: AxiosResponse = await axios.patch(`${this.API_URL}/product/${productId}`);
    console.log("archive result: ", result);
    return result.data;
  }

  public async save(newProduct: IProduct): Promise<any> {
    const result: AxiosResponse = await axios.post(`${this.API_URL}/product`, newProduct);
    console.log("new product result: ", result);
    return result.data;
  }
}
