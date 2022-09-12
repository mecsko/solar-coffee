import axios, { AxiosResponse } from "axios";
import { ISalesOrder } from "@/types/SalesOrder";

/**
 * Order Service
 * Provides UI business logic associated with sales orders.
 */
export class OrderService {
  API_URL = process.env.VUE_APP_API_URL_HTTPS;

  public async getOrders(): Promise<ISalesOrder[]> {
    const response: AxiosResponse = await axios.get(`${this.API_URL}/order`);
    return response.data;
  }

  public async markOrderComplete(id: number): Promise<any> {
    const result = await axios.patch(`${this.API_URL}/order/complete/${id}`);
    return result.data;
  }
}
