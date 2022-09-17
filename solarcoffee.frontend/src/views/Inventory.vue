<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Inventory Dashboard</h1>
    <hr />

    <inventory-chart :items="inventory" />

    <div class="inventory-actions">
      <solar-button id="addNewBtn" @button:click="showNewProductModal">Add New Item</solar-button>
      <solar-button id="receiveShipmentBtn" @button:click="showShipmentModal">
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
        <td :class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">
          {{ item.quantityOnHand }}
        </td>
        <td>{{ price(item.product.price) }}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td>
          <div
            class="lni lni-cross-circle product-archive"
            @click="archiveProduct(item.product.id)"
          ></div>
        </td>
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
  import { ref, onMounted } from "vue";
  import { useStore } from "@/store/index";
  import newProductModal from "@/components/modals/NewProduct.vue";
  import shipmentModal from "@/components/modals/ShipmentModal.vue";
  import solarButton from "@/components/SolarButton.vue";
  import InventoryChart from "@/components/charts/InventoryChart.vue";
  import { InventoryService } from "@/services/inventory-service";
  import { ProductService } from "@/services/product-service";
  import { price } from "@/utils/humanize";
  import { IProductInventory, IProduct } from "../types/Product";
  import { IShipment } from "@/types/Shipment";

  const inventoryService = new InventoryService();
  const productService = new ProductService();
  const store = useStore();

  const isNewProductVisible = ref(false);
  const isShipmentVisible = ref(false);
  const inventory = ref<IProductInventory[]>([]);

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

  function applyColor(current: number, target: number) {
    if (current <= 0) {
      return "red";
    } else if (Math.abs(target - current) > 8) {
      return "yellow";
    }
    return "green";
  }

  async function archiveProduct(productId: number) {
    await productService.archive(productId);
    await initialize();
  }

  async function saveNewProduct(newProduct: IProduct) {
    await productService.save(newProduct);
    isNewProductVisible.value = false;
    await initialize();
  }

  async function saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment);
    isShipmentVisible.value = false;
    await initialize();
  }

  async function initialize() {
    inventory.value = await inventoryService.getInventory();
    await store.getSnapshots();
  }

  onMounted(async () => {
    await initialize();
  });
</script>
<style scoped lang="scss">
  @import "@/scss/global.scss";

  .inventory-actions {
    display: flex;
    margin-bottom: 0.8rem;
  }

  .green {
    font-weight: bold;
    color: $solar-green;
  }

  .yellow {
    font-weight: bold;
    color: $solar-yellow;
  }

  .red {
    font-weight: bold;
    color: $solar-red;
  }

  .product-archive {
    cursor: pointer;
    font-weight: bold;
    font-size: 1.2rem;
    color: $solar-red;
  }
</style>
