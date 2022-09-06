<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Inventory Dashboard</h1>
    <hr />

    <div class="inventory-actions">
      <solar-button id="addNewBtn" @click="showNewProductModal">Add New Item</solar-button>
      <solar-button id="receiveShipmentBtn" @click="showShipmentModal">
        Receive Shipment
      </solar-button>
    </div>

    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>

      <tr v-for="item in inventory" :key="item.id">
        <td>{{ item.product.name }}</td>
        <td>{{ item.quantityOnHand }}</td>
        <td>{{ price(item.product.price) }}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td><div>X</div></td>
      </tr>
    </table>

    <new-product-modal
      v-if="isNewProductVisible"
      @close="closeModals"
      @save:product="saveNewProduct"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @close="closeModals"
      @save:shipment="saveNewShipment"
    />
  </div>
</template>
<script setup lang="ts">
  import { ref } from "vue";
  import newProductModal from "@/components/modals/NewProduct.vue";
  import shipmentModal from "@/components/modals/ShipmentModal.vue";
  import solarButton from "@/components/SolarButton.vue";
  import { IProductInventory, IProduct } from "../types/Product";
  import { IShipment } from "@/types/Shipment";

  const now = new Date();
  const isNewProductVisible = ref(false);
  const isShipmentVisible = ref(false);

  const inventory: IProductInventory[] = [
    {
      id: 1,
      product: {
        id: 1,
        name: "Some Product",
        description: "Good Stuff",
        price: 100,
        createdOn: now,
        updatedOn: now,
        isTaxable: true,
        isArchived: false,
      },
      quantityOnHand: 100,
      idealQuantity: 100,
    },
    {
      id: 2,
      product: {
        id: 2,
        name: "Another Product",
        description: "Good Stuff",
        price: 200,
        createdOn: now,
        updatedOn: now,
        isTaxable: false,
        isArchived: false,
      },
      quantityOnHand: 10,
      idealQuantity: 1000,
    },
  ];

  const price = (num: number) => {
    if (isNaN(num)) {
      return "-";
    }
    return "$ " + num.toFixed(2);
  };

  function showNewProductModal() {
    isNewProductVisible.value = true;
  }

  function showShipmentModal() {
    isShipmentVisible.value = true;
  }

  function closeModals() {
    isShipmentVisible.value = false;
    isNewProductVisible.value = false;
  }

  function saveNewProduct(product: IProduct) {
    // const prodInv: IProductInventory = {
    //   id: product.id,
    //   product: product,
    //   idealQuantity: 0,
    //   quantityOnHand: 0,
    // };
    // inventory.push(prodInv);
    console.log("product: ", product);
  }

  function saveNewShipment(shipment: IShipment) {
    console.log("shipment: ", shipment);
  }
</script>
<style scoped lang="scss">
  @import "@/scss/global.scss";

  .inventory-actions {
    display: flex;
  }
</style>
