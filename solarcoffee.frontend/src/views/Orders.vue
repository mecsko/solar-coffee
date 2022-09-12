<template>
  <div>
    <h1 id="orders-title">Sales Orders</h1>
    <hr />
    <table v-if="orders.length" id="sales-orders" class="table">
      <thead>
        <tr>
          <th>CustomerId</th>
          <th>OrderId</th>
          <th>Order Total</th>
          <th>Order Status</th>
          <th>Mark Complete</th>
        </tr>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.customer.id }}</td>
          <td>{{ order.id }}</td>
          <td>{{ runningTotal(order.id) }}</td>
          <td :class="{ green: order.isPaid }">{{ getStatus(order.isPaid) }}</td>
          <td>
            <div
              v-if="!order.isPaid"
              class="lni lni-checkmark-circle order-complete"
              @click="markComplete(order.id)"
            ></div>
          </td>
        </tr>
      </thead>
    </table>
  </div>
  <!-- <div v-for="order in orders" :key="order.id">
    <table class="table">
      <thead>
        <tr>
          <th>Customer</th>
          <th>Address</th>
          <th>Item</th>
          <th>Quantity</th>
          <th>Price</th>
        </tr>
      </thead>
      <tr v-for="item in order.salesOrderItems" :key="item.product?.id">
        <td>{{ order.customer.firstName + " " + order.customer.lastName }}</td>
        <td>
          {{
            order.customer.primaryAddress.addressLine1 +
            " " +
            order.customer.primaryAddress.addressLine2
          }}
        </td>
        <td>{{ item.product?.name }}</td>
        <td>{{ item.quantity }}</td>
        <td>{{ item.product?.price }}</td>
      </tr>
      <tr>
        <th colspan="4"></th>
        <th>Grand Total</th>
      </tr>
      <tfoot>
        <tr>
          <td colspan="4"></td>
          <td>{{ runningTotal(order.id) }}</td>
        </tr>
      </tfoot>
    </table>
  </div> -->
</template>

<!-- eslint-disable @typescript-eslint/no-non-null-assertion -->
<script setup lang="ts">
  import { ref, onBeforeMount } from "vue";
  import { OrderService } from "@/services/order-service";
  import { price } from "@/utils/humanize";
  import { ISalesOrder } from "@/types/SalesOrder";
  import { ILineItem } from "@/types/Invoice";

  const orderService = new OrderService();

  const orders = ref<ISalesOrder[]>([]);

  const runningTotal = (id: number) => {
    const lineItems: ILineItem[] = orders.value.find((o) => o.id == id)!.salesOrderItems!;
    return price(lineItems.reduce((a, b) => a + b.product!.price * b.quantity, 0));
  };

  function getStatus(isPaid: boolean) {
    return isPaid ? "Paid in Full" : "Unpaid";
  }

  async function markComplete(orderId: number) {
    await orderService.markOrderComplete(orderId);
    initialize();
  }

  async function initialize() {
    orders.value = await orderService.getOrders();
  }

  onBeforeMount(async () => {
    await initialize();
  });
</script>

<style scoped lang="scss">
  @import "@/scss/global.scss";
</style>
