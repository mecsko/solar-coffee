import { IInvoice } from "@/types/Invoice";
import axios, { AxiosResponse } from "axios";

export class InvoiceSerice {
  API_URL = process.env.VUE_APP_API_URL_HTTPS;

  public async makeNewInvoice(invoice: IInvoice): Promise<boolean> {
    const now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;
    const result: AxiosResponse = await axios.post(`${this.API_URL}/invoice`, invoice);
    return result.data;
  }
}
