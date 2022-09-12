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
              class="lni lni-checkmark-circle order-complete green"
              @click="markComplete(order.id)"
            ></div>
          </td>
        </tr>
      </thead>
    </table>
  </div>
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

  .green {
    font-weight: bold;
    color: $solar-green;
  }

  .order-complete {
    cursor: pointer;
  }
</style>
