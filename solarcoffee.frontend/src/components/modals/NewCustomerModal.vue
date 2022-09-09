<template>
  <solar-modal>
    <template #header>Add New Customer</template>
    <template #body>
      <ul class="newCustomer">
        <li>
          <label for="firstName">First Name</label>
          <input id="firstName" v-model="customer!.firstName" type="text" />
        </li>
        <li>
          <label for="lastName">Last Name</label>
          <input id="lastName" v-model="customer!.lastName" type="text" />
        </li>
        <li>
          <label for="address1">Address Line 1</label>
          <input id="address1" v-model="customer!.primaryAddress.addressLine1" type="text" />
        </li>
        <li>
          <label for="address2">Address Line 2</label>
          <input id="address2" v-model="customer!.primaryAddress.addressLine2" type="text" />
        </li>
        <li>
          <label for="city">City</label>
          <input id="city" v-model="customer!.primaryAddress.city" type="text" />
        </li>
        <li>
          <label for="state">State</label>
          <input id="state" v-model="customer!.primaryAddress.state" type="text" />
        </li>
        <li>
          <label for="postalCode">Postal Code</label>
          <input id="postalCode" v-model="customer!.primaryAddress.postalCode" type="text" />
        </li>
        <li>
          <label for="country">Country</label>
          <input id="country" v-model="customer!.primaryAddress.country" type="text" />
        </li>
      </ul>
    </template>
    <template #footer>
      <solar-button aria-label="save new customer" type="click" @button:click="save">
        Save
      </solar-button>
      <solar-button aria-label="close modal" type="click" @button:click="close">Close</solar-button>
    </template>
  </solar-modal>
</template>

<script setup lang="ts">
  import { ICustomer } from "@/types/Customer";
  import { reactive } from "vue";
  import SolarButton from "../SolarButton.vue";
  import SolarModal from "./SolarModal.vue";

  const customer = reactive<ICustomer>({
    createdOn: new Date(),
    firstName: "",
    lastName: "",
    updatedOn: new Date(),
    primaryAddress: {
      addressLine1: "",
      addressLine2: "",
      city: "",
      country: "",
      createdOn: new Date(),
      updatedOn: new Date(),
      postalCode: "",
      state: "",
    },
  });

  const emits = defineEmits<{
    (event: "save:customer", newCustomer: ICustomer): void;
    (event: "close"): void;
  }>();

  function save() {
    console.log(customer);
    emits("save:customer", customer);
  }

  function close() {
    emits("close");
  }
</script>

<style scoped lang="scss">
  .newCustomer {
    display: flex;
    flex-wrap: wrap;
    list-style: none;
    padding: 0;
    margin: 0;

    input {
      width: 80%;
      height: 1.8rem;
      margin: 0.8rem;
      margin-right: 2rem;
      font-size: 1.1rem;
      line-height: 1.3rem;
      padding: 0.2rem;
      color: #555;
    }

    label {
      font-weight: bold;
      margin: 0.8rem;
      display: block;
    }
  }
</style>
