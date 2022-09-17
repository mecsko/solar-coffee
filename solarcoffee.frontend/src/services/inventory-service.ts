import { IShipment } from "@/types/Shipment.d";
import { IProductInventory } from "@/types/Product.d";
import axios, { AxiosResponse } from "axios";
import { IInventoryTimeline } from "./../types/InventoryGraphs.d";
import { Service } from "./service";

/**
 * Inventory Service
 * Proviced UI business logic associated with product inventory
 */
export class InventoryService extends Service {
  public async getInventory(): Promise<IProductInventory[]> {
    const result: AxiosResponse = await axios.get(`${this.API_URL}/inventory/`);
    return result.data;
  }

  public async updateInventoryQuantity(shipment: IShipment): Promise<any> {
    const result: AxiosResponse = await axios.patch(`${this.API_URL}/inventory/`, shipment);
    return result.data;
  }

  public async getSnapshotHistory(): Promise<IInventoryTimeline> {
    const result: AxiosResponse = await axios.get(`${this.API_URL}/inventory/snapshot`);
    return result.data;
  }
}
