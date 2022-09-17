import { IInvoice } from "@/types/Invoice";
import axios, { AxiosResponse } from "axios";
import { Service } from "./service";

export class InvoiceSerice extends Service {
  public async makeNewInvoice(invoice: IInvoice): Promise<boolean> {
    const now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;
    const result: AxiosResponse = await axios.post(`${this.API_URL}/invoice`, invoice);
    return result.data;
  }
}
