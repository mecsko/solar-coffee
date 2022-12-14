import axios from "axios";
import { ICustomer } from "@/types/Customer.d";
import { IServiceResponse } from "@/types/ServiceResponse";
import { Service } from "./service";

/**
 * Customer Service
 * Provices UI business logic associated with Customers in our system
 */
export class CustomerService extends Service {
  public async getCustomers(): Promise<IServiceResponse<ICustomer[]>> {
    const response: IServiceResponse<ICustomer[]> = await axios.get(`${this.API_URL}/customer`);
    return response;
  }

  public async addCustomer(newCustomer: ICustomer): Promise<IServiceResponse<ICustomer>> {
    const response: IServiceResponse<ICustomer> = await axios.post(
      `${this.API_URL}/customer`,
      newCustomer
    );
    return response;
  }

  public async deleteCustomer(customerId: number): Promise<IServiceResponse<boolean>> {
    const response: IServiceResponse<boolean> = await axios.delete(
      `${this.API_URL}/customer/${customerId}`
    );
    return response;
  }
}
