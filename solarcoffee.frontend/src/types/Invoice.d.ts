import { IProduct } from "@/types/Product.d";

export interface IInvoice {
  customerId: number;
  lineItems: ILineItem[];
  createdOn?: Date;
  updatedOn?: Date;
}

export interface ILineItem {
  product: IProduct | null;
  quantity: number;
}
