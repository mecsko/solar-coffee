import { ICustomer } from "@/types/Customer.d";
import { ILineItem } from "./Invoice";
export interface ISalesOrder {
  id: number;
  createdOn: Date;
  updatedOn?: Date;
  customer: ICustomer;
  isPaid: boolean;
  salesOrderItems: ILineItem[];
}
