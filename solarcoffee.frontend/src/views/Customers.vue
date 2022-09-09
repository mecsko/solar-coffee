<template>
  <div>
    <h1 id="customersTitle">Manage Customers</h1>
    <hr />
    <div class="customer-actions">
      <solar-button @button:click="showNewCustomerModal">Add Customer</solar-button>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>State</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>{{ customer.firstName + " " + customer.lastName }}</td>
        <td>
          {{ customer.primaryAddress.addressLine1 + " " + customer.primaryAddress.addressLine2 }}
        </td>
        <td>{{ customer.primaryAddress.state }}</td>
        <td>{{ date(customer.createdOn) }}</td>
        <td>
          <div
            class="lni lni-cross-circle customer-delete"
            @click="deleteCustomer(customer.id!)"
          ></div>
        </td>
      </tr>
    </table>
  </div>
  <new-customer-modal
    v-if="isCustomerModalVisible"
    @close="closeModal"
    @save:customer="saveCustomer"
  />
</template>

<script setup lang="ts">
  import { onBeforeMount, ref } from "vue";
  import solarButton from "@/components/SolarButton.vue";
  import newCustomerModal from "@/components/modals/NewCustomerModal.vue";
  import { CustomerService } from "@/servives/customer-service";
  import { date } from "@/utils/humanize";
  import { ICustomer } from "@/types/Customer";

  const customerService = new CustomerService();

  const isCustomerModalVisible = ref(false);
  const customers = ref<ICustomer[]>([]);

  function showNewCustomerModal() {
    isCustomerModalVisible.value = true;
  }

  function closeModal() {
    isCustomerModalVisible.value = false;
  }

  async function saveCustomer(customer: ICustomer) {
    await customerService.addCustomer(customer);
    closeModal();
    await initialize();
  }

  async function deleteCustomer(customerId: number) {
    (await customerService.deleteCustomer(customerId)).data;
    await initialize();
  }

  async function initialize() {
    customers.value = (await customerService.getCustomers()).data;
  }

  onBeforeMount(async () => {
    await initialize();
  });
</script>

<style scoped lang="scss">
  @import "@/scss/global.scss";

  .customer-actions {
    display: flex;
    margin-bottom: 0.8rem;
  }

  div {
    margin-right: 0.8rem;
  }

  .customer-delete {
    cursor: pointer;
    font-weight: bold;
    font-size: 1.2rem;
    color: $solar-red;
  }
</style>
