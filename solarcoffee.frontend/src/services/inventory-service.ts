import { IShipment } from "@/types/Shipment.d";
import { IProductInventory } from "@/types/Product.d";
import axios, { AxiosResponse } from "axios";

/**
 * Inventory Service
 * Proviced UI business logic associated with product inventory
 */
export class InventoryService {
  API_URL = process.env.VUE_APP_API_URL_HTTPS;

  public async getInventory(): Promise<IProductInventory[]> {
    const result: AxiosResponse = await axios.get(`${this.API_URL}/inventory/`);
    return result.data;
  }

  public async updateInventoryQuantity(shipment: IShipment): Promise<any> {
    const result: AxiosResponse = await axios.patch(`${this.API_URL}/inventory/`, shipment);
    return result.data;
  }
}
